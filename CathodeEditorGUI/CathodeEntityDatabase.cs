using CATHODE;
using CATHODE.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CathodeEditorGUI
{
    public static class CathodeEntityDatabase
    {
        public struct EntityDefinition
        {
            public string guidName;
            public cGUID guid;
            public string className;
            public List<ParameterDefinition> parameters;
        }
        public struct ParameterDefinition
        {
            public string name;
            public string variable;
            public ParameterUsage usage;
            public string datatype; //todo:turn in to enum
        }
        public enum ParameterUsage
        {
            TARGET,
            STATE,
            INPUT,
            OUTPUT,
            PARAMETER,
            INTERNAL,
        }
        public enum ParameterDatatype
        {
            //wip
            INT,
            FLOAT,
            BOOL,
            STRING, //CATHODE:String
            DIRECTION, //CA::Vector

        }

        private static List<EntityDefinition> entities = new List<EntityDefinition>();
        static CathodeEntityDatabase()
        {
            JObject cathode_node_db = JObject.Parse(Properties.Resources.cathode_entities);
            foreach (JObject node in cathode_node_db["nodes"])
            {
                EntityDefinition entityDefinition = new EntityDefinition();
                entityDefinition.guidName = node["name"].Value<string>();
                entityDefinition.guid = Utilities.GenerateGUID(entityDefinition.guidName);
                entityDefinition.className = node["class"].Value<string>();
                entityDefinition.parameters = new List<ParameterDefinition>();
                foreach (JObject parameter in node["parameters"])
                {
                    ParameterDefinition parameterDefinition = new ParameterDefinition();
                    parameterDefinition.name = parameter["name"].Value<string>();
                    parameterDefinition.variable = parameter["variable"].Value<string>();
                    parameterDefinition.usage = (ParameterUsage)Enum.Parse(typeof(ParameterUsage), parameter["usage"].Value<string>().ToUpper());
                    parameterDefinition.datatype = parameter["datatype"].Value<string>();
                    entityDefinition.parameters.Add(parameterDefinition);
                }
                entities.Add(entityDefinition);
            }
        }

        public static string GetNodeNameByGUID(string guid_string)
        {
            return entities.FirstOrDefault(o => o.guidName == guid_string).className;
        }
        public static string GetNodeNameByGUID(cGUID guid_obj)
        {
            return entities.FirstOrDefault(o => o.guid == guid_obj).className;
        }

        public static List<ParameterDefinition> GetParametersByNodeName(string node_name)
        {
            return entities.FirstOrDefault(o => o.guidName == node_name).parameters;
        }

        public static ParameterDefinition GetParameterFromNodeByNodeName(string node_name, string parameter_name)
        {
            return entities.FirstOrDefault(o => o.guidName == node_name).parameters.FirstOrDefault(o => o.name == parameter_name);
        }

        public static CathodeParameter ParameterDefinitionToParameter(ParameterDefinition def)
        {
            CathodeParameter this_param = null;
            switch (def.datatype.ToUpper())
            {
                case "POSITION":
                    this_param = new CathodeTransform();
                    break;
                case "FLOAT":
                    this_param = new CathodeFloat();
                    break;
                case "FILEPATH":
                case "STRING":
                    this_param = new CathodeString();
                    break;
                case "SPLINEDATA":
                    this_param = new CathodeSpline();
                    break;
                case "BOOL":
                    this_param = new CathodeBool();
                    break;
                case "DIRECTION":
                    this_param = new CathodeVector3();
                    break;
                case "INT":
                    this_param = new CathodeInteger();
                    break;
                    /*
                case "ENUM":
                    thisParam = new CathodeEnum();
                    ((CathodeEnum)thisParam).enumID = new cGUID("4C-B9-82-48"); //ALERTNESS_STATE is the first alphabetically
                    break;
                case CathodeDataType.SHORT_GUID:
                    thisParam = new CathodeResource();
                    ((CathodeResource)thisParam).resourceID = new cGUID("00-00-00-00");
                    break;
                    */
            }
            return this_param;
        }
    }
}
