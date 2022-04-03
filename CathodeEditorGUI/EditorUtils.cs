using CATHODE;
using CATHODE.Commands;
using CathodeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CathodeEditorGUI
{
    static class EditorUtils
    {
        /* Utility: generate nice entity name to display in UI */
        public static string GenerateNodeName(CathodeEntity entity, CathodeComposite currentFlowgraph)
        {
            if (CurrentInstance.commandsPAK == null) 
                return "";
            if (hasFinishedCachingEntityNames && cachedEntityName.ContainsKey(entity.shortGUID)) 
                return cachedEntityName[entity.shortGUID];
            return GenerateNodeNameInternal(entity, currentFlowgraph);
        }
        private static string GenerateNodeNameInternal(CathodeEntity entity, CathodeComposite currentFlowgraph)
        {
            string desc = "";
            switch (entity.variant)
            {
                case EntityVariant.DATATYPE:
                    desc = EntityDBEx.GetParameterName(((DatatypeEntity)entity).parameter) + " (DataType " + ((DatatypeEntity)entity).type.ToString() + ")";
                    break;
                case EntityVariant.FUNCTION:
                    desc = EntityDBEx.GetEntityName(entity.shortGUID) + " (" + EntityDBEx.GetParameterName(((FunctionEntity)entity).function) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((OverrideEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = EntityDBEx.GetEntityName(entity.shortGUID) + " (*OVERRIDE*)";
                    break;
                case EntityVariant.PROXY:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((ProxyEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = EntityDBEx.GetEntityName(entity.shortGUID) + " (*PROXY*)";
                    break;
                case EntityVariant.NOT_SETUP:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID);
                    desc = EntityDBEx.GetEntityName(entity.shortGUID) + " (*NOT SETUP*)";
                    break;
            }
            return "[" + entity.shortGUID.ToString() + "] " + desc;
        }

        /* Generate a cache of entity names to save re-generating them every time */
        private static bool hasFinishedCachingEntityNames = false;
        private static Dictionary<cGUID, string> cachedEntityName = new Dictionary<cGUID, string>();
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
                        //TODO: Figure out why this is happening... aren't node IDs meant to be unique to the whole PAK? Maybe it's per flowgraph?
                        string bleh = "";
                    }
                    else
                    {
                        cachedEntityName.Add(ents[x].shortGUID, GenerateNodeNameInternal(ents[x], CurrentInstance.commandsPAK.Composites[i]));
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
        private static List<cGUID> queuedForRemoval = new List<cGUID>();
        public static void PurgeEntityNameFromCache(cGUID entId)
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
                    cGUID function = ((FunctionEntity)entity).function;
                    List<CathodeEntityDatabase.ParameterDefinition> parameters = CathodeEntityDatabase.GetParametersFromEntity(function);
                    if (parameters != null)
                    {
                        didGenerateFromDB = true;
                        for (int i = 0; i < parameters.Count; i++) items.Add(parameters[i].name);
                    }
                    else
                    {
                        string[] options = EntityDB.GetEntityParameterList(EntityDBEx.GetParameterName(function));
                        items.Add("trigger"); items.Add("reference"); //TODO: populate all params from EntityMethodInterface?
                        if (options == null)
                        {
                            CathodeComposite flow = CurrentInstance.commandsPAK.GetComposite(function);
                            if (flow == null) break;
                            for (int i = 0; i < flow.datatypes.Count; i++)
                            {
                                string to_add = EntityDBEx.GetParameterName(flow.datatypes[i].parameter);
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
                    items.Add(EntityDBEx.GetParameterName(((DatatypeEntity)entity).parameter));
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

        /* Resolve a node hierarchy: **firstHierarchyIsFlowgraph should be TRUE for proxies!** */
        public static CathodeEntity ResolveHierarchy(List<cGUID> hierarchy, out CathodeComposite containedFlowgraph)
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
        public static string HierarchyToString(List<cGUID> hierarchy)
        {
            string combinedString = "";
            for (int i = 0; i < hierarchy.Count; i++)
            {
                combinedString += "[" + hierarchy[i].ToString() + "] " + EntityDBEx.GetEntityName(hierarchy[i]);
                if (i == hierarchy.Count - 2) break; //Last is always 00-00-00-00
                combinedString += " -> ";
            }
            return combinedString;
        }

        /* CA's CAGE doesn't properly tidy up hierarchies pointing to deleted nodes - so we do that here to save confusion */
        private static List<cGUID> purgedFlows = new List<cGUID>();
        public static void PurgeDeadHierarchiesInActiveComposite()
        {
            return;

            CathodeComposite flow = CurrentInstance.selectedComposite;
            if (purgedFlows.Contains(flow.shortGUID)) return;
            purgedFlows.Add(flow.shortGUID);

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
            originalUnknownCount = flow.unknowns.Count;
            flow.unknowns.Clear();

            //Clear overrides
            List<OverrideEntity> overridePurged = new List<OverrideEntity>();
            for (int i = 0; i < flow.overrides.Count; i++)
                if (ResolveHierarchy(flow.overrides[i].hierarchy, out CathodeComposite flowTemp) != null)
                    overridePurged.Add(flow.overrides[i]);
            originalOverrideCount += flow.overrides.Count;
            newOverrideCount += overridePurged.Count;
            flow.overrides = overridePurged;

            //Clear proxies
            List<ProxyEntity> proxyPurged = new List<ProxyEntity>();
            for (int i = 0; i < flow.proxies.Count; i++)
                if (ResolveHierarchy(flow.proxies[i].hierarchy, out CathodeComposite flowTemp) != null)
                    proxyPurged.Add(flow.proxies[i]);
            originalProxyCount += flow.proxies.Count;
            newProxyCount += proxyPurged.Count;
            flow.proxies = proxyPurged;

            //Clear TriggerSequence and CAGEAnimation entities
            for (int i = 0; i < flow.functions.Count; i++)
            {
                switch (EntityDB.GetCathodeName(flow.functions[i].function))
                {
                    case "TriggerSequence":
                        TriggerSequence trig = (TriggerSequence)flow.functions[i];
                        List<TEMP_TriggerSequenceExtraDataHolder1> trigSeq = new List<TEMP_TriggerSequenceExtraDataHolder1>();
                        for (int x = 0; x < trig.triggers.Count; x++)
                            if (ResolveHierarchy(trig.triggers[x].hierarchy, out CathodeComposite flowTemp) != null)
                                trigSeq.Add(trig.triggers[x]);
                        originalTriggerCount += trig.triggers.Count;
                        newTriggerCount += trigSeq.Count;
                        trig.triggers = trigSeq;
                        break;
                    case "CAGEAnimation":
                        CAGEAnimation anim = (CAGEAnimation)flow.functions[i];
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
            List<CathodeEntity> entities = flow.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                List<CathodeEntityLink> childLinksPurged = new List<CathodeEntityLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                    if (flow.GetEntityByID(entities[i].childLinks[x].childID) != null)
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
                "Purged all dead hierarchies and nodes in " + flow.name + "!" +
                "\n - " + originalUnknownCount + " unknown nodes" +
                "\n - " + (originalProxyCount - newProxyCount) + " proxies (of " + originalProxyCount + ")" +
                "\n - " + (originalOverrideCount - newOverrideCount) + " overrides (of " + originalOverrideCount + ")" +
                "\n - " + (originalTriggerCount - newTriggerCount) + " triggers (of " + originalTriggerCount + ")" +
                "\n - " + (originalAnimCount - newAnimCount) + " anims (of " + originalAnimCount + ")" +
                "\n - " + (originalLinkCount - newLinkCount) + " links (of " + originalLinkCount + ")");
        }
        public static void ResetHierarchyPurgeCache()
        {
            purgedFlows.Clear();
        }
    }
}
