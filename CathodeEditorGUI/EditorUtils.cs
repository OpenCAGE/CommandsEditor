using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using System;
using System.Collections.Generic;

namespace CathodeEditorGUI
{
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
                case EntityVariant.DATATYPE:
                    desc = ShortGuidUtils.FindString(((VariableEntity)entity).name) + " (DataType " + ((VariableEntity)entity).type.ToString() + ")";
                    break;
                case EntityVariant.FUNCTION:
                    Composite funcComposite = Editor.commands.GetComposite(((FunctionEntity)entity).function);
                    if (funcComposite != null)
                        desc = Editor.util.entity.GetName(composite.shortGUID, entity.shortGUID) + " (" + funcComposite.name + ")";
                    else
                        desc = Editor.util.entity.GetName(composite.shortGUID, entity.shortGUID) + " (" + ShortGuidUtils.FindString(((FunctionEntity)entity).function) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((OverrideEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = Editor.util.entity.GetName(composite.shortGUID, entity.shortGUID) + " (*OVERRIDE*)";
                    break;
                case EntityVariant.PROXY:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((ProxyEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = Editor.util.entity.GetName(composite.shortGUID, entity.shortGUID) + " (*PROXY*)";
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
            for (int i = 0; i < Editor.commands.Composites.Count; i++)
            {
                Composite comp = Editor.commands.Composites[i];
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
                case EntityVariant.DATATYPE:
                    items.Add(ShortGuidUtils.FindString(((VariableEntity)entity).name));
                    break;
                case EntityVariant.OVERRIDE:
                    return GenerateParameterList(EditorUtils.ResolveHierarchy(((OverrideEntity)Editor.selected.entity).hierarchy, out Composite comp1, out string hierarchy1));
                case EntityVariant.PROXY:
                    return GenerateParameterList(EditorUtils.ResolveHierarchy(((ProxyEntity)Editor.selected.entity).hierarchy, out Composite comp2, out string hierarchy2));
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

        /* Resolve a node hierarchy */
        public static Entity ResolveHierarchy(List<ShortGuid> hierarchy, out Composite containedFlowgraph, out string asString)
        {
            if (hierarchy.Count == 0)
            {
                containedFlowgraph = null;
                asString = "";
                return null;
            }

            List<ShortGuid> hierarchyCopy = new List<ShortGuid>();
            for (int x = 0; x < hierarchy.Count; x++)
                hierarchyCopy.Add(new ShortGuid((byte[])hierarchy[x].val.Clone()));

            Composite currentFlowgraphToSearch = Editor.selected.composite;
            if (currentFlowgraphToSearch == null || currentFlowgraphToSearch.GetEntityByID(hierarchyCopy[0]) == null)
            {
                currentFlowgraphToSearch = Editor.commands.EntryPoints[0];
                if (currentFlowgraphToSearch == null || currentFlowgraphToSearch.GetEntityByID(hierarchyCopy[0]) == null)
                {
                    currentFlowgraphToSearch = Editor.commands.GetComposite(hierarchyCopy[0]);
                    if (currentFlowgraphToSearch == null || currentFlowgraphToSearch.GetEntityByID(hierarchyCopy[1]) == null)
                    {
                        containedFlowgraph = null;
                        asString = "";
                        return null;
                    }
                    hierarchyCopy.RemoveAt(0);
                }
            }

            Entity entity = null;
            string hierarchyString = "";
            for (int i = 0; i < hierarchyCopy.Count; i++)
            {
                entity = currentFlowgraphToSearch.GetEntityByID(hierarchyCopy[i]);

                if (entity == null) break;
                hierarchyString += "[" + entity.shortGUID + "] " + Editor.util.entity.GetName(currentFlowgraphToSearch.shortGUID, entity.shortGUID);
                if (i >= hierarchyCopy.Count - 2) break; //Last is always 00-00-00-00
                hierarchyString += " -> ";

                if (entity.variant == EntityVariant.FUNCTION)
                {
                    Composite flowRef = Editor.commands.GetComposite(((FunctionEntity)entity).function);
                    if (flowRef != null)
                    {
                        currentFlowgraphToSearch = flowRef;
                    }
                    else
                    {
                        entity = null;
                        break;
                    }
                }
            }
            containedFlowgraph = (entity == null) ? null : currentFlowgraphToSearch;
            asString = hierarchyString;
            return entity;
        }

        /* CA's CAGE doesn't properly tidy up hierarchies pointing to deleted entities - so we do that here to save confusion */
        private static List<ShortGuid> purgedComps = new List<ShortGuid>();
        public static void PurgeDeadHierarchiesInActiveComposite()
        {
            Composite comp = Editor.selected.composite;
            if (purgedComps.Contains(comp.shortGUID)) return;
            purgedComps.Add(comp.shortGUID);

            int originalUnknownCount = 0;
            int originalProxyCount = 0;
            int newProxyCount = 0;
            int originalOverrideCount = 0;
            int newOverrideCount = 0;
            int originalTriggerCount = 0;
            int newTriggerCount = 0;
            int originalAnimCount = 0;
            int newAnimCount = 0;
            int originalLinkCount = 0;
            int newLinkCount = 0;

            //Clear overrides
            List<OverrideEntity> overridePurged = new List<OverrideEntity>();
            for (int i = 0; i < comp.overrides.Count; i++)
                if (ResolveHierarchy(comp.overrides[i].hierarchy, out Composite flowTemp, out string hierarchy) != null)
                    overridePurged.Add(comp.overrides[i]);
            originalOverrideCount += comp.overrides.Count;
            newOverrideCount += overridePurged.Count;
            comp.overrides = overridePurged;

            //Clear proxies
            List<ProxyEntity> proxyPurged = new List<ProxyEntity>();
            for (int i = 0; i < comp.proxies.Count; i++)
                if (ResolveHierarchy(comp.proxies[i].hierarchy, out Composite flowTemp, out string hierarchy) != null)
                    proxyPurged.Add(comp.proxies[i]);
            originalProxyCount += comp.proxies.Count;
            newProxyCount += proxyPurged.Count;
            comp.proxies = proxyPurged;

            //Clear TriggerSequence and CAGEAnimation entities
            for (int i = 0; i < comp.functions.Count; i++)
            {
                //TODO: will this also clear up TriggerSequence/CAGEAnimation data for proxies?
                switch (ShortGuidUtils.FindString(comp.functions[i].function))
                {
                    case "TriggerSequence":
                        TriggerSequence trig = (TriggerSequence)comp.functions[i];
                        List<CathodeTriggerSequenceTrigger> trigSeq = new List<CathodeTriggerSequenceTrigger>();
                        for (int x = 0; x < trig.triggers.Count; x++)
                            if (ResolveHierarchy(trig.triggers[x].hierarchy, out Composite flowTemp, out string hierarchy) != null)
                                trigSeq.Add(trig.triggers[x]);
                        originalTriggerCount += trig.triggers.Count;
                        newTriggerCount += trigSeq.Count;
                        trig.triggers = trigSeq;
                        break;
                    case "CAGEAnimation":
                        CAGEAnimation anim = (CAGEAnimation)comp.functions[i];
                        List<CathodeParameterKeyframeHeader> headers = new List<CathodeParameterKeyframeHeader>();
                        for (int x = 0; x < anim.keyframeHeaders.Count; x++)
                            if (ResolveHierarchy(anim.keyframeHeaders[x].connectedEntity, out Composite flowTemp, out string hierarchy) != null)
                                headers.Add(anim.keyframeHeaders[x]);
                        originalAnimCount += anim.keyframeHeaders.Count;
                        newAnimCount += headers.Count;
                        anim.keyframeHeaders = headers;
                        break;
                }
            }

            //Clear links 
            List<Entity> entities = comp.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                List<EntityLink> childLinksPurged = new List<EntityLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                    if (comp.GetEntityByID(entities[i].childLinks[x].childID) != null)
                        childLinksPurged.Add(entities[i].childLinks[x]);
                originalLinkCount += entities[i].childLinks.Count;
                newLinkCount += childLinksPurged.Count;
                entities[i].childLinks = childLinksPurged;
            }

            if (originalUnknownCount + 
                (originalProxyCount - newProxyCount) + 
                (originalOverrideCount - newOverrideCount) + 
                (originalTriggerCount - newTriggerCount) + 
                (originalAnimCount - newAnimCount) + 
                (originalLinkCount - newLinkCount) == 0) 
                return;
            Console.WriteLine(
                "Purged all dead hierarchies and entities in " + comp.name + "!" +
                "\n - " + originalUnknownCount + " unknown entities" +
                "\n - " + (originalProxyCount - newProxyCount) + " proxies (of " + originalProxyCount + ")" +
                "\n - " + (originalOverrideCount - newOverrideCount) + " overrides (of " + originalOverrideCount + ")" +
                "\n - " + (originalTriggerCount - newTriggerCount) + " triggers (of " + originalTriggerCount + ")" +
                "\n - " + (originalAnimCount - newAnimCount) + " anims (of " + originalAnimCount + ")" +
                "\n - " + (originalLinkCount - newLinkCount) + " links (of " + originalLinkCount + ")");
        }
        public static void ResetHierarchyPurgeCache()
        {
            purgedComps.Clear();
        }
    }
}
