using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerModesWrapper.Core
{
    internal class Preferences
    {

        internal void Load()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MidiDomotica-PowerModesWrapper", "preferences.json");

            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);

                Preferences pref = System.Text.Json.JsonSerializer.Deserialize<Preferences>(jsonString);

            }
            else
            {
                Store();
            }
        }

        internal void Store()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MidiDomotica-PowerModesWrapper");

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string path = Path.Combine(dir, "preferences.json");

            string content = System.Text.Json.JsonSerializer.Serialize(this);

            File.WriteAllText(path, content);
        }
    }
}
