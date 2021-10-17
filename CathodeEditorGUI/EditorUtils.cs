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
        public static string GenerateNodeName(CathodeEntity entity)
        {
            if (_pak == null) return "";
            string desc = "";
            switch (entity.variant)
            {
                case EntityVariant.DATATYPE:
                    desc = NodeDB.GetCathodeName(((DatatypeEntity)entity).parameter) + " (DataType " + ((DatatypeEntity)entity).type.ToString() + ")";
                    break;
                case EntityVariant.FUNCTION:
                    desc = NodeDB.GetEditorName(entity.nodeID) + " (" + NodeDB.GetCathodeName(((FunctionEntity)entity).function, _pak) + ")";
                    break;
                case EntityVariant.OVERRIDE:
                    desc = "OVERRIDE!"; //TODO
                    break;
                case EntityVariant.PROXY:
                    desc = "PROXY!"; //TODO
                    break;
                case EntityVariant.NOT_SETUP:
                    desc = "NOT SETUP!"; //Huh?
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
                    string[] options = NodeDB.GetEntityParameterList(NodeDB.GetCathodeName(function));
                    if (options == null)
                    {
                        CathodeFlowgraph flow = _pak.GetFlowgraph(function);
                        if (flow == null) break;
                        for (int i = 0; i < flow.datatypes.Count; i++) items.Add(NodeDB.GetCathodeName(flow.datatypes[i].parameter));
                    }
                    else
                    {
                        for (int i = 0; i < options.Length; i++) items.Add(options[i]);
                    }
                    break;
                case EntityVariant.DATATYPE:
                    items.Add(NodeDB.GetCathodeName(((DatatypeEntity)entity).parameter));
                    break;
                    //TODO: support other types here
            }
            return items;
        }

        /* Utility: force a string to be numeric */
        public static string ForceStringNumeric(string str, bool allowDots = false)
        {
            string editedText = "";
            bool hasIncludedDot = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsNumber(str[i]) || (str[i] == '.' && allowDots))
                {
                    if (str[i] == '.' && hasIncludedDot) continue;
                    if (str[i] == '.') hasIncludedDot = true;
                    editedText += str[i];
                }
            }
            if (editedText == "") editedText = "0";
            return editedText;
        }
    }
}
