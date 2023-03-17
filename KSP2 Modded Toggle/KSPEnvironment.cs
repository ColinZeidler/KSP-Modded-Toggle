using System.Collections.ObjectModel;

namespace KSP2_Modded_Toggle
{
    public class KSPEnvironment
    {
        public string ModsDirectory { get; set; }
        public bool Modded { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }

    public class ModConfigs
    {
        public string KspPath { get; set; }
        public ObservableCollection<KSPEnvironment> ModEnvironments { get; set; }
    }
}
