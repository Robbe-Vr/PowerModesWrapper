using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace PowerModesWrapper.Core.Default
{
    internal partial class PowerModesControl
    {
        internal PowerModesControl()
        {
            
        }

        public static string Set(string mode)
        {
            try
            {
                List<PowerMode> modes = ProcessExecuter.ExecuteGet("powercfg -L");

                mode = modes.FirstOrDefault(x => x.Guid == mode || x.Name == mode)?.Guid;

                if (String.IsNullOrEmpty(mode)) return "UNKNOWN MODE";

                ProcessExecuter.Execute("powercfg -setactive " + mode);

                return "EXECUTED";
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to set powermode {mode}!\nError: {e.Message}");
                return "FAIL";
            }
        }
    }
}
