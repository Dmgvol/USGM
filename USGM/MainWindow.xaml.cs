using System;
using System.IO;
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
            string[] files = Directory.GetFiles(Config.GetInstance().SavesFolder, "*.sav", SearchOption.AllDirectories);
            saveListBox.Items.Clear();
            foreach(string file in files) {
                saveListBox.Items.Add(new ListBoxItem() { Content = Path.GetFileNameWithoutExtension(file) });
            }
        }

        private void InitSaveSlots() {
            for(int i = 0; i < Config.GetInstance().TotalSlots; i++) {
                saveSlotSelector.Items.Add(Config.GetInstance().SaveFilePrefix.Replace("{n}", "" + (Config.GetInstance().SaveFileCountStart + i)));
            }
        }

        private void LoadSelectedSave() {
            // anything selected?
            if(saveListBox.SelectedItem == null) {
                MessageBox.Show("Please select a file to load!", "No file selected");
                return;
            }
            // file to load
            string fileToLoad = Config.GetInstance().SavesFolder + "\\" + ((ListBoxItem)saveListBox.SelectedItem).Content.ToString() + ".sav";
            // still exists?
            if(!File.Exists(fileToLoad)) {
                MessageBox.Show("Save file can't be located. Has it been moved or deleted?", "File not found");
                GetSaveFiles();
                return;
            }

            // Try to copy
            string pathToSave = Config.GetInstance().SaveFilePrefix.Replace("{n}", "" + (saveSlotSelector.SelectedIndex + Config.GetInstance().SaveFileCountStart)) + ".sav";
            try {
                File.Copy(fileToLoad, pathToSave, true);
            } catch(Exception exception) {
                MessageBox.Show(exception.Message, "Error");
            }
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e) => GetSaveFiles();

        private void loadSaveBtn_Click(object sender, RoutedEventArgs e) => LoadSelectedSave();
    }
}
