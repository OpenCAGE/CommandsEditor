using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;

namespace CommandsEditor
{
    //Wrappers around CathodeLib utils, and some utils for formatting strings
    public class EditorUtils
    {
        private LevelContent _content;
        public EditorUtils(LevelContent content)
        {
            _content = content;
        }

        /* Some additional composite info for rich display in editor */
        public enum CompositeType
        {
            IS_GENERIC_COMPOSITE,
            IS_ROOT,
            IS_PAUSE_MENU,
            IS_GLOBAL,
            IS_DISPLAY_MODEL,
        }
        public CompositeType GetCompositeType(Composite composite)
        {
            return GetCompositeType(composite.name);
        }
        public CompositeType GetCompositeType(string composite)
        {
            string c = composite.Replace('/', '\\');
            if (_content.commands.EntryPoints[0].name.Replace('/', '\\') == c) return CompositeType.IS_ROOT;
            if (_content.commands.EntryPoints[1].name.Replace('/', '\\') == c) return CompositeType.IS_PAUSE_MENU;
            if (_content.commands.EntryPoints[2].name.Replace('/', '\\') == c) return CompositeType.IS_GLOBAL;
            if (c.Length > ("DisplayModel:").Length && c.Substring(0, ("DisplayModel:").Length) == "DisplayModel:") return CompositeType.IS_DISPLAY_MODEL;
            return CompositeType.IS_GENERIC_COMPOSITE;
        }

        /* Generate all composite instance information for Commands */
        private Dictionary<ShortGuid, List<Tuple<ShortGuid, List<ShortGuid>>>> _compositeInstancePaths = new Dictionary<ShortGuid, List<Tuple<ShortGuid, List<ShortGuid>>>>();
        private CancellationTokenSource _prevTaskToken = null;
        public void GenerateCompositeInstances(Commands commands, bool runOnThread = true)
        {
            if (_prevTaskToken != null)
                _prevTaskToken.Cancel();

            _compositeInstancePaths.Clear();

            _prevTaskToken = new CancellationTokenSource();

            if (runOnThread)
                Task.Run(() => _content.editor_utils.GenerateCompositeInstancesRecursive(commands, commands.EntryPoints[0], new List<ShortGuid>(), _prevTaskToken.Token), _prevTaskToken.Token);
            else
                _content.editor_utils.GenerateCompositeInstancesRecursive(commands, commands.EntryPoints[0], new List<ShortGuid>(), _prevTaskToken.Token);
        }
        private void GenerateCompositeInstancesRecursive(Commands commands, Composite composite, List<ShortGuid> hierarchy, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return;

            if (!_compositeInstancePaths.ContainsKey(composite.shortGUID))
                _compositeInstancePaths.Add(composite.shortGUID, new List<Tuple<ShortGuid, List<ShortGuid>>>());

            _compositeInstancePaths[composite.shortGUID].Add(new Tuple<ShortGuid, List<ShortGuid>>(hierarchy.GenerateCompositeInstanceID(false), hierarchy));

            for (int i = 0; i < composite.functions.Count; i++)
            {
                if (CommandsUtils.FunctionTypeExists(composite.functions[i].function)) continue;

                if (ct.IsCancellationRequested) break;

                List<ShortGuid> newHierarchy = new List<ShortGuid>(hierarchy.ConvertAll(x => x));
                newHierarchy.Add(composite.functions[i].shortGUID);

                Composite newComposite = commands.GetComposite(composite.functions[i].function);
                if (newComposite != null) GenerateCompositeInstancesRecursive(commands, newComposite, newHierarchy, ct);
            }
        }

        /* Get all possible instance IDs for a given composite */
        public ShortGuid[] GetInstanceIDsForComposite(Composite composite)
        {
            if (!_compositeInstancePaths.ContainsKey(composite.shortGUID))
                return new ShortGuid[0];

            List<Tuple<ShortGuid, List<ShortGuid>>> hierarchies = _compositeInstancePaths[composite.shortGUID];
            ShortGuid[] instanceIDs = new ShortGuid[hierarchies.Count];
            for (int i = 0; i < hierarchies.Count; i++)
                instanceIDs[i] = hierarchies[i].Item1;
            return instanceIDs;
        }

        /* Get all possible hierarchies for a given entity */
        public List<EntityPath> GetHierarchiesForEntity(Composite composite, Entity entity)
        {
            List<EntityPath> formattedHierarchies = new List<EntityPath>();
            if (_compositeInstancePaths.ContainsKey(composite.shortGUID))
            {
                List<Tuple<ShortGuid, List<ShortGuid>>> hierarchies = _compositeInstancePaths[composite.shortGUID];
                for (int i = 0; i < hierarchies.Count; i++)
                {
                    List<ShortGuid> hierarchy = new List<ShortGuid>(hierarchies[i].Item2);
                    if (hierarchy.Count != 0 && hierarchy[hierarchy.Count - 1] == ShortGuid.Invalid)
                        hierarchy.RemoveAt(hierarchy.Count - 1); 
                    hierarchy.Add(entity.shortGUID);
                    formattedHierarchies.Add(new EntityPath(hierarchy));
                }
            }
            return formattedHierarchies;
        }

        /* Get a composite (& instance path) from a given composite instance ID */
        public (Composite, EntityPath) GetCompositeFromInstanceID(Commands commands, ShortGuid instanceID)
        {
            if (instanceID == ShortGuid.InitialiserBase)
                return (commands.EntryPoints[0], new EntityPath());

            (Composite compositeResult, EntityPath entityPathResult) = (null, null);
            object lockObject = new object();
            bool found = false;

            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Parallel.ForEach(_compositeInstancePaths, parallelOptions, (compositeInstancePaths, state) =>
            {
                if (found) return;

                foreach (Tuple<ShortGuid, List<ShortGuid>> path in compositeInstancePaths.Value)
                {
                    if (path.Item2.Count == 0 || (path.Item2.Count == 1 && path.Item2[0] == ShortGuid.Invalid))
                        continue;

                    if (path.Item1 == instanceID)
                    {
                        lock (lockObject)
                        {
                            if (!found)
                            {
                                compositeResult = commands.GetComposite(compositeInstancePaths.Key);
                                entityPathResult = new EntityPath(path.Item2);
                                found = true;

                                cancellationTokenSource.Cancel();
                                state.Stop();
                            }
                        }
                    }
                }
            });
            return (compositeResult, entityPathResult);
        }

        [Obsolete("This function is safe to use but not performant. It's intended for test code only.")]
        public (Composite, EntityPath, Entity) GetZoneFromInstanceID(Commands commands, ShortGuid instanceID)
        {
            if (instanceID == new ShortGuid("01-00-00-00"))
            {
                //global zone
                return (null, new EntityPath(new List<ShortGuid>() { new ShortGuid("01-00-00-00") }), null);
            }
            if (instanceID == new ShortGuid("00-00-00-00"))
            {
                //global zone
                return (null, new EntityPath(new List<ShortGuid>() { new ShortGuid("00-00-00-00") }), null);
            }

            ShortGuid GUID_Zone = CommandsUtils.GetFunctionTypeGUID(FunctionType.Zone);
            for (int i = 0; i < commands.Entries.Count; i++)
            {
                for (int x = 0; x < commands.Entries[i].functions.Count; x++)
                {
                    if (commands.Entries[i].functions[x].function != GUID_Zone)
                        continue;

                    List<EntityPath> zonePaths = GetHierarchiesForEntity(commands.Entries[i], commands.Entries[i].functions[x]);
                    for (int p = 0; p < zonePaths.Count; p++)
                    {
                        if (zonePaths[p].GenerateZoneID() == instanceID)
                        {
                            return (commands.Entries[i], zonePaths[p], commands.Entries[i].functions[x]);
                        }
                    }
                }
            }
            return (null, null, null);
        }

        /* Get the hierarchy for a commands entity reference (used to link legacy resource/mvr stuff) */
        public EntityPath GetHierarchyFromHandle(EntityHandle reference)
        {
            EntityPath toReturn = null;
            object lockObj = new object(); 

            Parallel.ForEach(_compositeInstancePaths, (pair, state) =>
            {
                foreach (var instance in pair.Value)
                {
                    if (instance.Item1 == reference.composite_instance_id)
                    {
                        List<ShortGuid> hierarchy = new List<ShortGuid>(instance.Item2);
                        if (hierarchy.Count > 0 && hierarchy[hierarchy.Count - 1] == ShortGuid.Invalid)
                            hierarchy.RemoveAt(hierarchy.Count - 1);
                        hierarchy.Add(reference.entity_id);

                        lock (lockObj)
                        {
                            if (toReturn == null)
                                toReturn = new EntityPath(hierarchy);
                        }

                        state.Stop(); 
                        break;
                    }
                }

                if (toReturn != null)
                    state.Stop(); 
            });

            return toReturn;
        }


        /* Utility: generate nice entity name to display in UI */
        public string GenerateEntityName(Entity entity, Composite composite, bool regenCache = false)
        {
            if (_content.commands == null)
                return entity.shortGUID.ToByteString();

            if (hasFinishedCachingEntityNames && regenCache)
            {
                if (!cachedEntityName.ContainsKey(composite.shortGUID)) cachedEntityName.Add(composite.shortGUID, new Dictionary<ShortGuid, string>());

                if (cachedEntityName[composite.shortGUID].ContainsKey(entity.shortGUID)) cachedEntityName[composite.shortGUID].Remove(entity.shortGUID);
                cachedEntityName[composite.shortGUID].Add(entity.shortGUID, GenerateEntityNameInternal(entity, composite));
            }

            if (!cachedEntityName.ContainsKey(composite.shortGUID))
                cachedEntityName.Add(composite.shortGUID, new Dictionary<ShortGuid, string>());

            if (hasFinishedCachingEntityNames && cachedEntityName[composite.shortGUID].ContainsKey(entity.shortGUID))
                return cachedEntityName[composite.shortGUID][entity.shortGUID];

            return GenerateEntityNameInternal(entity, composite);
        }
        public string GenerateEntityNameWithoutID(Entity entity, Composite composite, bool regenCache = false)
        {
            return GenerateEntityName(entity, composite, regenCache).Substring(14);
        }
        private string GenerateEntityNameInternal(Entity entity, Composite composite)
        {
            string desc = "";
            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    desc = "[" + ((VariableEntity)entity).type.ToString() + " VARIABLE] " + ShortGuidUtils.FindString(((VariableEntity)entity).name);
                    break;
                case EntityVariant.FUNCTION:
                    Composite funcComposite = _content.commands.GetComposite(((FunctionEntity)entity).function);
                    if (funcComposite != null)
                        desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + funcComposite.name + ")";
                    else
                        desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + CathodeEntityDatabase.GetEntity(((FunctionEntity)entity).function).className + ")";
                    break;
                case EntityVariant.ALIAS:
                    CommandsUtils.ResolveHierarchy(_content.commands, composite, ((AliasEntity)entity).alias.path, out Composite c, out string s, false);
                    desc = "[ALIAS] " + s;
                    break;
                case EntityVariant.PROXY:
                    CommandsUtils.ResolveHierarchy(_content.commands, composite, ((ProxyEntity)entity).proxy.path, out Composite c2, out string s2, false);
                    desc = "[PROXY] " + EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + s2 + ")";
                    break;
            }
            bool showID = SettingsManager.GetBool(Singleton.Settings.EntIdOpt);
            return (showID ? "[" + entity.shortGUID.ToByteString() + "] " : "") + desc;
        }

        /* Generate a cache of entity names */
        private bool hasFinishedCachingEntityNames = false;
        private Dictionary<ShortGuid, Dictionary<ShortGuid, string>> cachedEntityName = new Dictionary<ShortGuid, Dictionary<ShortGuid, string>>();
        public void GenerateEntityNameCache(CommandsEditor mainInst)
        {
            if (_content.commands == null) return;
            hasFinishedCachingEntityNames = false;
            mainInst?.EnableLoadingOfPaks(false, "Generating caches...");
            cachedEntityName.Clear();
            for (int i = 0; i < _content.commands.Entries.Count; i++)
            {
                Composite comp = _content.commands.Entries[i];
                if (!cachedEntityName.ContainsKey(comp.shortGUID))
                    cachedEntityName.Add(comp.shortGUID, new Dictionary<ShortGuid, string>());
                List<Entity> ents = comp.GetEntities();
                for (int x = 0; x < ents.Count; x++)
                    if (!cachedEntityName[comp.shortGUID].ContainsKey(ents[x].shortGUID))
                        cachedEntityName[comp.shortGUID].Add(ents[x].shortGUID, GenerateEntityNameInternal(ents[x], comp));
            }
            mainInst?.EnableLoadingOfPaks(true, "");
            hasFinishedCachingEntityNames = true;
        }

        /* Utility: generate a list of suggested parameters for an entity */
        public List<string> GenerateParameterListAsString(Entity entity, Composite composite)
        {
            List<string> items = new List<string>();
            if (entity == null) return items;
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    {
                        ShortGuid function = ((FunctionEntity)entity).function;
                        bool isComposite = !CommandsUtils.FunctionTypeExists(function);
                        if (isComposite) function = CommandsUtils.GetFunctionTypeGUID(FunctionType.CompositeInterface);
                        items.Add("reference");

                        List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(function);
                        if (parameters != null)
                            for (int i = 0; i < parameters.Count; i++)
                                items.Add(parameters[i].name);

                        if (!isComposite) break;

                        foreach (VariableEntity ent in _content.commands.GetComposite(((FunctionEntity)entity).function).variables)
                            if (!items.Contains(ent.name.ToString()))
                                items.Add(ent.name.ToString());
                    }
                    break;
                case EntityVariant.VARIABLE:
                    items.Add(ShortGuidUtils.FindString(((VariableEntity)entity).name));
                    break;
                case EntityVariant.ALIAS:
                    return GenerateParameterListAsString(CommandsUtils.ResolveHierarchy(_content.commands, composite, ((AliasEntity)entity).alias.path, out Composite comp1, out string hierarchy1), comp1);
                case EntityVariant.PROXY:
                    {
                        items.AddRange(GenerateParameterListAsString(CommandsUtils.ResolveHierarchy(_content.commands, composite, ((ProxyEntity)entity).proxy.path, out Composite comp2, out string hierarchy2), comp2));

                        List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(ShortGuidUtils.Generate("ProxyInterface"));
                        if (parameters != null)
                            for (int i = 0; i < parameters.Count; i++)
                                items.Add(parameters[i].name);
                    }
                    break;
            }
            items.Sort();
            return items;
        }
        public List<ListViewItem> GenerateParameterListAsListViewItem(Entity entity, Composite composite)
        {
            List<ListViewItem> items = new List<ListViewItem>();
            if (entity == null) return items;
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    {
                        ShortGuid function = ((FunctionEntity)entity).function;
                        bool isComposite = !CommandsUtils.FunctionTypeExists(function);
                        if (isComposite) function = CommandsUtils.GetFunctionTypeGUID(FunctionType.CompositeInterface);
                        items.Add(ParameterDefinitionToListViewItem(ShortGuidUtils.Generate("reference")));

                        List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(function);
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                items.Add(ParameterDefinitionToListViewItem(ShortGuidUtils.Generate(parameters[i].name), parameters[i].datatype, parameters[i].usage));
                            }
                        }

                        if (!isComposite) 
                            break;

                        foreach (VariableEntity ent in _content.commands.GetComposite(((FunctionEntity)entity).function).variables)
                        {
                            if (items.FirstOrDefault(o => o.Text == ent.name.ToString()) == null)
                            {
                                items.Add(ParameterDefinitionToListViewItem(ent.name, ent.type.ToString()));
                            }
                        }
                    }
                    break;
                case EntityVariant.VARIABLE:
                    VariableEntity varEnt = (VariableEntity)entity;
                    items.Add(ParameterDefinitionToListViewItem(varEnt.name, varEnt.type.ToString()));
                    break;
                case EntityVariant.ALIAS:
                    return GenerateParameterListAsListViewItem(CommandsUtils.ResolveHierarchy(_content.commands, composite, ((AliasEntity)entity).alias.path, out Composite comp1, out string hierarchy1), comp1);
                case EntityVariant.PROXY:
                    {
                        items.AddRange(GenerateParameterListAsListViewItem(CommandsUtils.ResolveHierarchy(_content.commands, composite, ((ProxyEntity)entity).proxy.path, out Composite comp2, out string hierarchy2), comp2));

                        List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(ShortGuidUtils.Generate("ProxyInterface"));
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                items.Add(ParameterDefinitionToListViewItem(ShortGuidUtils.Generate(parameters[i].name), parameters[i].datatype, parameters[i].usage));
                            }
                        }
                    }
                    break;
            }
            return items;
        }
        private ListViewItem ParameterDefinitionToListViewItem(ShortGuid name, string datatype = "FLOAT", CathodeEntityDatabase.ParameterUsage usage = CathodeEntityDatabase.ParameterUsage.PARAMETER)
        {
            ListViewItem item = new ListViewItem(name.ToString());
            item.SubItems.Add(datatype);
            item.Tag = new ParameterListViewItemTag() { ShortGUID = name, Usage = usage };
            return item;
        }
        public struct ParameterListViewItemTag
        {
            public CathodeEntityDatabase.ParameterUsage Usage;
            public ShortGuid ShortGUID;
        }

        /* Utility: force a string to be numeric */
        public static string ForceStringNumeric(string str, bool allowDots = false)
        {
            string editedText = "";
            bool hasIncludedDot = false;
            bool hasIncludedMinus = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsNumber(str[i]) || (str[i] == '.' && allowDots) || (str[i] == '-'))
                {
                    if (str[i] == '-' && hasIncludedMinus) continue;
                    if (str[i] == '-' && i != 0) continue;
                    if (str[i] == '-') hasIncludedMinus = true;
                    if (str[i] == '.' && hasIncludedDot) continue;
                    if (str[i] == '.') hasIncludedDot = true;
                    editedText += str[i];
                }
            }
            if (editedText == "") editedText = "0";
            if (editedText == "-") editedText = "-0";
            if (editedText == ".") editedText = "0";
            return editedText;
        }

        /* Utility: get composite name */
        public static string GetCompositeName(Composite comp)
        {
            if (comp == null)
                return "";
            string[] cont = comp.name.Replace('\\', '/').Split('/');
            return cont[cont.Length - 1];
        }

        /* Utility: work out if any proxies/overrides reference the currently selected entity */
        public bool IsEntityReferencedExternally(Entity entity, CancellationToken ct)
        {
            bool found = false;
            Parallel.ForEach(_content.commands.Entries, (comp, status) =>
            {
                Parallel.ForEach(comp.proxies, (prox, status2) =>
                {
                    if (found || ct.IsCancellationRequested)
                        status2.Stop();
                    Entity ent = CommandsUtils.ResolveHierarchy(_content.commands, comp, prox.proxy.path, out Composite compRef, out string str);
                    if (ent == entity) found = true;
                });
                Parallel.ForEach(comp.aliases, (alias, status2) =>
                {
                    if (found || ct.IsCancellationRequested)
                        status2.Stop();

                    Entity ent = CommandsUtils.ResolveHierarchy(_content.commands, comp, alias.alias.path, out Composite compRef, out string str);
                    if (ent == entity) found = true;
                });
                List<FunctionEntity> triggerSequences = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence));
                Parallel.ForEach(triggerSequences, (trigEnt, status2) =>
                {
                    if (found || ct.IsCancellationRequested)
                        status2.Stop();

                    TriggerSequence trig = (TriggerSequence)trigEnt;
                    Parallel.ForEach(trig.entities, (trigger, status3) =>
                    {
                        if (found || ct.IsCancellationRequested)
                            status3.Stop();

                        Entity ent = CommandsUtils.ResolveHierarchy(_content.commands, comp, trigger.connectedEntity.path, out Composite compRef, out string str);
                        if (ent == entity) found = true;
                    });
                });
                List<FunctionEntity> cageAnims = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation));
                Parallel.ForEach(cageAnims, (animEnt, status2) =>
                {
                    if (found || ct.IsCancellationRequested)
                        status2.Stop();

                    CAGEAnimation anim = (CAGEAnimation)animEnt;
                    Parallel.ForEach(anim.connections, (connection, status3) =>
                    {
                        if (found || ct.IsCancellationRequested)
                            status3.Stop();

                        Entity ent = CommandsUtils.ResolveHierarchy(_content.commands, comp, connection.connectedEntity.path, out Composite compRef, out string str);
                        if (ent == entity) found = true;
                    });
                });

                if (found || ct.IsCancellationRequested)
                    status.Stop();
            });
            return found;
        }

        /* Utility: try figure out what zone this entity is in (if any) */
        public void TryFindZoneForEntity(Entity entity, Composite startComposite, out Composite composite, out FunctionEntity zone, CancellationToken ct)
        {
            Func<Composite, FunctionEntity> findZone = comp => {
                if (comp == null) return null;

                FunctionEntity toReturn = null;
                ShortGuid compositesGUID = ShortGuidUtils.Generate("composites");

                List<FunctionEntity> triggerSequences = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence));
                Parallel.ForEach(triggerSequences, (Action<FunctionEntity, ParallelLoopState>)((trigEnt, status) =>
                {
                    TriggerSequence trig = (TriggerSequence)trigEnt;
                    Parallel.ForEach(trig.entities, (Action<TriggerSequence.Entity, ParallelLoopState>)((trigger, status2) =>
                    {
                        if (CommandsUtils.ResolveHierarchy((Commands)this._content.commands, comp, trigger.connectedEntity.path, out Composite compRef, out string str) == entity)
                        {
                            List<FunctionEntity> zones = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.Zone));
                            Parallel.ForEach(zones, (z, status3) =>
                            {
                                Parallel.ForEach(z.childLinks, (link, status4) =>
                                {
                                    if (link.thisParamID == compositesGUID && link.linkedEntityID == trig.shortGUID)
                                    {
                                        toReturn = z;

                                        status.Stop();
                                        status2.Stop();
                                        status3.Stop();
                                        status4.Stop();
                                    }

                                    if (ct.IsCancellationRequested)
                                        status4.Stop();
                                });

                                if (ct.IsCancellationRequested)
                                    status3.Stop();
                            });
                        }

                        if (ct.IsCancellationRequested)
                            status2.Stop();
                    }));

                    if (ct.IsCancellationRequested)
                        status.Stop();
                }));

                return toReturn;
            };

            composite = startComposite;
            zone = findZone(composite);
            if (zone != null) return;

            foreach (Composite comp in _content.commands.Entries)
            {
                composite = comp;
                zone = findZone(composite);
                if (zone != null) return;
            }
        }

        [Obsolete("This function is safe to use but not performant. It's intended for test code only.")]
        public string GetAllZonesForEntity(Entity entity)
        {
            ShortGuid compositesGUID = ShortGuidUtils.Generate("composites");
            string toReturn = "";
            List<ShortGuid> foundIDs = new List<ShortGuid>();
            foreach (Composite comp in _content.commands.Entries)
            {
                List<FunctionEntity> triggerSequences = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence));
                foreach (FunctionEntity trigEnt in triggerSequences)
                {
                    TriggerSequence trig = (TriggerSequence)trigEnt;
                    foreach (TriggerSequence.Entity trigger in trig.entities)
                    {
                        if (CommandsUtils.ResolveHierarchy(_content.commands, comp, trigger.connectedEntity.path, out Composite compRef, out string str) == entity)
                        {
                            List<FunctionEntity> zones = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.Zone));
                            foreach (FunctionEntity z in zones)
                            {
                                foreach (EntityConnector link in z.childLinks)
                                {
                                    if (link.thisParamID == compositesGUID && link.linkedEntityID == trig.shortGUID)
                                    {
                                        if (foundIDs.Contains(z.shortGUID))
                                            continue;

                                        Parameter p = z.GetParameter("name");
                                        string name = "";
                                        if (p != null && p.content.dataType == DataType.STRING)
                                            name = ((cString)p.content).value;

                                        foundIDs.Add(z.shortGUID);
                                        toReturn += "\n[Found zone entity '" + z.shortGUID.ToByteString() + "' (" + name + ") in composite '" + comp.name + "']\n";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return toReturn;
        }

        [Obsolete("This function is safe to use but not performant. It's intended for test code only.")]
        public string PrettyPrintMoverRenderable(Movers.MOVER_DESCRIPTOR mvr)
        {
            if (mvr == null)
                return "";

            string output = "";
            for (int x = 0; x < mvr.renderable_element_count; x++)
            {
                var reds = _content.resource.reds.Entries[(int)mvr.renderable_element_index];

                var submesh = _content.resource.models.GetAtWriteIndex(reds.ModelIndex);
                var model = _content.resource.models.FindModelForSubmesh(submesh);
                var component = _content.resource.models.FindModelComponentForSubmesh(submesh);
                var lod = _content.resource.models.FindModelLODForSubmesh(submesh);
                var material = _content.resource.materials.GetAtWriteIndex(reds.MaterialIndex);

                output += model?.Name + " (" + lod?.Name + ") -> " + material?.Name + "\n";
            }
            return output;
        }

        //Util: patch the game exe to launch to the specified map (handy for debugging)
        public static bool PatchLaunchMode(string MapName = "Frontend")
        {
            //This is the level the benchmark function loads into - we can overwrite it to change
            byte[] mapStringByteArray = { 0x54, 0x45, 0x43, 0x48, 0x5F, 0x52, 0x4E, 0x44, 0x5F, 0x48, 0x5A, 0x44, 0x4C, 0x41, 0x42, 0x00, 0x00, 0x65, 0x6E, 0x67, 0x69, 0x6E, 0x65, 0x5F, 0x73, 0x65, 0x74, 0x74, 0x69, 0x6E, 0x67, 0x73 };

            //These are the original/edited setters in the benchmark function to enable benchmark mode - if we're just loading a level, we want to change them
            List<PatchBytes> benchmarkPatches = new List<PatchBytes>();
            switch (SettingsManager.GetString("META_GameVersion"))
            {
                case "STEAM":
                    benchmarkPatches.Add(new PatchBytes(3842041, new byte[] { 0xe3, 0x48, 0x26 }, new byte[] { 0x13, 0x3c, 0x28 }));
                    benchmarkPatches.Add(new PatchBytes(3842068, new byte[] { 0xce, 0x0c, 0x6f }, new byte[] { 0x26, 0x0f, 0x64 }));
                    benchmarkPatches.Add(new PatchBytes(3842146, new byte[] { 0xcb, 0x0c, 0x6f }, new byte[] { 0x26, 0x0f, 0x64 }));
                    //benchmarkPatches.Add(new PatchBytes(3842846, new byte[] { 0x4e, 0x4c, 0x56 }, new byte[] { 0xce, 0xc1, 0x6f })); //skip_frontend
                    //benchmarkPatches.Add(new PatchBytes(4047697, new byte[] { 0x1b, 0x2c, 0x53 }, new byte[] { 0x9b, 0xa1, 0x6c })); //skip_frontend
                    break;
                case "EPIC_GAMES_STORE":
                    benchmarkPatches.Add(new PatchBytes(3911321, new byte[] { 0x13, 0x5f, 0x1a }, new byte[] { 0x23, 0x43, 0x1c }));
                    benchmarkPatches.Add(new PatchBytes(3911348, new byte[] { 0xee, 0xd1, 0x70 }, new byte[] { 0xe6, 0xce, 0x65 }));
                    benchmarkPatches.Add(new PatchBytes(3911426, new byte[] { 0xeb, 0xd1, 0x70 }, new byte[] { 0xe6, 0xce, 0x65 }));
                    //benchmarkPatches.Add(new PatchBytes(3912126, new byte[] { 0x7e, 0xbf, 0x5f, 0x00 }, new byte[] { 0x1e, 0x5b, 0xf3, 0xff })); //skip_frontend
                    //benchmarkPatches.Add(new PatchBytes(4117408, new byte[] { 0x9c, 0x9d, 0x5c, 0x00 }, new byte[] { 0x3c, 0x39, 0xf0, 0xff })); //skip_frontend
                    break;
                case "GOG":
                    benchmarkPatches.Add(new PatchBytes(3842217, new byte[] { 0x33, 0x4b, 0x26 }, new byte[] { 0x13, 0x3c, 0x28 }));
                    benchmarkPatches.Add(new PatchBytes(3842244, new byte[] { 0x0e, 0xaf, 0x70 }, new byte[] { 0x26, 0xaf, 0x65 }));
                    benchmarkPatches.Add(new PatchBytes(3842322, new byte[] { 0x0b, 0xaf, 0x70 }, new byte[] { 0x26, 0xaf, 0x65 }));
                    //benchmarkPatches.Add(new PatchBytes(3843022, new byte[] { 0x0e, 0x43, 0x04 }, new byte[] { 0x8e, 0x29, 0x0b })); //skip_frontend
                    //benchmarkPatches.Add(new PatchBytes(4047514, new byte[] { 0x42, 0x24, 0x01 }, new byte[] { 0xc2, 0x0a, 0x08 })); //skip_frontend
                    break;
            }

            //Frontend acts as a reset
            bool shouldPatch = true;
            if (MapName.ToUpper() == "FRONTEND")
            {
                MapName = "Tech_RnD_HzdLab";
                shouldPatch = false;
            }

            //Update vanilla byte array with selection
            for (int i = 0; i < MapName.Length; i++)
            {
                mapStringByteArray[i] = (byte)MapName[i];
            }
            mapStringByteArray[MapName.Length] = 0x00;

            //Edit game EXE with selected option & hack out the benchmark mode
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(SettingsManager.GetString("PATH_GameRoot") + "/AI.exe")))
                {
                    for (int i = 0; i < benchmarkPatches.Count; i++)
                    {
                        writer.BaseStream.Position = benchmarkPatches[i].offset;
                        if (shouldPatch) writer.Write(benchmarkPatches[i].patched);
                        else writer.Write(benchmarkPatches[i].original);
                    }
                    switch (SettingsManager.GetString("META_GameVersion"))
                    {
                        case "STEAM":
                            writer.BaseStream.Position = 15676275;
                            break;
                        case "EPIC_GAMES_STORE":
                            writer.BaseStream.Position = 15773411;
                            break;
                        case "GOG":
                            writer.BaseStream.Position = 15773451;
                            break;
                    }
                    if (writer.BaseStream.Position != 0)
                        writer.Write(mapStringByteArray);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("PatchManager::PatchLaunchMode - " + e.ToString());
                return false;
            }
        }

        struct PatchBytes
        {
            public PatchBytes(int _o, byte[] _orig, byte[] _patch)
            {
                offset = _o;
                original = _orig;
                patched = _patch;
            }
            public int offset;
            public byte[] original;
            public byte[] patched;
        }
    }
}
