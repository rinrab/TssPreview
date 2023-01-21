using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace TssPreview
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\Applications\TssPreview.exe\shell\open\command"))
                {
                    key.SetValue(null, string.Format("{0} \"%1\"", Assembly.GetExecutingAssembly().Location));
                }
            }
            catch (Exception) { }
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\.tss\OpenWithList\TssPreview.exe"))
                {
                }
            }
            catch (Exception) { }

            InitializeComponent();
            Title = string.Format("TSS Preview {0}", App.Version);

            string[] files = { };
            List<FileInfo> fileInfos = new List<FileInfo>();

            string[] dirs = new string[] {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            foreach (string dir in dirs)
            {
                try
                {
                    if (Directory.Exists(dir))
                    {
                        files = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(s => s.EndsWith(".tss") || s.EndsWith(".tssrace")).ToArray();
                    }
                    foreach (string file in files)
                    {
                        fileInfos.Add(new FileInfo(file));
                    }
                }
                catch (Exception) { }
            }

            fileInfos.Sort((a, b) => b.CreationTime.CompareTo(a.CreationTime));

            fileList.ItemsSource = fileInfos;

            string[] args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                if (args.Length >= 2)
                {
                    string str = args[1];
                    for (int i = 2; i < args.Length; i++)
                    {
                        str += " " + args[i];
                    }
                    if (File.Exists(str))
                    {
                        Race newWin = new Race(str);
                        newWin.Show();
                        Close();
                    }
                }
            }

        }

        private void fileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FileInfo src = (FileInfo)fileList.SelectedValue;
            if (src.Extension == ".tss" || src.Extension == ".tssrace")
            {
                Race newWin = new Race(src.FullName);
                newWin.Show();
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Game";
            dialog.DefaultExt = "(*.tss, *.tssgame)";
            dialog.Filter = "TSS Race (*.tss, *.tssrace)|*.tss;*.tssrace";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                Race newWin = new Race(dialog.FileName);
                newWin.Show();
                Close();
            }
        }
    }
}
