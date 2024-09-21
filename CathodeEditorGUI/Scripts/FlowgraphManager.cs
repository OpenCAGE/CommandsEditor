using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;

namespace CommandsEditor
{
    //Handles loading vanilla/custom flowgraph layouts, and saving custom layouts
    public class FlowgraphManager
    {
        private static CompositeFlowgraphsTable _vanilla;
        private static CompositeFlowgraphsTable _custom;

        public static Commands LinkedCommands => _commands;
        private static Commands _commands;

        static FlowgraphManager()
        {
            _vanilla = new CompositeFlowgraphsTable();
            _custom = new CompositeFlowgraphsTable();

            //TODO: need to populate flowgraphs.bin by converting the content in NodePositionDatabase

            using (BinaryReader reader = new BinaryReader(new MemoryStream(Properties.Resources.flowgraphs)))
            {
                _vanilla.Read(reader);
            }
        }

        public static void LinkCommands(Commands commands)
        {
            if (_commands != null)
            {
                _commands.OnLoadSuccess -= LoadCustomFlowgraphs;
                _commands.OnSaveSuccess -= SaveCustomFlowgraphs;
            }

            _commands = commands;
            if (_commands == null) return;

            _commands.OnLoadSuccess += LoadCustomFlowgraphs;
            _commands.OnSaveSuccess += SaveCustomFlowgraphs;

            LoadCustomFlowgraphs(_commands.Filepath);
        }

        private static void LoadCustomFlowgraphs(string filepath)
        {
            _custom = (CompositeFlowgraphsTable)CustomTable.ReadTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPHS);
            if (_custom == null) _custom = new CompositeFlowgraphsTable();
            Console.WriteLine("Loaded " + _custom.flowgraphs.Count + " custom flowgraph layouts!");
        }

        private static void SaveCustomFlowgraphs(string filepath)
        {
            CustomTable.WriteTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPHS, _custom);
            Console.WriteLine("Saved " + _custom.flowgraphs.Count + " custom flowgraph layouts!");
        }
    }
}
