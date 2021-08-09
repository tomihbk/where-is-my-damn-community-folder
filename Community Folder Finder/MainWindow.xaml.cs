using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Community_Folder_Finder
{
    public partial class MainWindow : Window
    {
        private static readonly string relPath = Path.Combine(Directory.GetCurrentDirectory());
        private readonly string AppDataLocalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        private readonly string AppDataRoamingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        private readonly string WINDOWS_STORE_USERCFG_PATH;
        private readonly string STEAM_USERCFG_PATH;
        private bool fileNotFound;
        public MainWindow()
        {
            InitializeComponent();
            WINDOWS_STORE_USERCFG_PATH = $"{AppDataLocalPath}\\Packages\\Microsoft.FlightSimulator_8wekyb3d8bbwe\\LocalCache\\UserCfg.opt";
            STEAM_USERCFG_PATH = $"{AppDataRoamingPath}\\Microsoft Flight Simulator\\UserCfg.opt";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string comPath = CommunityFolderPath();
            if (!fileNotFound)
            {
                tbxCommunityPath.Text = comPath;
            }
            else
            {
                tbxCommunityPath.Text = "You might not be using a legit MSFS or you must have your installation files not at the default location.";
                btnCopyToCommunity.Visibility = Visibility.Hidden;
                btnGoToCommunity.Visibility = Visibility.Hidden;
            }
        }

        private string CommunityFolderPath()
        {
            string userCFGpath;
            //Check if the usercfg was found
            if (File.Exists(WINDOWS_STORE_USERCFG_PATH))
            {
                userCFGpath = WINDOWS_STORE_USERCFG_PATH;
                fileNotFound = false;
            }
            else if (File.Exists(STEAM_USERCFG_PATH))
            {
                userCFGpath = STEAM_USERCFG_PATH;
                fileNotFound = false;
            }
            else
            {
                //If not just go to the relative path
                userCFGpath = Path.GetFullPath(relPath);
                fileNotFound = true;
            }

            if (userCFGpath != Path.GetFullPath(relPath))
            {
                Regex r = new Regex(@"InstalledPackagesPath\s?\""?([^""]*)\""?");
                string text = File.ReadAllText(userCFGpath);
                userCFGpath = $"{r.Match(text).Groups[1].Value}\\Community"; //This opens the community folder
            }
            return userCFGpath;
        }

        public bool ExploreFile(string filePath)
        {
            Process.Start("explorer.exe", string.Format("\"{0}\"", filePath));
            return true;
        }

        private void BtnCopyPath_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tbxCommunityPath.Text);
            btnCopyToCommunity.Content = "Path Copied";
            btnCopyToCommunity.Foreground = Brushes.Green;
        }

        private void BtnGoToCommunity_Click(object sender, RoutedEventArgs e)
        {
            ExploreFile(tbxCommunityPath.Text);
        }

    }
}
