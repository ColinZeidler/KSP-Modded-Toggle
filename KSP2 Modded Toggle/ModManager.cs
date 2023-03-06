
using System;
using System.IO;

namespace KSP2_Modded_Toggle
{

    internal class ModManager
    {
        public string kspPath;
        public string modsPath;
        public bool ModsEnabled { get; private set; }


        public readonly string[] items =
        {
            $"BepInEx{Path.DirectorySeparatorChar}",
            "doorstop_config.ini",
            "winhttp.dll"
        };

        public ModManager()
        {
            kspPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program 2";
            modsPath = "";

            checkMods();
        }

        private void checkMods()
        {
            ModsEnabled = Directory.Exists(composePath(kspPath, items[0])) && File.Exists(composePath(kspPath, items[1]));
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
                        Directory.CreateSymbolicLink(composePath(this.kspPath, item), composePath(modsPath, item));
                    }
                    else
                    {
                        File.CreateSymbolicLink(composePath(this.kspPath, item), composePath(modsPath, item));
                    }
                } catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
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
                        Directory.Delete(composePath(kspPath, item));
                    }
                    else
                    {
                        File.Delete(composePath(this.kspPath, item));
                    }
                } catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            this.ModsEnabled = false;
        }
    }
}
