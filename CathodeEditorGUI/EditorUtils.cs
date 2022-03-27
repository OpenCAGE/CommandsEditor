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
        public static string GenerateNodeName(CathodeEntity entity, CathodeFlowgraph currentFlowgraph)
        {
            if (CurrentInstance.commandsPAK == null) 
                return "";
            if (hasFinishedCachingEntityNames && cachedEntityName.ContainsKey(entity.nodeID)) 
                return cachedEntityName[entity.nodeID];
            return GenerateNodeNameInternal(entity, currentFlowgraph);
        }
        private static string GenerateNodeNameInternal(CathodeEntity entity, CathodeFlowgraph currentFlowgraph)
        {
            string desc = "";
            switch (entity.variant)
            {
                case EntityVariant.DATATYPE:
                    desc = NodeDBEx.GetParameterName(((DatatypeEntity)entity).parameter) + " (DataType " + ((DatatypeEntity)entity).type.ToString() + ")";
                    break;
                case EntityVariant.FUNCTION:
                    desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + NodeDBEx.GetParameterName(((FunctionEntity)entity).function) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((OverrideEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = NodeDBEx.GetEntityName(entity.nodeID) + " (*OVERRIDE*)";
                    break;
                case EntityVariant.PROXY:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID) + " (" + HierarchyToString(((ProxyEntity)entity).hierarchy, currentFlowgraph) + ")";
                    desc = NodeDBEx.GetEntityName(entity.nodeID) + " (*PROXY*)";
                    break;
                case EntityVariant.NOT_SETUP:
                    //desc = NodeDBEx.GetEntityName(entity.nodeID);
                    desc = NodeDBEx.GetEntityName(entity.nodeID) + " (*NOT SETUP*)";
                    break;
            }
            return "[" + entity.nodeID.ToString() + "] " + desc;
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
            for (int i = 0; i < CurrentInstance.commandsPAK.Flowgraphs.Count; i++)
            {
                List<CathodeEntity> ents = CurrentInstance.commandsPAK.Flowgraphs[i].GetEntities();
                for (int x = 0; x < ents.Count; x++)
                {
                    if (cachedEntityName.ContainsKey(ents[x].nodeID))
                    {
                        //TODO: Figure out why this is happening... aren't node IDs meant to be unique to the whole PAK? Maybe it's per flowgraph?
                        string bleh = "";
                    }
                    else
                    {
                        cachedEntityName.Add(ents[x].nodeID, GenerateNodeNameInternal(ents[x], CurrentInstance.commandsPAK.Flowgraphs[i]));
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
                        string[] options = NodeDB.GetEntityParameterList(NodeDBEx.GetParameterName(function));
                        items.Add("trigger"); items.Add("reference"); //TODO: populate all params from EntityMethodInterface?
                        if (options == null)
                        {
                            CathodeFlowgraph flow = CurrentInstance.commandsPAK.GetFlowgraph(function);
                            if (flow == null) break;
                            for (int i = 0; i < flow.datatypes.Count; i++)
                            {
                                string to_add = NodeDBEx.GetParameterName(flow.datatypes[i].parameter);
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
                    items.Add(NodeDBEx.GetParameterName(((DatatypeEntity)entity).parameter));
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
        public static CathodeEntity ResolveHierarchy(List<cGUID> hierarchy, bool firstHierarchyIsFlowgraph, out CathodeFlowgraph containedFlowgraph)
        {
            CathodeFlowgraph currentFlowgraphToSearch = (firstHierarchyIsFlowgraph) ? CurrentInstance.commandsPAK.GetFlowgraph(hierarchy[0]) : CurrentInstance.selectedFlowgraph;
            if (firstHierarchyIsFlowgraph) hierarchy.RemoveAt(0);
            CathodeEntity entity = null;
            for (int i = 0; i < hierarchy.Count; i++)
            {
                if (hierarchy[i] == new cGUID("00-00-00-00")) break;
                entity = currentFlowgraphToSearch.GetEntityByID(hierarchy[i]);
                if (entity != null && entity.variant == EntityVariant.FUNCTION)
                {
                    CathodeFlowgraph flowRef = CurrentInstance.commandsPAK.GetFlowgraph(((FunctionEntity)entity).function);
                    if (flowRef != null) currentFlowgraphToSearch = flowRef;
                }
            }
            containedFlowgraph = currentFlowgraphToSearch;
            return entity;
        }

        /* Display an entity hierarchy as a string */
        public static string HierarchyToString(List<cGUID> hierarchy)
        {
            CathodeFlowgraph currentFlowgraphToSearch = CurrentInstance.selectedFlowgraph;
            CathodeEntity entity = null;
            string combinedString = "";
            for (int i = 0; i < hierarchy.Count; i++)
            {
                if (hierarchy[i] == new cGUID("00-00-00-00")) break;
                entity = currentFlowgraphToSearch.GetEntityByID(hierarchy[i]);
                if (entity != null) combinedString += NodeDBEx.GetEntityName(entity.nodeID) + " -> ";
                if (entity != null && entity.variant == EntityVariant.FUNCTION)
                {
                    CathodeFlowgraph flowRef = CurrentInstance.commandsPAK.GetFlowgraph(((FunctionEntity)entity).function);
                    if (flowRef != null) currentFlowgraphToSearch = flowRef;
                }
            }
            if (combinedString.Length >= 4) combinedString = combinedString.Substring(0, combinedString.Length - 4);
            return combinedString;
        }

        /* CA's CAGE doesn't properly tidy up hierarchies pointing to deleted nodes - so we do that here to save confusion */
        public static void PurgeDeadHierarchiesInActiveFlowgraph()
        {
            int originalProxyCount = 0;
            int newProxyCount = 0;
            int originalOverrideCount = 0;
            int newOverrideCount = 0;
            int originalTriggerCount = 0;
            int newTriggerCount = 0;
            int originalAnimCount = 0;
            int newAnimCount = 0;
            CathodeFlowgraph flow = CurrentInstance.selectedFlowgraph;

            //Clear overrides
            List<OverrideEntity> overridePurged = new List<OverrideEntity>();
            for (int i = 0; i < flow.overrides.Count; i++)
                if (ResolveHierarchy(flow.overrides[i].hierarchy, false, out CathodeFlowgraph flowTemp) != null)
                    overridePurged.Add(flow.overrides[i]);
            originalOverrideCount += flow.overrides.Count;
            newOverrideCount += overridePurged.Count;
            flow.overrides = overridePurged;

            //Clear proxies
            List<ProxyEntity> proxyPurged = new List<ProxyEntity>();
            for (int i = 0; i < flow.proxies.Count; i++)
                if (ResolveHierarchy(flow.proxies[i].hierarchy, true, out CathodeFlowgraph flowTemp) != null)
                    proxyPurged.Add(flow.proxies[i]);
            originalProxyCount += flow.proxies.Count;
            newProxyCount += proxyPurged.Count;
            flow.proxies = proxyPurged;

            //Clear TriggerSequence and CAGEAnimation entities
            for (int i = 0; i < flow.functions.Count; i++)
            {
                switch (NodeDB.GetCathodeName(flow.functions[i].function))
                {
                    case "TriggerSequence":
                        TriggerSequence trig = (TriggerSequence)flow.functions[i];
                        List<TEMP_TriggerSequenceExtraDataHolder1> trigSeq = new List<TEMP_TriggerSequenceExtraDataHolder1>();
                        for (int x = 0; x < trig.triggers.Count; x++)
                            if (ResolveHierarchy(trig.triggers[x].hierarchy, false, out CathodeFlowgraph flowTemp) != null)
                                trigSeq.Add(trig.triggers[x]);
                        originalTriggerCount += trig.triggers.Count;
                        newTriggerCount += trigSeq.Count;
                        trig.triggers = trigSeq;
                        break;
                    case "CAGEAnimation":
                        CAGEAnimation anim = (CAGEAnimation)flow.functions[i];
                        List<CathodeParameterKeyframeHeader> headers = new List<CathodeParameterKeyframeHeader>();
                        for (int x = 0; x < anim.keyframeHeaders.Count; x++)
                            if (ResolveHierarchy(anim.keyframeHeaders[x].connectedEntity, false, out CathodeFlowgraph flowTemp) != null)
                                headers.Add(anim.keyframeHeaders[x]);
                        originalAnimCount += anim.keyframeHeaders.Count;
                        newAnimCount += headers.Count;
                        anim.keyframeHeaders = headers;
                        break;
                }
                if (NodeDB.GetCathodeName(flow.functions[i].function) != "TriggerSequence") continue;
            }

            if ((originalProxyCount - newProxyCount) + (originalOverrideCount - newOverrideCount) + (originalTriggerCount - newTriggerCount) + (originalAnimCount - newAnimCount) == 0) return;
            Console.WriteLine(
                "Purged all dead hierarchies in " + flow.name + "!" +
                "\n - " + (originalProxyCount - newProxyCount) + " proxies (of " + originalProxyCount + ")" +
                "\n - " + (originalOverrideCount - newOverrideCount) + " overrides (of " + originalOverrideCount + ")" +
                "\n - " + (originalTriggerCount - newTriggerCount) + " triggers (of " + originalTriggerCount + ")" +
                "\n - " + (originalAnimCount - newAnimCount) + " anims (of " + originalAnimCount + ")");
        }
    }
}
