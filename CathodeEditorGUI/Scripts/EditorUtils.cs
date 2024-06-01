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
        private Dictionary<ShortGuid, List<List<ShortGuid>>> _compositeInstancePaths = new Dictionary<ShortGuid, List<List<ShortGuid>>>();
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
                _compositeInstancePaths.Add(composite.shortGUID, new List<List<ShortGuid>>());

            _compositeInstancePaths[composite.shortGUID].Add(hierarchy);

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

        /* Get all possible hierarchies for a given entity */
        public List<EntityPath> GetHierarchiesForEntity(Composite composite, Entity entity)
        {
            List<EntityPath> formattedHierarchies = new List<EntityPath>();
            if (_compositeInstancePaths.ContainsKey(composite.shortGUID))
            {
                List<List<ShortGuid>> hierarchies = _compositeInstancePaths[composite.shortGUID];
                for (int i = 0; i < hierarchies.Count; i++)
                {
                    List<ShortGuid> hierarchy = new List<ShortGuid>(hierarchies[i].ConvertAll(x => x));
                    if (hierarchy.Count != 0 && hierarchy[hierarchy.Count - 1] == ShortGuid.Invalid)
                        hierarchy.RemoveAt(hierarchy.Count - 1);
                    hierarchy.Add(entity.shortGUID);
                    formattedHierarchies.Add(new EntityPath(hierarchy));
                }
            }
            return formattedHierarchies;
        }

#if DEBUG
        //TEST ONLY
        public (Composite, EntityPath) GetCompositeFromInstanceID(Commands commands, ShortGuid instanceID)
        {
            if (instanceID == ShortGuid.InitialiserBase)
                return (commands.EntryPoints[0], new EntityPath());

            foreach (KeyValuePair<ShortGuid, List<List<ShortGuid>>> compositeInstancePaths in _compositeInstancePaths)
            {
                foreach (List<ShortGuid> path in compositeInstancePaths.Value)
                {
                    if (path.Count == 0) continue;
                    if (path.Count == 1 && path[0] == ShortGuid.Invalid) continue;

                    List<ShortGuid> pathFaked = new List<ShortGuid>(path.ConvertAll(x => x));
                    pathFaked.Add(new ShortGuid("01-00-00-00")); //NOTE: need to add a faked entity ID in here so we can generate the instance
                    EntityPath pathFormatted = new EntityPath(pathFaked);
                    if (pathFormatted.GenerateInstance() == instanceID)
                        return (commands.GetComposite(compositeInstancePaths.Key), new EntityPath(path));
                }
            }
            return (null, null);
        }

        //TEST ONLY
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
#endif

        /* Get the hierarchy for a commands entity reference (used to link legacy resource/mvr stuff) */
        public EntityPath GetHierarchyFromHandle(EntityHandle reference)
        {
            EntityPath toReturn = null;
            Parallel.ForEach(_compositeInstancePaths, (pair, state) =>
            {
                if (toReturn != null) state.Stop();
                else
                {
                    Parallel.For(0, pair.Value.Count, (i, state2) =>
                    {
                        if (toReturn != null) state2.Stop();
                        else
                        {
                            List<ShortGuid> hierarchy = new List<ShortGuid>(pair.Value[i].ConvertAll(x => x));
                            if (hierarchy[hierarchy.Count - 1] == ShortGuid.Invalid)
                                hierarchy.RemoveAt(hierarchy.Count - 1);
                            hierarchy.Add(reference.entity_id);

                            EntityPath h = new EntityPath(hierarchy);
                            ShortGuid instance = h.GenerateInstance();

                            if (instance == reference.composite_instance_id)
                                toReturn = h;
                        }
                    });
                }
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
            mainInst.EnableLoadingOfPaks(false, "Generating caches...");
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
            mainInst.EnableLoadingOfPaks(true, "");
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
                        items.Add(ParameterDefinitionToListViewItem("reference"));

                        List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(function);
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Count; i++)
                            {
                                items.Add(ParameterDefinitionToListViewItem(parameters[i].name, parameters[i].datatype, parameters[i].usage));
                            }
                        }

                        if (!isComposite) 
                            break;

                        foreach (VariableEntity ent in _content.commands.GetComposite(((FunctionEntity)entity).function).variables)
                        {
                            if (items.FirstOrDefault(o => o.Text == ent.name.ToString()) == null)
                            {
                                items.Add(ParameterDefinitionToListViewItem(ent.name.ToString(), ent.type.ToString()));
                            }
                        }
                    }
                    break;
                case EntityVariant.VARIABLE:
                    VariableEntity varEnt = (VariableEntity)entity;
                    items.Add(ParameterDefinitionToListViewItem(ShortGuidUtils.FindString(varEnt.name), varEnt.type.ToString()));
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
                                items.Add(ParameterDefinitionToListViewItem(parameters[i].name, parameters[i].datatype, parameters[i].usage));
                            }
                        }
                    }
                    break;
            }
            return items;
        }
        private ListViewItem ParameterDefinitionToListViewItem(string name, string datatype = "FLOAT", CathodeEntityDatabase.ParameterUsage usage = CathodeEntityDatabase.ParameterUsage.PARAMETER)
        {
            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add(datatype);
            item.Tag = usage;
            return item;
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
    }
}
