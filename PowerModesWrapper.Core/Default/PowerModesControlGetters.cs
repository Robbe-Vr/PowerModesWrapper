using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerModesWrapper.Core.Default
{
    internal partial class PowerModesControl
    {
        public static string GetId(string name)
        {
            List<PowerMode> modes = ProcessExecuter.ExecuteGet("powercfg -L");

            return modes.FirstOrDefault(x => x.Name == name)?.Guid ?? "UNKNOWN MODE";
        }

        public static string GetName(string id)
        {
            List<PowerMode> modes = ProcessExecuter.ExecuteGet("powercfg -L");

            return modes.FirstOrDefault(x => x.Guid == id)?.Name ?? "UNKNOWN MODE";
        }

        public static string GetModes()
        {
            return String.Join(',', ProcessExecuter.ExecuteGet("powercfg -L").Select(x => $"{x.Guid}:{x.Name}"));
        }
    }
}
