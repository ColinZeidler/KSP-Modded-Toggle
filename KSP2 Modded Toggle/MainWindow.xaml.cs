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
        private ModManager ModManager;
        private KSPEnvironment ViewedEnv;
        public MainWindow()
        {
            ModManager = new ModManager();
            InitializeComponent();
            ModList.ItemsSource = ModManager.mods.ModEnvironments;


            if (ModManager.ActiveEnvironment == null)
            {
                ViewedEnv = new KSPEnvironment
                {
                    Name = "",
                    Modded = true,
                    ModsDirectory = ""
                };
            } else
            {
                ViewedEnv = ModManager.ActiveEnvironment;
                ModList.SelectedItem = ViewedEnv;
            }
            UpdateUI(ViewedEnv);
        }

        private void KspDirChange(object sender, RoutedEventArgs e)
        {
            ModManager.mods.KspPath = kspDir.Text;
            ModManager.SaveConfig();
        }

        private void ButtonApplyClick(object sender, RoutedEventArgs e)
        {
            ModManager.ApplyEnvironment(ViewedEnv);
            UpdateUI(ViewedEnv);
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            var editedEnv = new KSPEnvironment
            {
                Name = name.Text,
                ModsDirectory = modsDir.Text,
                Modded = (bool)modsEnabled.IsChecked
            };

            ModManager.EditEnvironment(editedEnv, ViewedEnv);
            UpdateUI(editedEnv);
        }

        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            ViewedEnv = new KSPEnvironment
            {
                Name = "New Mod Env",
                Modded = true,
                ModsDirectory = ""
            };
            ModManager.AddEnvironment(ViewedEnv);
            //ModList.ItemsSource = ModManager.mods.ModEnvironments;
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            ModManager.DeleteEnv(ViewedEnv);
            if (ModManager.mods.ModEnvironments.Count > 0)
            {
                ViewedEnv = ModManager.mods.ModEnvironments[0];
                UpdateUI(ViewedEnv);
            } else { ViewedEnv = null; }
        }

        private void ModList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ModList.SelectedItem as KSPEnvironment;
            if (item != null)
            {
                UpdateUI(item);
            }
        }

        private void UpdateUI(KSPEnvironment displayEnv)
        {
            if (displayEnv == null)
            {
                modsDir.Text = "";
                name.Text = "";
                modsEnabled.IsChecked = false;
                ViewedEnv = displayEnv;
            }
            kspDir.Text = ModManager.mods.KspPath;
            modsDir.Text = displayEnv.ModsDirectory;
            name.Text = displayEnv.Name;
            modsEnabled.IsChecked = displayEnv.Modded;
            active.Content = displayEnv.Active.ToString();
            ViewedEnv = displayEnv;
        }
    }
}
