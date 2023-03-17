
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System;

namespace KSP2_Modded_Toggle
{
    internal class ModManager
    {
        public ModConfigs mods;
        private static readonly string configFile = "configuration.json";
        public bool ModsEnabled { get; private set; }

        public KSPEnvironment ActiveEnvironment;

        public readonly string[] items =
        {
            $"BepInEx{Path.DirectorySeparatorChar}",
            "doorstop_config.ini",
            "winhttp.dll"
        };

        public ModManager()
        {
            string jsonText = "";
            ModConfigs config = null;
            try
            {
                jsonText = File.ReadAllText(configFile);
                config = JsonSerializer.Deserialize<ModConfigs>(jsonText);
            } catch (IOException e)
            {
                Trace.WriteLine(e.ToString());
            }

            if (config != null )
            {
                mods = config;
            } else
            {
                mods = new ModConfigs
                {
                    KspPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program 2",
                    ModEnvironments = new ObservableCollection<KSPEnvironment> {}
                };
            }

            ActiveEnvironment = null;
            foreach (var env in mods.ModEnvironments)
            {
                if (env.Active)
                {
                    ActiveEnvironment = env;
                    break;
                }
            }

            CheckMods();
        }

        public void SaveConfig()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(mods, options);
            File.WriteAllText(configFile, jsonString);
        }

        private void CheckMods()
        {
            ModsEnabled = IsModded();
        }

        private bool IsModded()
        {
            return Directory.Exists(Path.Join(mods.KspPath, items[0])) && File.Exists(Path.Join(mods.KspPath, items[1]));
        }

        public void AddEnvironment(KSPEnvironment env)
        {
            if (mods.ModEnvironments == null)
            {
                mods.ModEnvironments = new ObservableCollection<KSPEnvironment>();
            }
            mods.ModEnvironments.Add(env);
            SaveConfig();
        }

        public void ApplyEnvironment(KSPEnvironment env)
        {
            if (ActiveEnvironment != null)
            {
                DisableEnvironment(ActiveEnvironment);
            }
            EnableEnvironment(env);
            ActiveEnvironment = env;
            SaveConfig();
        }

        public void EditEnvironment(KSPEnvironment editedEnv, KSPEnvironment oldEnv)
        {
            // Lookup existing Env in list (match against Active)
            int pos;
            for (pos=0; pos < mods.ModEnvironments.Count; pos++)
            {
                if (mods.ModEnvironments[pos] == oldEnv)
                {
                    mods.ModEnvironments[pos] = editedEnv;
                    break;
                }
            }
            SaveConfig();
        }

        public void DeleteEnv(KSPEnvironment deletedEnv)
        {
            mods.ModEnvironments.Remove(deletedEnv);
        }

        private void EnableEnvironment(KSPEnvironment env)
        {
            if (env.Modded)
            {
                foreach (var item in this.items)
                {
                    try
                    {
                        if (Path.EndsInDirectorySeparator(item))
                        {
                            Directory.CreateSymbolicLink(Path.Join(mods.KspPath, item), Path.Join(env.ModsDirectory, item));
                        }
                        else
                        {
                            File.CreateSymbolicLink(Path.Join(mods.KspPath, item), Path.Join(env.ModsDirectory, item));
                        }
                    }
                    catch (IOException e)
                    {
                        Trace.WriteLine(e.ToString());
                    }
                }
            }
            env.Active = true;
        }

        private void DisableEnvironment(KSPEnvironment env)
        {
            if (IsModded())
            {
                foreach (var item in this.items)
                {
                    try
                    {
                        if (Path.EndsInDirectorySeparator(item))
                        {
                            Directory.Delete(Path.Join(mods.KspPath, item));
                        }
                        else
                        {
                            File.Delete(Path.Join(mods.KspPath, item));
                        }
                    }
                    catch (IOException e)
                    {
                        Trace.WriteLine(e.ToString());
                    }
                }
            }
            env.Active = false;
        }
    }
}
