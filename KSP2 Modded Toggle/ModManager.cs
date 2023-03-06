
using System;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

namespace KSP2_Modded_Toggle
{

    public class PathConfigs
    {
        public string KspPath { get; set; }
        public string ModsPath { get; set; }
    }

    internal class ModManager
    {
        public PathConfigs paths;
        private static string configFile = "configuration.json";
        public bool ModsEnabled { get; private set; }


        public readonly string[] items =
        {
            $"BepInEx{Path.DirectorySeparatorChar}",
            "doorstop_config.ini",
            "winhttp.dll"
        };

        public ModManager()
        {
            string jsonText = "";
            PathConfigs pathConfigs = null;
            try
            {
                jsonText = File.ReadAllText(configFile);
                pathConfigs = JsonSerializer.Deserialize<PathConfigs>(jsonText);
            } catch (IOException e)
            {
                Trace.WriteLine(e.ToString());
            }
            if (pathConfigs != null )
            {
                paths = pathConfigs;
            } else
            {
                paths = new PathConfigs();
                paths.KspPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program 2";
                paths.ModsPath = "";
            }

            checkMods();
        }

        public void saveConfig()
        {
            string jsonString = JsonSerializer.Serialize(paths);
            File.WriteAllText(configFile, jsonString);
        }

        private void checkMods()
        {
            ModsEnabled = Directory.Exists(composePath(paths.KspPath, items[0])) && File.Exists(composePath(paths.KspPath, items[1]));
        }

        private static string composePath(string dir, string item)
        {
            return Path.Join(dir, item);
        }

        public void enableMods()
        {
            foreach (var item in this.items)
            {
                try
                {
                    if (Path.EndsInDirectorySeparator(item))
                    {
                        Directory.CreateSymbolicLink(composePath(this.paths.KspPath, item), composePath(paths.ModsPath, item));
                    }
                    else
                    {
                        File.CreateSymbolicLink(composePath(this.paths.KspPath, item), composePath(paths.ModsPath, item));
                    }
                } catch (IOException e)
                {
                    Trace.WriteLine(e.ToString());
                }
            }
            this.ModsEnabled = true;
        }

        public void disableMods()
        {
            foreach(var item in this.items)
            {
                try
                {
                    if (Path.EndsInDirectorySeparator(item))
                    {
                        Directory.Delete(composePath(paths.KspPath, item));
                    }
                    else
                    {
                        File.Delete(composePath(this.paths.KspPath, item));
                    }
                } catch (IOException e)
                {
                    Trace.WriteLine(e.ToString());
                }
            }
            this.ModsEnabled = false;
        }
    }
}
