using PowerModesWrapper.Core.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SYWPipeNetworkManager;

namespace PowerModesWrapper.Core
{
    public class PowerModesManager
    {
        private IEnumerable<string> validatedSources = new List<string>()
        {
            "MidiDomotica"
        };

        public bool Setup()
        {
            PipeMessageControl.Init("PowerModes");
            PipeMessageControl.StartClient(
                (sourceName, message) =>
                {
                    if (ValidateSource(sourceName))
                    {
                        return $"{message} -> " + ProcessMessage(message);
                    }
                    else return $"{message} -> NO";
                }
            );

            return true;
        }

        private bool ValidateSource(string source)
        {
            return validatedSources.Contains(source);
        }

        public string ProcessMessage(string message)
        {
            IEnumerable<string> parts = new Regex(@"(::\[|\]::|::)|]$").Split(message).Where(x => !String.IsNullOrWhiteSpace(x) && !x.Contains("::")).Select(x => x.Trim());

            switch (parts.FirstOrDefault())
            {
                case "Get":
                    return ProcessGetCommand(parts.Skip(1));

                case "Set":
                    return PowerModesControl.Set(parts.Skip(1)?.FirstOrDefault());
            }

            return "INVALID DATA";
        }

        private string ProcessGetCommand(IEnumerable<string> parts)
        {
            switch (parts.FirstOrDefault())
            {
                case "Name":
                    return PowerModesControl.GetName(parts.Skip(1)?.FirstOrDefault());

                case "Id":
                    return PowerModesControl.GetId(parts.Skip(1)?.FirstOrDefault());

                case "All":
                    return PowerModesControl.GetModes();
            }

            return "UNKNOWN GET";
        }
    }
}
