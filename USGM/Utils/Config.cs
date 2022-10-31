using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace USGM {
    public class Config {
        private static Config Instance;
        public static Config GetInstance() {
            if(Instance == null) Instance = new Config();
            return Instance;
        }

        public string ConfigFileName { get; private set; } = "USGM.config";
        public List<string> SaveFilePrefix { get; private set; } = new List<string>() { "SaveSlotIndex{n}", "ProgressionSlotIndex{n}" };
        public int SaveFileCountStart { get; private set; } = 1;
        public int TotalSlots { get; private set; } = 3;
        public string SavesFolder { get; private set; } = "saves";
        public bool IsComplex { get; private set; } = true;
        public bool Sort { get; private set; } = true;

        private Config() { InitConfig(); }

        // Load config
        private void InitConfig() {
            if(!File.Exists(ConfigFileName)) {
                MessageBox.Show("Missing config file, creating default config - for SW3.\n(might not work with your game)", "Missing config file", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // Create new config
                string CfgContent = SaveFilePrefixToCfg() +
                    nameof(SaveFileCountStart) + "=" + SaveFileCountStart + "\n" +
                    nameof(TotalSlots) + "=" + TotalSlots + "\n" +
                    nameof(IsComplex) + "=" + IsComplex + "\n" +
                    nameof(Sort) + "=" + Sort + "\n";
                File.WriteAllText(ConfigFileName, CfgContent);
            }

            // Parse and convert to variables
            var data = File.ReadAllLines(ConfigFileName);
            for(int i = 0; i < data.Length; i++) {
                var Par = GetParameter(data[i]);
                if(Par.Item1 == nameof(SaveFilePrefix)) {
                    // range loop?
                    if (Par.Item2.Contains("{range:")) {
                        try {
                            string par = Par.Item2.Substring(Par.Item2.IndexOf("{range:") + ("{range:").Length);
                            par = par.Substring(0, par.IndexOf('}'));
                            string[] parts = par.Split('-');
                            if(parts != null && parts.Length == 2) {
                                int min = int.Parse(parts[0]);
                                int max = int.Parse(parts[1]);
                                if(max > min) {
                                    // add range
                                    string prefix = "{range:" + min + "-" + max + "}";
                                    for(int j = min; j <= max; j++) {
                                        string newSaveFilePrefix = Par.Item2.Replace(prefix, ""+ j);
                                        SaveFilePrefix.Add(newSaveFilePrefix);
                                    }
                                }
                            }
                        } catch (Exception) {
                            MessageBox.Show("Failed to parse config file\n" +
                                "Line: " + (i+1) + ", element {range:<min>-<max>}\n\n" +
                                "Will still work but without:\n" +
                                Par.Item1 +"="+ Par.Item2, "USGM - Error parsing config file", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    } else {
                        SaveFilePrefix.Add(Par.Item2);
                    }
                } else if(Par.Item1 == nameof(SaveFileCountStart)) {
                    if(IsNumeric(Par.Item2)) {
                        SaveFileCountStart = int.Parse(Par.Item2);
                    }
                } else if(Par.Item1 == nameof(TotalSlots)) {
                    if(IsNumeric(Par.Item2)) {
                        TotalSlots = Clamp(int.Parse(Par.Item2), 1, 10);
                    }
                } else if(Par.Item1 == nameof(IsComplex)) {
                    IsComplex = bool.Parse(Par.Item2);
                } else if(Par.Item1 == nameof(Sort)) {
                    Sort = bool.Parse(Par.Item2);
                }
            }
        }

        private Tuple<string, string> GetParameter(string data) {
            var split = data.Split('=');
            return split?.Length == 2 ? new Tuple<string, string>(split[0], split[1]) : null;
        }

        public static bool IsNumeric(string value) => float.TryParse(value, out _);

        public static int Clamp(int value, int min, int max) {
            if(value < min) return min;
            if(value > max) return max;
            return value;
        }

        private string SaveFilePrefixToCfg() {
            string str = "";
            for(int i = 0; i < SaveFilePrefix.Count; i++) {
                str += nameof(SaveFilePrefix) + "=" + SaveFilePrefix[i] + "\n";
            }
            return str;
        }
    }
}
