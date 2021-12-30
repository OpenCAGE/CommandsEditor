using CATHODE;
using CATHODE.Commands;
using CathodeLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//This is an extension to CathodeLib's NodeDB system which allows saving of node and parameter names into COMMANDS.PAK.
//Param names and node names are appended to the bottom of the COMMANDS.PAK file, and then read by this script.

//Still undecided if this should actually be a part of CathodeLib and not this GUI tool, however I feel it suits this project best for now.

namespace CathodeEditorGUI
{
    static class NodeDBEx
    {
        private static List<ShortGUIDDescriptor> customParamNames = null;
        private static List<ShortGUIDDescriptor> customNodeNames = null;

        //To be called directly after loading the pak using CathodeLib
        public static void LoadNames()
        {
            customParamNames = new List<ShortGUIDDescriptor>();
            customNodeNames = new List<ShortGUIDDescriptor>();

            BinaryReader reader = new BinaryReader(File.OpenRead(CurrentInstance.commandsPAK.Filepath));
            reader.BaseStream.Position = 20;
            int end_of_pak = reader.ReadInt32() * 4;
            end_of_pak += reader.ReadInt32() * 4;
            reader.BaseStream.Position = end_of_pak;

            int content_after_pak = (int)reader.BaseStream.Length - end_of_pak;
            if (content_after_pak == 0) return;

            int number_of_custom_param_names = reader.ReadInt32();
            for (int i = 0; i < number_of_custom_param_names; i++)
            {
                ShortGUIDDescriptor thisDesc = new ShortGUIDDescriptor();
                thisDesc.ID = Utilities.Consume<cGUID>(reader);
                thisDesc.Description = reader.ReadString();
                customParamNames.Add(thisDesc);
            }

            int number_of_custom_node_names = reader.ReadInt32();
            for (int i = 0; i < number_of_custom_node_names; i++)
            {
                ShortGUIDDescriptor thisDesc = new ShortGUIDDescriptor();
                thisDesc.ID = Utilities.Consume<cGUID>(reader);
                thisDesc.Description = reader.ReadString();
                customNodeNames.Add(thisDesc);
            }

            reader.Close();
        }

        //To be called directly after saving the pak using CathodeLib
        public static void SaveNames()
        {
            BinaryWriter writer = new BinaryWriter(File.OpenWrite(CurrentInstance.commandsPAK.Filepath));
            writer.BaseStream.Position = writer.BaseStream.Length;

            writer.Write(customParamNames.Count);
            for (int i = 0; i < customParamNames.Count; i++)
            {
                Utilities.Write<cGUID>(writer, customParamNames[i].ID);
                writer.Write(customParamNames[i].Description);
            }

            writer.Write(customNodeNames.Count);
            for (int i = 0; i < customNodeNames.Count; i++)
            {
                Utilities.Write<cGUID>(writer, customNodeNames[i].ID);
                writer.Write(customNodeNames[i].Description);
            }

            writer.Close();
        }

        //Add new param/node names
        public static void AddNewParameterName(cGUID id, string name)
        {
            customParamNames.Add(new ShortGUIDDescriptor{ ID = id, Description = name });
        }
        public static void RemoveNewParameterName(cGUID id)
        {
            ShortGUIDDescriptor desc = customParamNames.FirstOrDefault(o => o.ID == id);
            if (desc == null) return;
            customParamNames.Remove(desc);
        }
        //--
        public static void AddNewNodeName(cGUID id, string name)
        {
            customNodeNames.Add(new ShortGUIDDescriptor { ID = id, Description = name });
        }
        public static void RemoveNewNodeName(cGUID id)
        {
            ShortGUIDDescriptor desc = customNodeNames.FirstOrDefault(o => o.ID == id);
            if (desc == null) return;
            customNodeNames.Remove(desc);
        }

        //Get parameter/entity name
        //We fall through to NodeDB here which means we can replace all NodeDB calls to Cathode/Editor name in the GUI app
        public static string GetParameterName(cGUID id)
        {
            ShortGUIDDescriptor desc = customParamNames.FirstOrDefault(o => o.ID == id);
            if (desc == null) return NodeDB.GetCathodeName(id, CurrentInstance.commandsPAK);
            return desc.Description;
        }
        public static string GetEntityName(cGUID id)
        {
            ShortGUIDDescriptor desc = customNodeNames.FirstOrDefault(o => o.ID == id);
            if (desc == null) return NodeDB.GetEditorName(id);
            return desc.Description;
        }

        public static string[] GetEntityParameterList(string entity_name)
        {
            //We should never need to correct NodeDB here, as you can't add custom parameters to hardcoded nodes (for obvious reasons)
            //But... doing it anyway 
            string[] content = NodeDB.GetEntityParameterList(entity_name);
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i].Length == 11)
                {
                    if (content[i][2] == '-' && content[i][5] == '-' && content[i][8] == '-')
                    {
                        try
                        {
                            cGUID id = new cGUID(content[i]);
                            content[i] = GetParameterName(id);
                        }
                        catch { }
                    }
                }
            }
            return content;
        }
    }
}
