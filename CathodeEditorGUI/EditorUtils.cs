using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using System;
using System.Collections.Generic;

namespace CathodeEditorGUI
{
    //Wrappers around CathodeLib utils, and some utils for formatting strings
    static class EditorUtils
    {
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
                    desc = ShortGuidUtils.FindString(((VariableEntity)entity).name) + " (DataType " + ((VariableEntity)entity).type.ToString() + ")";
                    break;
                case EntityVariant.FUNCTION:
                    Composite funcComposite = Editor.commands.GetComposite(((FunctionEntity)entity).function);
                    if (funcComposite != null)
                        desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + funcComposite.name + ")";
                    else
                        desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (" + ShortGuidUtils.FindString(((FunctionEntity)entity).function) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((OverrideEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (*OVERRIDE*)";
                    break;
                case EntityVariant.PROXY:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((ProxyEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = EntityUtils.GetName(composite.shortGUID, entity.shortGUID) + " (*PROXY*)";
                    break;
            }
            return "[" + entity.shortGUID.ToByteString() + "] " + desc;
        }

        /* Generate a cache of entity names to save re-generating them every time */
        private static bool hasFinishedCachingEntityNames = false;
        private static Dictionary<ShortGuid, Dictionary<ShortGuid, string>> cachedEntityName = new Dictionary<ShortGuid, Dictionary<ShortGuid, string>>();
        public static void GenerateEntityNameCache(CathodeEditorGUI mainInst)
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
                    if (CommandsUtils.FunctionTypeExists(function))
                    {
                        //Function node
                        List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(function);
                        if (parameters == null) break;
                        for (int i = 0; i < parameters.Count; i++) 
                            items.Add(parameters[i].name);
                    }
                    else
                    {
                        //Composite node
                        foreach (VariableEntity ent in Editor.commands.GetComposite(function).variables)
                            items.Add(ShortGuidUtils.FindString(ent.name));
                    }
                    break;
                case EntityVariant.VARIABLE:
                    items.Add(ShortGuidUtils.FindString(((VariableEntity)entity).name));
                    break;
                case EntityVariant.OVERRIDE:
                    return GenerateParameterList(CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((OverrideEntity)entity).hierarchy, out Composite comp1, out string hierarchy1));
                case EntityVariant.PROXY:
                    return GenerateParameterList(CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, ((ProxyEntity)entity).hierarchy, out Composite comp2, out string hierarchy2));
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
    }
}
