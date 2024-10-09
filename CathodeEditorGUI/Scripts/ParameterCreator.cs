using CATHODE.Scripting.Internal;
using CATHODE.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandsEditor.DockPanels;

namespace CommandsEditor
{
    public class ParameterCreator
    {
        Entity _ent = null;
        FunctionEntity _funcEnt = null; //NOTE: This is not necessarily the entity we want to apply the parameters to - it's instead the "truth" that we should get the parameter list and datatypes from
        ParameterData _param = null;

        string _name = "";

        public FunctionEntity RootFunc => _funcEnt;

        public ParameterCreator(Entity ent, Composite comp)
        {
            _ent = ent;

            switch (_ent.variant)
            {
                case EntityVariant.PROXY:
                    Entity proxiedEntity = CommandsUtils.ResolveHierarchy(Singleton.Editor?.CommandsDisplay?.Content?.commands, comp, ((ProxyEntity)_ent).proxy.path, out Composite c, out string h);
                    if (proxiedEntity.variant == EntityVariant.FUNCTION)
                        _funcEnt = (FunctionEntity)proxiedEntity;
                    break;
                case EntityVariant.ALIAS:
                    Entity aliasedEntity = CommandsUtils.ResolveHierarchy(Singleton.Editor?.CommandsDisplay?.Content?.commands, comp, ((AliasEntity)_ent).alias.path, out Composite c2, out string h2);
                    if (aliasedEntity.variant == EntityVariant.FUNCTION)
                        _funcEnt = (FunctionEntity)aliasedEntity;
                    break;
                case EntityVariant.FUNCTION:
                    _funcEnt = (FunctionEntity)_ent;
                    break;
            }
        }

        public string GetInfo(string param_name, string fallback_type = "FLOAT")
        {
            _param = null;
            _name = param_name;
            if (_funcEnt != null)
            {
                bool isComposite = !CommandsUtils.FunctionTypeExists(_funcEnt.function);
                ShortGuid function = (isComposite) ? CommandsUtils.GetFunctionTypeGUID(FunctionType.CompositeInterface) : _funcEnt.function;

                CathodeEntityDatabase.ParameterDefinition def = CathodeEntityDatabase.GetParameterFromEntity(function, param_name);

                //Fallback for Proxied entities: also check ProxyInterface
                if (def.name == null && _ent.variant == EntityVariant.PROXY)
                    def = CathodeEntityDatabase.GetParameterFromEntity(ShortGuidUtils.Generate("ProxyInterface"), param_name);

                if (def.name != null)
                {
                    switch (def.usage)
                    {
                        case CathodeEntityDatabase.ParameterUsage.REFERENCE:
                        case CathodeEntityDatabase.ParameterUsage.TARGET:
                        case CathodeEntityDatabase.ParameterUsage.METHOD:
                        case CathodeEntityDatabase.ParameterUsage.FINISHED:
                        case CathodeEntityDatabase.ParameterUsage.RELAY:
                            return "FLOAT"; //The FLOAT datatype is used a placeholder for this.
                        default:
                            _param = CathodeEntityDatabase.ParameterDefinitionToParameter(def);
                            if (_param == null)
                                return fallback_type;
                            return _param.dataType.ToString();
                    }
                }

                //if we're a composite & didn't find the param from CompositeInterface, try check the actual composite
                if (isComposite)
                {
                    ShortGuid param = ShortGuidUtils.Generate(param_name);
                    VariableEntity var = Singleton.Editor?.CommandsDisplay?.Content?.commands?.GetComposite(_funcEnt.function)?.variables?.FirstOrDefault(o => o.name == param);
                    if (var == null || var.type == DataType.NONE)
                        return fallback_type;
                    return var.type.ToString();
                }
            }

            return fallback_type;
        }

        public void Create(string param_name, string fallback_type="FLOAT")
        {
            string type = GetInfo(param_name, fallback_type);

            if (_param != null)
                _ent.AddParameter(_name, _param);
            else
                _ent.AddParameter(_name, (DataType)Enum.Parse(typeof(DataType), type));
        }
    }
}
