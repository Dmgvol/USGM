using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace USGM {
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            Config.GetInstance();
            GetSaveFiles();
            InitSaveSlots();
        }

        private void GetSaveFiles() {
            if(!Directory.Exists(Config.GetInstance().SavesFolder)) {
                MessageBox.Show("Can't find the \"Saves\" directory", "Directory not found");
                return;
            }

            List<string> files;
            if(Config.GetInstance().IsComplex) {
                // Complex - get directories
                files = Directory.GetDirectories(Config.GetInstance().SavesFolder)
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToList();
            } else {
                // Simple - get actual .sav files
                files = Directory.GetFiles(Config.GetInstance().SavesFolder, "*.sav", SearchOption.AllDirectories)
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToList();
            }

            // Sort if needed
            saveListBox.Items.Clear();
            if(Config.GetInstance().Sort)
                files = files.OrderBy(k => k, new SaveComparer()).ToList();

                // Add to ListBox
            files.ForEach(x => saveListBox.Items.Add(new ListBoxItem() { Content = x }));

        }

        private void InitSaveSlots() {
            if(Config.GetInstance().IsComplex) {
                // Complex - Name slots with "Slot {n}"
                for(int i = 0; i < Config.GetInstance().TotalSlots; i++) {
                    saveSlotSelector.Items.Add("Slot " + (Config.GetInstance().SaveFileCountStart + i));
                }
            } else {
                // Simple - Name slots with actual SaveFilePrefix
                for(int i = 0; i < Config.GetInstance().TotalSlots; i++) {
                    saveSlotSelector.Items.Add(Config.GetInstance().SaveFilePrefix[0].Replace("{n}", "" + (Config.GetInstance().SaveFileCountStart + i)));
                }
            }
        }

        private void LoadSelectedSave() {
            // anything selected?
            if(saveListBox.SelectedItem == null) {
                MessageBox.Show("Please select a file to load!", "No file selected");
                return;
            }

            if(Config.GetInstance().IsComplex) {
                // Complex - load all nested .sav files based on SaveFilePrefix items
                string[] filesToLoad = Directory.GetFiles(Config.GetInstance().SavesFolder + "\\" + ((ListBoxItem)saveListBox.SelectedItem).Content.ToString());
                int currSlotIndex = (saveSlotSelector.SelectedIndex + Config.GetInstance().SaveFileCountStart);
                for (int i = 0; i < filesToLoad.Length; i++) {
                    // Get the matching file pattern
                    string matchedPrefix = GetMatchedPrefix(filesToLoad[i]);
                    if (string.IsNullOrEmpty(matchedPrefix)) continue;

                    // copy patterned file
                    string pathToSave = matchedPrefix.Replace("{n}", "" + currSlotIndex) + ".sav";
                    try {
                        File.Copy(filesToLoad[i], pathToSave, true);
                    } catch(Exception exception) {
                        MessageBox.Show(exception.Message, "Error");
                    }
                }

            } else {
                // Simple - load .sav file directly based on SaveFilePrefix

                // file to load
                string fileToLoad = Directory.GetFiles(Config.GetInstance().SavesFolder, "*.sav", SearchOption.AllDirectories)
                    .Where(x => x.EndsWith(((ListBoxItem)saveListBox.SelectedItem).Content.ToString() + ".sav"))
                    .FirstOrDefault();
                // still exists?
                if(!string.IsNullOrEmpty(fileToLoad) && !File.Exists(fileToLoad)) {
                    MessageBox.Show("Save file can't be located. Has it been moved or deleted?", "File not found");
                    GetSaveFiles();
                    return;
                }

                // Try to copy
                string pathToSave = Config.GetInstance().SaveFilePrefix[0].Replace("{n}", "" + (saveSlotSelector.SelectedIndex + Config.GetInstance().SaveFileCountStart)) + ".sav";
                try {
                    File.Copy(fileToLoad, pathToSave, true);
                } catch(Exception exception) {
                    MessageBox.Show(exception.Message, "Error");
                }
            }
        }

        private string GetMatchedPrefix(string file) {
            for (int i = Config.GetInstance().SaveFileCountStart; i <= Config.GetInstance().TotalSlots; i++) {
                var matchedPrefix = Config.GetInstance().SaveFilePrefix
                       .FirstOrDefault(x => Path.GetFileNameWithoutExtension(file.Replace("{n}", "" + i))
                       .Equals(x.Replace("{n}", "" + i)));
                if (!string.IsNullOrEmpty(matchedPrefix)) return matchedPrefix;
            }
            return string.Empty;
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e) => GetSaveFiles();

        private void loadSaveBtn_Click(object sender, RoutedEventArgs e) => LoadSelectedSave();
    }

    /// <summary>
    /// Comparer for listed save names, custom for sorting numbers as well.
    /// </summary>
    class SaveComparer : IComparer<string> {
        public int Compare(string x, string y) {
            var arr1 = x.Split(' ', '-', '.');
            var arr2 = y.Split(' ', '-', '.');
            if(arr1.Length == 0 || arr2.Length == 0)
                return string.Compare(x, y, true);

            if(Config.IsNumeric(arr1[0]) && Config.IsNumeric(arr2[0])) {

                int left = int.Parse(arr1[0]);
                int right = int.Parse(arr2[0]);
                if(right > left)
                    return -1;
                else if(right < left)
                    return 1;
                else
                    return 0;
            } else {
                return string.Compare(x, y, true);
            }
        }
    }
}