using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace KSP2_Modded_Toggle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModManager modM;
        public MainWindow()
        {
            InitializeComponent();
            modM = new ModManager();

            kspDir.Text = modM.paths.KspPath;
            modsDir.Text = modM.paths.ModsPath;

            SetModState();
        }

        private void KspDirChange(object sender, RoutedEventArgs e)
        {
            modM.paths.KspPath = kspDir.Text;
            modM.saveConfig();
        }

        private void ModsDirChange(object sender, RoutedEventArgs e)
        {
            modM.paths.ModsPath = modsDir.Text;
            modM.saveConfig();
        }

        private void SetModState()
        {
            if (modM.ModsEnabled)
            {
                modState.Content = "Modded";
                modsEnabled.IsChecked = true;
            }
            else
            {
                modState.Content = "Un-Modded";
            }
        }

        private void ButtonApplyClick(object sender, RoutedEventArgs e)
        {
            if (modsEnabled.IsChecked == true)
            {
                Trace.WriteLine("Enabling Mods");
                modM.enableMods();
            } else
            {
                Trace.WriteLine("Disabling Mods");
                modM.disableMods();
            }

            SetModState();
            modM.saveConfig();
        }
    }
}
