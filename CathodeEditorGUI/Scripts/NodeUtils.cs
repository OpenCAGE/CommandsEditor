using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CATHODE.EXPERIMENTAL.Lights;
using static CathodeLib.CompositePinInfoTable;

namespace CommandsEditor
{
    public static class NodeUtils
    {
        public static void AddAllPins(this STNode node, Composite composite, Commands commands)
        {
            switch (node.Entity.variant)
            {
                case EntityVariant.VARIABLE:
                    VariableEntity varEnt = (VariableEntity)node.Entity;
                    PinInfo info = commands.Utils.GetPinInfo(composite, varEnt);
                    switch (info.PinTypeGUID.AsCompositePinType)
                    {
                        case CompositePinType.CompositeInputAnimationInfoVariablePin:
                        case CompositePinType.CompositeInputBoolVariablePin:
                        case CompositePinType.CompositeInputDirectionVariablePin:
                        case CompositePinType.CompositeInputFloatVariablePin:
                        case CompositePinType.CompositeInputIntVariablePin:
                        case CompositePinType.CompositeInputObjectVariablePin:
                        case CompositePinType.CompositeInputPositionVariablePin:
                        case CompositePinType.CompositeInputStringVariablePin:
                        case CompositePinType.CompositeInputVariablePin:
                        case CompositePinType.CompositeInputZoneLinkPtrVariablePin:
                        case CompositePinType.CompositeInputZonePtrVariablePin:
                        case CompositePinType.CompositeInputEnumVariablePin:
                        case CompositePinType.CompositeInputEnumStringVariablePin:
                        case CompositePinType.CompositeOutputAnimationInfoVariablePin:
                        case CompositePinType.CompositeOutputBoolVariablePin:
                        case CompositePinType.CompositeOutputDirectionVariablePin:
                        case CompositePinType.CompositeOutputFloatVariablePin:
                        case CompositePinType.CompositeOutputIntVariablePin:
                        case CompositePinType.CompositeOutputObjectVariablePin:
                        case CompositePinType.CompositeOutputPositionVariablePin:
                        case CompositePinType.CompositeOutputStringVariablePin:
                        case CompositePinType.CompositeOutputVariablePin:
                        case CompositePinType.CompositeOutputZoneLinkPtrVariablePin:
                        case CompositePinType.CompositeOutputZonePtrVariablePin:
                        case CompositePinType.CompositeOutputEnumVariablePin:
                        case CompositePinType.CompositeOutputEnumStringVariablePin:
                            node.AddBottomOption(varEnt.name);
                            break;
                        case CompositePinType.CompositeMethodPin:
                            node.AddOutputOption(varEnt.name);
                            break;
                        case CompositePinType.CompositeTargetPin:
                            node.AddInputOption(varEnt.name);
                            break;
                        case CompositePinType.CompositeReferencePin:
                            node.AddTopOption(varEnt.name, PinStyle.ArrowDown);
                            break;
                    }
                    break;
                default:
                    List<(ShortGuid, ParameterVariant, DataType)> allParameters = commands.Utils.GetAllParameters(node.Entity, composite);
                    foreach ((ShortGuid, ParameterVariant, DataType) parameter in allParameters)
                    {
                        switch (parameter.Item2)
                        {
                            case ParameterVariant.INPUT_PIN:
                            case ParameterVariant.PARAMETER:
                            case ParameterVariant.STATE_PARAMETER:
                                node.AddTopOption(parameter.Item1, PinStyle.ArrowDown);
                                break;
                            case ParameterVariant.METHOD_PIN:
                                node.AddInputOption(parameter.Item1);
                                ShortGuid relay = commands.Utils.GetRelay(parameter.Item1);
                                if (relay != ShortGuid.Invalid)
                                    node.AddOutputOption(relay);
                                break;
                            case ParameterVariant.OUTPUT_PIN:
                                node.AddTopOption(parameter.Item1, PinStyle.ArrowUp);
                                break;
                            case ParameterVariant.TARGET_PIN:
                                node.AddOutputOption(parameter.Item1);
                                break;
                            case ParameterVariant.REFERENCE_PIN:
                                node.AddBottomOption(parameter.Item1);
                                break;
                        }

                        if (node.Entity.variant == EntityVariant.FUNCTION)
                        {
                            FunctionEntity func = (FunctionEntity)node.Entity;
                            switch (func.function.AsFunctionType)
                            {
                                case FunctionType.CAGEAnimation:
                                    CAGEAnimation cageAnim = (CAGEAnimation)func;
                                    foreach (CAGEAnimation.EventTrack track in cageAnim.events)
                                    {
                                        foreach (CAGEAnimation.EventTrack.Keyframe keyframe in track.keyframes)
                                        {
                                            if (keyframe.track_type != CAGEAnimation.TrackType.STRING)
                                                continue;

                                            node.AddOutputOption(keyframe.forward);
                                            node.AddOutputOption(keyframe.reverse);
                                        }
                                    }
                                    break;
                                case FunctionType.TriggerSequence:
                                    TriggerSequence triggerSeq = (TriggerSequence)func;
                                    foreach (TriggerSequence.MethodEntry method in triggerSeq.methods)
                                    {
                                        node.AddInputOption(method.method);
                                        node.AddOutputOption(method.relay);
                                        node.AddOutputOption(method.finished);
                                    }
                                    HashSet<ShortGuid> newTopOptions = new HashSet<ShortGuid>();
                                    HashSet<ShortGuid> checkedFunctionTypes = new HashSet<ShortGuid>();
                                    HashSet<ShortGuid> checkedEntityGuids = new HashSet<ShortGuid>();
                                    foreach (TriggerSequence.SequenceEntry entry in triggerSeq.sequence)
                                    {
                                        ShortGuid entryEntityGuid = entry.connectedEntity.GetPointedEntityID();
                                        if (checkedEntityGuids.Contains(entryEntityGuid))
                                            continue;
                                        checkedEntityGuids.Add(entryEntityGuid);

                                        (Composite entryComp, Entity entryEnt) = commands.Utils.GetResolvedTarget(commands.Utils.ResolveAlias(entry.connectedEntity, composite));
                                        if (entryEnt == null) continue;

                                        if (entryEnt.variant == EntityVariant.FUNCTION)
                                        {
                                            ShortGuid entryFunction = ((FunctionEntity)entryEnt).function;
                                            if (checkedFunctionTypes.Contains(entryFunction))
                                                continue;
                                            checkedFunctionTypes.Add(entryFunction);
                                        }

                                        List<(ShortGuid, ParameterVariant, DataType)> allParametersEntry = commands.Utils.GetAllParameters(entryEnt, entryComp);
                                        foreach ((ShortGuid, ParameterVariant, DataType) parameterEntry in allParametersEntry)
                                        {
                                            switch (parameterEntry.Item2)
                                            {
                                                //TODO: need to verify it is actually these three, and not just parameters
                                                case ParameterVariant.INPUT_PIN:
                                                case ParameterVariant.PARAMETER:
                                                case ParameterVariant.STATE_PARAMETER:
                                                    newTopOptions.Add(parameterEntry.Item1);
                                                    break;
                                            }
                                        }
                                    }
                                    foreach (ShortGuid topOption in newTopOptions)
                                        node.AddTopOption(topOption, PinStyle.ArrowDown);
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        public static void RemoveUnusedPins(this STNode node)
        {
            //Variable entities only ever have the right pins added
            if (node.Entity.variant == EntityVariant.VARIABLE)
                return;

            STNodeOption[] ins = node.GetInputOptions();
            for (int i = 0; i < ins.Length; i++)
                if (ins[i].ConnectionCount == 0)
                    node.RemoveInputOption(ins[i].ShortGUID);
            STNodeOption[] outs = node.GetOutputOptions();
            for (int i = 0; i < outs.Length; i++)
                if (outs[i].ConnectionCount == 0)
                    node.RemoveOutputOption(outs[i].ShortGUID);
            STNodeOption[] ups = node.GetTopOptions();
            for (int i = 0; i < ups.Length; i++)
                if (ups[i].ConnectionCount == 0)
                    node.RemoveTopOption(ups[i].ShortGUID);
            STNodeOption[] downs = node.GetBottomOptions();
            for (int i = 0; i < downs.Length; i++)
                if (downs[i].ConnectionCount == 0)
                    node.RemoveBottomOption(downs[i].ShortGUID);
        }
    }
}
