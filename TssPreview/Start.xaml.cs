﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            catch (Exception)
            {
            }
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Classes\.tss\OpenWithList\TssPreview.exe"))
                {
                }
            }
            catch (Exception)
            {
            }

            InitializeComponent();

            string[] files = { };
            List<FileInfo> fileInfos = new List<FileInfo>();
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (Directory.Exists(dir))
            {
                files = Directory.GetFiles(dir, "*.tss");
            }
            
            for (int i = 0; i < files.Length; i++)
            {
                fileInfos.Add(new FileInfo(files[i]));

                files[i] = Path.GetFileName(files[i]);
            }

            fileInfos.Sort((a, b) => b.CreationTime.CompareTo(a.CreationTime));

            fileList.ItemsSource = fileInfos;

            string[] args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                if (args.Length >= 2)
                {
                    var str = args[1];
                    for (int i = 2; i < args.Length; i++)
                    {
                        str += " " + args[i];
                    }
                    if (File.Exists(str))
                    {
                        var newWin = new Race(str);
                        newWin.Show();
                        Close();
                    }
                }
            }

        }

        private void fileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var src = (FileInfo)fileList.SelectedValue;
            if (src.Extension == ".tss" || src.Extension == ".tssrace")
            {
                var newWin = new Race(src.FullName);
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
                var newWin = new Race(dialog.FileName);
                newWin.Show();
                Close();
            }
        }
    }
}
