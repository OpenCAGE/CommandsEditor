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
        private static CommandsPAK _pak = null;
        public static CommandsPAK Commands { get { return _pak; } }

        public static void Setup(CommandsPAK commandsPAK)
        {
            _pak = commandsPAK;
        }

        /* Utility: generate nice entity name to display in UI */
        public static string GenerateNodeName(CathodeEntity entity, CathodeFlowgraph currentFlowgraph)
        {
            if (_pak == null) return "";
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

        /* Utility: generate a list of suggested parameters for an entity */
        public static List<string> GenerateParameterList(CathodeEntity entity)
        {
            List<string> items = new List<string>();
            if (_pak == null) return items;
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    cGUID function = ((FunctionEntity)entity).function;
                    string[] options = NodeDB.GetEntityParameterList(NodeDBEx.GetParameterName(function));
                    items.Add("trigger"); items.Add("reference"); //TODO: populate all params from EntityMethodInterface?
                    if (options == null)
                    {
                        CathodeFlowgraph flow = _pak.GetFlowgraph(function);
                        if (flow == null) break;
                        for (int i = 0; i < flow.datatypes.Count; i++)
                        {
                            string to_add = NodeDBEx.GetParameterName(flow.datatypes[i].parameter);
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
            return editedText;
        }

        /* Resolve a node hierarchy */
        public static CathodeEntity ResolveHierarchy(List<cGUID> hierarchy, CathodeFlowgraph flowgraph, out CathodeFlowgraph containedFlowgraph)
        {
            CathodeFlowgraph currentFlowgraphToSearch = flowgraph;
            CathodeEntity entity = null;
            for (int i = 0; i < hierarchy.Count; i++)
            {
                if (hierarchy[i] == new cGUID("00-00-00-00")) break;
                entity = currentFlowgraphToSearch.GetEntityByID(hierarchy[i]);
                if (entity != null && entity.variant == EntityVariant.FUNCTION)
                {
                    CathodeFlowgraph flowRef = _pak.GetFlowgraph(((FunctionEntity)entity).function);
                    if (flowRef != null) currentFlowgraphToSearch = flowRef;
                }
            }
            containedFlowgraph = currentFlowgraphToSearch;
            return entity;
        }

        /* Display an entity hierarchy as a string */
        public static string HierarchyToString(List<cGUID> hierarchy, CathodeFlowgraph flowgraph)
        {
            CathodeFlowgraph currentFlowgraphToSearch = flowgraph;
            CathodeEntity entity = null;
            string combinedString = "";
            for (int i = 0; i < hierarchy.Count; i++)
            {
                if (hierarchy[i] == new cGUID("00-00-00-00")) break;
                entity = currentFlowgraphToSearch.GetEntityByID(hierarchy[i]);
                if (entity != null) combinedString += NodeDBEx.GetEntityName(entity.nodeID) + " -> ";
                if (entity != null && entity.variant == EntityVariant.FUNCTION)
                {
                    CathodeFlowgraph flowRef = _pak.GetFlowgraph(((FunctionEntity)entity).function);
                    if (flowRef != null) currentFlowgraphToSearch = flowRef;
                }
            }
            if (combinedString.Length >= 4) combinedString = combinedString.Substring(0, combinedString.Length - 4);
            return combinedString;
        }
    }
}
