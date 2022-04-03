using CATHODE;
using CATHODE.Commands;
using CathodeLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//This is an extension to CathodeLib's EntityDB system which allows saving of entity and parameter names into COMMANDS.PAK.
//Param names and entity names are appended to the bottom of the COMMANDS.PAK file, and then read by this script.

//Still undecided if this should actually be a part of CathodeLib and not this GUI tool, however I feel it suits this project best for now.

namespace CathodeEditorGUI
{
    static class EntityDBEx
    {
        private static List<ShortGUIDDescriptor> customParamNames = null;
        private static List<ShortGUIDDescriptor> customEntityNames = null;

        //To be called directly after loading the pak using CathodeLib
        public static void LoadNames()
        {
            customParamNames = new List<ShortGUIDDescriptor>();
            customEntityNames = new List<ShortGUIDDescriptor>();

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
            for (int i = 0; i < customParamNames.Count; i++) customParamNames[i].ID_cachedstring = customParamNames[i].ID.ToString();

            int number_of_custom_entity_names = reader.ReadInt32();
            for (int i = 0; i < number_of_custom_entity_names; i++)
            {
                ShortGUIDDescriptor thisDesc = new ShortGUIDDescriptor();
                thisDesc.ID = Utilities.Consume<cGUID>(reader);
                thisDesc.Description = reader.ReadString();
                customEntityNames.Add(thisDesc);
            }
            for (int i = 0; i < customEntityNames.Count; i++) customEntityNames[i].ID_cachedstring = customEntityNames[i].ID.ToString();

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

            writer.Write(customEntityNames.Count);
            for (int i = 0; i < customEntityNames.Count; i++)
            {
                Utilities.Write<cGUID>(writer, customEntityNames[i].ID);
                writer.Write(customEntityNames[i].Description);
            }

            writer.Close();
        }

        //Add new param/entity names
        public static void AddNewParameterName(cGUID id, string name)
        {
            ShortGUIDDescriptor desc = customParamNames.FirstOrDefault(o => o.ID == id);
            if (desc != null) desc.Description = name;
            else customParamNames.Add(new ShortGUIDDescriptor{ ID = id, ID_cachedstring = id.ToString(), Description = name });
        }
        public static void RemoveNewParameterName(cGUID id)
        {
            ShortGUIDDescriptor desc = customParamNames.FirstOrDefault(o => o.ID == id);
            if (desc == null) return;
            customParamNames.Remove(desc);
        }
        //--
        public static void AddNewEntityName(cGUID id, string name)
        {
            ShortGUIDDescriptor desc = customEntityNames.FirstOrDefault(o => o.ID == id);
            if (desc != null) desc.Description = name;
            else customEntityNames.Add(new ShortGUIDDescriptor { ID = id, ID_cachedstring = id.ToString(), Description = name });
            EditorUtils.PurgeEntityNameFromCache(id);
        }
        public static void RemoveNewEntityName(cGUID id)
        {
            ShortGUIDDescriptor desc = customEntityNames.FirstOrDefault(o => o.ID == id);
            if (desc == null) return;
            customEntityNames.Remove(desc);
        }

        //Get parameter/entity name
        //We fall through to EntityDB here which means we can replace all EntityDB calls to Cathode/Editor name in the GUI app
        public static string GetParameterName(cGUID id)
        {
            string id_string = id.ToString();
            ShortGUIDDescriptor desc = customParamNames.FirstOrDefault(o => o.ID_cachedstring == id_string);
            if (desc == null) return EntityDB.GetCathodeName(id, CurrentInstance.commandsPAK);
            return desc.Description;
        }
        public static string GetEntityName(cGUID id)
        {
            string id_string = id.ToString();
            ShortGUIDDescriptor desc = customEntityNames.FirstOrDefault(o => o.ID_cachedstring == id_string);
            if (desc == null) return EntityDB.GetEditorName(id);
            return desc.Description;
        }

        public static string[] GetEntityParameterList(string entity_name)
        {
            //We should never need to correct EntityDB here, as you can't add custom parameters to hardcoded entities (for obvious reasons)
            //But... doing it anyway 
            string[] content = EntityDB.GetEntityParameterList(entity_name);
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
