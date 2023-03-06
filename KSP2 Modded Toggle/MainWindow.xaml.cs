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

            kspDir.Text = modM.kspPath;
            modsDir.Text = modM.modsPath;

            SetModState();
        }

        private void KspDirChange(object sender, RoutedEventArgs e)
        {
            modM.kspPath = kspDir.Text;
        }

        private void ModsDirChange(object sender, RoutedEventArgs e)
        {
            modM.modsPath = modsDir.Text;
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
                Console.WriteLine("Enabling Mods");
                modM.enableMods();
            } else
            {
                Console.WriteLine("Disabling Mods");
                modM.disableMods();
            }

            SetModState();
        }
    }
}
