﻿using CATHODE.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CathodeEditorGUI
{
    public static class CurrentInstance
    {
        public static CommandsPAK commandsPAK;
        public static CathodeFlowgraph selectedFlowgraph;
        public static CathodeEntity selectedEntity;
    }
}