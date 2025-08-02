using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsEditor
{
    public static class Debug
    {
        public static void Log(string system, string message)
        {
#if DEBUG
            Console.WriteLine($"[{system.ToUpper()}] {message}");
#endif
        }
    }
}
