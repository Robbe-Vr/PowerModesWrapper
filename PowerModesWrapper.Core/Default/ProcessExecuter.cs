using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace PowerModesWrapper.Core.Default
{
    internal class ProcessExecuter
    {
        internal static string[] Execute(string commando)
        {
            using (Process command = new Process())
            {
                command.StartInfo.FileName = "cmd.exe";
                command.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                command.StartInfo.CreateNoWindow = true;
                command.StartInfo.UseShellExecute = false;

                command.StartInfo.RedirectStandardInput = true;
                command.StartInfo.RedirectStandardOutput = true;
                command.StartInfo.RedirectStandardError = true;

                command.StartInfo.WorkingDirectory = @"C:\";

                command.Start();

                command.StandardInput.WriteLine(commando + command.StandardInput.NewLine);
                command.StandardInput.Flush();
                command.StandardInput.Close();

                Thread.Sleep(100);

                StringBuilder strOuput = new StringBuilder();
                while (command.StandardOutput.Peek() > -1)
                {
                    string line = command.StandardOutput.ReadLine();
                    if (line.StartsWith("Power Scheme GUID"))
                    {
                        strOuput.AppendLine(line);
                    }
                }

                StringBuilder strError = new StringBuilder();
                while (command.StandardError.Peek() > -1)
                {
                    string line = command.StandardError.ReadLine();
                    strError.AppendLine(line);
                }

                command.WaitForExit();
                command.Close();

                string output = strOuput.ToString();
                string error = strError.ToString();

                if (output.Length > 0 && output.StartsWith("\r\n\r\n"))
                {
                    return new string[] { output, error };
                }
                else
                {
                    Logger.Log($"Error executing command {commando}! Error: {error}\n");
                    return new string[] { output, error };
                }
            }
        }

        internal static List<PowerMode> ExecuteGet(string commando)
        {
            List<PowerMode> powerModes = new List<PowerMode>();

            string[] response = Execute(commando);

            string output = response[0];
            string error = response[1];

            if (output.Length > 0)
            {
                string[] powerSchemeInfos = output.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string powerSchemeInfo in powerSchemeInfos)
                {
                    var infopoints = powerSchemeInfo.Split(':')[1].Split('(');

                    powerModes.Add(new PowerMode()
                    {
                        Guid = infopoints[0].Trim(),
                        Name = infopoints[1].Split(')')[0],
                    });
                }
            }
            else
            {
                Logger.Log($"Error executing get command {commando}! Error: {error}\n");
            }

            return powerModes;
        }
    }
}
