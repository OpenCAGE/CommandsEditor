using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.UserControls;
using System;
using System.Collections.Generic;

namespace CommandsEditor
{
    //Wrappers around CathodeLib utils, and some utils for formatting strings
    static class EditorUtils
    {
        //New hotfix for link to Editor loaded data
        private static Editor Editor;
        public static void SetEditor(CommandsEditor editor)
        {
            Editor = editor.Loaded;
        }

        /* Utility: generate nice entity name to display in UI */
        public static string GenerateEntityName(Entity entity, Composite composite, bool regenCache = false)
        {
            if (Editor.commands == null) 
                return entity.shortGUID.ToByteString();

            if (hasFinishedCachingEntityNames && regenCache)
            {
                if (!cachedEntityName.ContainsKey(composite.shortGUID)) cachedEntityName.Add(composite.shortGUID, new Dictionary<ShortGuid, string>());

                if (cachedEntityName[composite.shortGUID].ContainsKey(entity.shortGUID)) cachedEntityName[composite.shortGUID].Remove(entity.shortGUID);
                cachedEntityName[composite.shortGUID].Add(entity.shortGUID, GenerateEntityNameInternal(entity, composite));
            }

            if (hasFinishedCachingEntityNames && cachedEntityName[composite.shortGUID].ContainsKey(entity.shortGUID)) 
                return cachedEntityName[composite.shortGUID][entity.shortGUID];

            return GenerateEntityNameInternal(entity, composite);
        }
        public static string GenerateEntityNameWithoutID(Entity entity, Composite composite, bool regenCache = false)
        {
            return GenerateEntityName(entity, composite, regenCache).Substring(14);
        }
        private static string GenerateEntityNameInternal(Entity entity, Composite composite)
        {
            string desc = "";
            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    desc = "[" + ((VariableEntity)entity).type.ToString() + " VARIABLE] " + ShortGuidUtils.FindString(((VariableEntity)entity).name);
                    break;
                case EntityVariant.FUNCTION:
                    Composite funcComposite = Editor.commands.GetComposite(((FunctionEntity)entity).function);
                    if (funcComposite != null)
                        desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + funcComposite.name + ")";
                    else
                        desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + ShortGuidUtils.FindString(((FunctionEntity)entity).function) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    CommandsUtils.ResolveHierarchy(Editor.commands, composite, ((OverrideEntity)entity).connectedEntity.hierarchy, out Composite c, out string s, false);
                    desc = "[OVERRIDE] " + s;
                    break;
                case EntityVariant.PROXY:
                    CommandsUtils.ResolveHierarchy(Editor.commands, composite, ((ProxyEntity)entity).connectedEntity.hierarchy, out Composite c2, out string s2, false);
                    desc = "[PROXY] " + EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + s2 + ")";
                    break;
            }
            return "[" + entity.shortGUID.ToByteString() + "] " + desc;
        }

        /* Generate a cache of entity names to save re-generating them every time */
        private static bool hasFinishedCachingEntityNames = false;
        private static Dictionary<ShortGuid, Dictionary<ShortGuid, string>> cachedEntityName = new Dictionary<ShortGuid, Dictionary<ShortGuid, string>>();
        public static void GenerateEntityNameCache(CommandsEditor mainInst)
        {
            if (Editor.commands == null) return;
            hasFinishedCachingEntityNames = false;
            mainInst.EnableLoadingOfPaks(false);
            cachedEntityName.Clear();
            for (int i = 0; i < Editor.commands.Entries.Count; i++)
            {
                Composite comp = Editor.commands.Entries[i];
                cachedEntityName.Add(comp.shortGUID, new Dictionary<ShortGuid, string>());
                List<Entity> ents = comp.GetEntities();
                for (int x = 0; x < ents.Count; x++)
                    cachedEntityName[comp.shortGUID].Add(ents[x].shortGUID, GenerateEntityNameInternal(ents[x], comp));
            }
            mainInst.EnableLoadingOfPaks(true);
            hasFinishedCachingEntityNames = true;
        }

        /* Utility: generate a list of suggested parameters for an entity */
        public static List<string> GenerateParameterList(Entity entity)
        {
            List<string> items = new List<string>();
            if (Editor.commands == null || entity == null) return items;
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    ShortGuid function = ((FunctionEntity)entity).function;
                    bool isComposite = !CommandsUtils.FunctionTypeExists(function);
                    if (isComposite) function = CommandsUtils.GetFunctionTypeGUID(FunctionType.CompositeInterface);

                    List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(function);
                    if (parameters != null)
                        for (int i = 0; i < parameters.Count; i++) 
                            items.Add(parameters[i].name);

                    if (!isComposite) break;

                    foreach (VariableEntity ent in Editor.commands.GetComposite(((FunctionEntity)entity).function).variables)
                        if (!items.Contains(ent.name.ToString()))
                            items.Add(ent.name.ToString());
                    break;
                case EntityVariant.VARIABLE:
                    items.Add(ShortGuidUtils.FindString(((VariableEntity)entity).name));
                    break;
                case EntityVariant.OVERRIDE:
                    return GenerateParameterList(CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((OverrideEntity)entity).connectedEntity.hierarchy, out Composite comp1, out string hierarchy1));
                case EntityVariant.PROXY:
                    return GenerateParameterList(CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((ProxyEntity)entity).connectedEntity.hierarchy, out Composite comp2, out string hierarchy2));
            }
            items.Sort();
            return items;
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

        /* Utility: work out if any proxies/overrides reference the currently selected entity */
        public static bool IsSelectedEntityReferencedExternally()
        {
            foreach (Composite comp in Editor.commands.Entries)
            {
                foreach (OverrideEntity ovr in comp.overrides)
                {
                    Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, ovr.connectedEntity.hierarchy, out Composite compRef, out string str);
                    if (ent != Editor.selected.entity) continue;
                    return true;
                }
                foreach (ProxyEntity prox in comp.proxies)
                {
                    Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, prox.connectedEntity.hierarchy, out Composite compRef, out string str);
                    if (ent != Editor.selected.entity) continue;
                    return true;
                }
                foreach (TriggerSequence trig in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence)))
                {
                    foreach (TriggerSequence.Entity trigger in trig.entities)
                    {
                        Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, trigger.connectedEntity.hierarchy, out Composite compRef, out string str);
                        if (ent != Editor.selected.entity) continue;
                        return true;
                    }
                }
                foreach (CAGEAnimation anim in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation)))
                {
                    foreach (CAGEAnimation.Connection connection in anim.connections)
                    {
                        Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, connection.connectedEntity.hierarchy, out Composite compRef, out string str);
                        if (ent != Editor.selected.entity) continue;
                        return true;
                    }
                }
            }
            return false;
        }

        /* Utility: try figure out what zone this entity is in (if any) */
        public static void TryFindZoneForSelectedEntity(out Composite composite, out FunctionEntity zone)
        {
            Func<Composite, FunctionEntity> findZone = comp => {
                if (comp == null) return null;
                foreach (TriggerSequence trig in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence)))
                {
                    foreach (TriggerSequence.Entity trigger in trig.entities)
                    {
                        if (CommandsUtils.ResolveHierarchy(Editor.commands, comp, trigger.connectedEntity.hierarchy, out Composite compRef, out string str) != Editor.selected.entity) continue;

                        List<FunctionEntity> zones = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.Zone));
                        foreach (FunctionEntity z in zones)
                        {
                            foreach (EntityLink link in z.childLinks)
                            {
                                if (link.parentParamID != ShortGuidUtils.Generate("composites")) continue;
                                if (link.childID != trig.shortGUID) continue;
                                return z;
                            }
                        }
                    }
                }
                return null;
            };

            composite = Editor.selected.composite;
            zone = findZone(composite);
            if (zone != null) return;

            foreach (Composite comp in Editor.commands.Entries)
            {
                composite = comp;
                zone = findZone(composite);
                if (zone != null) return;
            }
        }
    }
}
