using CATHODE;
using CATHODE.Commands;
using CathodeLib;
using System;
using System.Collections.Generic;

namespace CathodeEditorGUI
{
    static class EditorUtils
    {
        /* Utility: generate nice entity name to display in UI */
        public static string GenerateEntityName(CathodeEntity entity, CathodeComposite currentFlowgraph, bool regenCache = false)
        {
            if (CurrentInstance.commandsPAK == null) 
                return entity.shortGUID.ToString();

            if (hasFinishedCachingEntityNames && regenCache)
            {
                if (cachedEntityName.ContainsKey(entity.shortGUID)) cachedEntityName.Remove(entity.shortGUID);
                cachedEntityName.Add(entity.shortGUID, GenerateEntityNameInternal(entity, currentFlowgraph));
            }

            if (hasFinishedCachingEntityNames && cachedEntityName.ContainsKey(entity.shortGUID)) 
                return cachedEntityName[entity.shortGUID];

            return GenerateEntityNameInternal(entity, currentFlowgraph);
        }
        private static string GenerateEntityNameInternal(CathodeEntity entity, CathodeComposite composite)
        {
            string desc = "";
            switch (entity.variant)
            {
                case EntityVariant.DATATYPE:
                    desc = ShortGuidUtils.FindString(((DatatypeEntity)entity).parameter) + " (DataType " + ((DatatypeEntity)entity).type.ToString() + ")";
                    break;
                case EntityVariant.FUNCTION:
                    CathodeComposite funcComposite = CurrentInstance.commandsPAK.GetComposite(((FunctionEntity)entity).function);
                    if (funcComposite != null)
                        desc = CurrentInstance.compositeLookup.GetEntityName(composite.shortGUID, entity.shortGUID) + " (" + funcComposite.name + ")";
                    else
                        desc = CurrentInstance.compositeLookup.GetEntityName(composite.shortGUID, entity.shortGUID) + " (" + ShortGuidUtils.FindString(((FunctionEntity)entity).function) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((OverrideEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = CurrentInstance.compositeLookup.GetEntityName(composite.shortGUID, entity.shortGUID) + " (*OVERRIDE*)";
                    break;
                case EntityVariant.PROXY:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((ProxyEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = CurrentInstance.compositeLookup.GetEntityName(composite.shortGUID, entity.shortGUID) + " (*PROXY*)";
                    break;
                case EntityVariant.NOT_SETUP:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID);
                    desc = CurrentInstance.compositeLookup.GetEntityName(composite.shortGUID, entity.shortGUID) + " (*NOT SETUP*)";
                    break;
            }
            return "[" + entity.shortGUID.ToString() + "] " + desc;
        }

        /* Generate a cache of entity names to save re-generating them every time */
        private static bool hasFinishedCachingEntityNames = false;
        private static Dictionary<ShortGuid, string> cachedEntityName = new Dictionary<ShortGuid, string>();
        public static void GenerateEntityNameCache(CathodeEditorGUI mainInst)
        {
            if (CurrentInstance.commandsPAK == null) return;
            hasFinishedCachingEntityNames = false;
            mainInst.EnableLoadingOfPaks(false);
            cachedEntityName.Clear();
            for (int i = 0; i < CurrentInstance.commandsPAK.Composites.Count; i++)
            {
                List<CathodeEntity> ents = CurrentInstance.commandsPAK.Composites[i].GetEntities();
                for (int x = 0; x < ents.Count; x++)
                {
                    if (cachedEntityName.ContainsKey(ents[x].shortGUID))
                    {
                        //TODO: Figure out why this is happening... aren't node IDs meant to be unique to the whole PAK? Maybe it's per composite?
                        string bleh = "";
                    }
                    else
                    {
                        cachedEntityName.Add(ents[x].shortGUID, GenerateEntityNameInternal(ents[x], CurrentInstance.commandsPAK.Composites[i]));
                    }
                }
            }
            if (queuedForRemoval.Count != 0)
            {
                for (int i = 0; i < queuedForRemoval.Count; i++)
                    cachedEntityName.Remove(queuedForRemoval[i]);
                queuedForRemoval.Clear();
            }
            mainInst.EnableLoadingOfPaks(true);
            hasFinishedCachingEntityNames = true;
        }
        private static List<ShortGuid> queuedForRemoval = new List<ShortGuid>();
        public static void PurgeEntityNameFromCache(ShortGuid entId)
        {
            if (!hasFinishedCachingEntityNames) queuedForRemoval.Add(entId);
            else cachedEntityName.Remove(entId);
        }

        /* Utility: generate a list of suggested parameters for an entity */
        public static List<string> GenerateParameterList(CathodeEntity entity, out bool didGenerateFromDB)
        {
            didGenerateFromDB = false;
            List<string> items = new List<string>();
            if (CurrentInstance.commandsPAK == null) return items;
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    ShortGuid function = ((FunctionEntity)entity).function;
                    List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(function);
                    if (parameters != null)
                    {
                        didGenerateFromDB = true;
                        for (int i = 0; i < parameters.Count; i++) items.Add(parameters[i].name);
                    }
                    else
                    {
                        string[] options = EntityDB.GetEntityParameterList(ShortGuidUtils.FindString(function));
                        items.Add("trigger"); items.Add("reference"); //TODO: populate all params from EntityMethodInterface?
                        if (options == null)
                        {
                            CathodeComposite flow = CurrentInstance.commandsPAK.GetComposite(function);
                            if (flow == null) break;
                            for (int i = 0; i < flow.datatypes.Count; i++)
                            {
                                string to_add = ShortGuidUtils.FindString(flow.datatypes[i].parameter);
                                //TODO: also return datatype here
                                if (!items.Contains(to_add)) items.Add(to_add);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < options.Length; i++)
                            {
                                if (!items.Contains(options[i])) items.Add(options[i]);
                            }
                        }
                    }
                    break;
                case EntityVariant.DATATYPE:
                    items.Add(ShortGuidUtils.FindString(((DatatypeEntity)entity).parameter));
                    break;
                    //TODO: support other types here
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
            return editedText;
        }

        /* Resolve a node hierarchy */
        public static CathodeEntity ResolveHierarchy(List<ShortGuid> hierarchy, out CathodeComposite containedFlowgraph)
        {
            if (hierarchy.Count == 0)
            {
                containedFlowgraph = null;
                return null;
            }

            CathodeComposite currentFlowgraphToSearch = CurrentInstance.selectedComposite;
            if (currentFlowgraphToSearch == null || currentFlowgraphToSearch.GetEntityByID(hierarchy[0]) == null)
            {
                currentFlowgraphToSearch = CurrentInstance.commandsPAK.EntryPoints[0];
                if (currentFlowgraphToSearch == null || currentFlowgraphToSearch.GetEntityByID(hierarchy[0]) == null)
                {
                    currentFlowgraphToSearch = CurrentInstance.commandsPAK.GetComposite(hierarchy[0]);
                    if (currentFlowgraphToSearch == null || currentFlowgraphToSearch.GetEntityByID(hierarchy[1]) == null)
                    {
                        containedFlowgraph = null;
                        return null;
                    }
                    hierarchy.RemoveAt(0);
                }
            }

            CathodeEntity entity = null;
            for (int i = 0; i < hierarchy.Count; i++)
            {
                entity = currentFlowgraphToSearch.GetEntityByID(hierarchy[i]);

                if (entity == null) break;
                if (i >= hierarchy.Count - 2) break; //Last is always 00-00-00-00

                if (entity.variant == EntityVariant.FUNCTION)
                {
                    CathodeComposite flowRef = CurrentInstance.commandsPAK.GetComposite(((FunctionEntity)entity).function);
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
            return entity;
        }

        /* Display an entity hierarchy as a string */
        public static string HierarchyToString(List<ShortGuid> hierarchy)
        {
            string combinedString = "";
            for (int i = 0; i < hierarchy.Count; i++)
            {
                //TODO: how can i get the composite containing the node if we are chasing a hierarchy?
                //combinedString += "[" + hierarchy[i].ToString() + "] " + EntityDBEx.GetEntityName(hierarchy[i]);
                combinedString += hierarchy[i].ToString();
                if (i == hierarchy.Count - 2) break; //Last is always 00-00-00-00
                combinedString += " -> ";
            }
            return combinedString;
        }

        /* CA's CAGE doesn't properly tidy up hierarchies pointing to deleted entities - so we do that here to save confusion */
        private static List<ShortGuid> purgedComps = new List<ShortGuid>();
        public static void PurgeDeadHierarchiesInActiveComposite()
        {
            CathodeComposite comp = CurrentInstance.selectedComposite;
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

            //Clear unknowns
            originalUnknownCount = comp.unknowns.Count;
            comp.unknowns.Clear();

            //Clear overrides
            List<OverrideEntity> overridePurged = new List<OverrideEntity>();
            for (int i = 0; i < comp.overrides.Count; i++)
                if (ResolveHierarchy(comp.overrides[i].hierarchy, out CathodeComposite flowTemp) != null)
                    overridePurged.Add(comp.overrides[i]);
            originalOverrideCount += comp.overrides.Count;
            newOverrideCount += overridePurged.Count;
            comp.overrides = overridePurged;

            //Clear proxies
            List<ProxyEntity> proxyPurged = new List<ProxyEntity>();
            for (int i = 0; i < comp.proxies.Count; i++)
                if (ResolveHierarchy(comp.proxies[i].hierarchy, out CathodeComposite flowTemp) != null)
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
                        List<TEMP_TriggerSequenceExtraDataHolder1> trigSeq = new List<TEMP_TriggerSequenceExtraDataHolder1>();
                        for (int x = 0; x < trig.triggers.Count; x++)
                            if (ResolveHierarchy(trig.triggers[x].hierarchy, out CathodeComposite flowTemp) != null)
                                trigSeq.Add(trig.triggers[x]);
                        originalTriggerCount += trig.triggers.Count;
                        newTriggerCount += trigSeq.Count;
                        trig.triggers = trigSeq;
                        break;
                    case "CAGEAnimation":
                        CAGEAnimation anim = (CAGEAnimation)comp.functions[i];
                        List<CathodeParameterKeyframeHeader> headers = new List<CathodeParameterKeyframeHeader>();
                        for (int x = 0; x < anim.keyframeHeaders.Count; x++)
                            if (ResolveHierarchy(anim.keyframeHeaders[x].connectedEntity, out CathodeComposite flowTemp) != null)
                                headers.Add(anim.keyframeHeaders[x]);
                        originalAnimCount += anim.keyframeHeaders.Count;
                        newAnimCount += headers.Count;
                        anim.keyframeHeaders = headers;
                        break;
                }
            }

            //Clear links 
            List<CathodeEntity> entities = comp.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                List<CathodeEntityLink> childLinksPurged = new List<CathodeEntityLink>();
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
