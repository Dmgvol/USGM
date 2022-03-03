using System;
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
        public string SaveFilePrefix { get; private set; } = "SaveSlotIndex{n}";
        public int SaveFileCountStart { get; private set; } = 1;
        public int TotalSlots { get; private set; } = 3;
        public string SavesFolder { get; private set; } = "saves";

        private Config() {InitConfig();}

        // load config
        private void InitConfig() {
            if(!File.Exists(ConfigFileName)) {
                MessageBox.Show("Missing config file, creating default config - for SW3.\n(might not work with your game)", "Missing config file", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                // create new config
                string CfgContent = nameof(SaveFilePrefix) + "=" + SaveFilePrefix + "\n" +
                    nameof(SaveFileCountStart) + "=" + SaveFileCountStart + "\n" +
                    nameof(TotalSlots) + "=" + TotalSlots + "\n";
                File.WriteAllText(ConfigFileName, CfgContent);
            }

            var data = File.ReadAllLines(ConfigFileName);
            for(int i = 0; i < data.Length; i++) {
                var Par = GetParameter(data[i]);
                if(Par.Item1 == nameof(SaveFilePrefix)) {
                    SaveFilePrefix = Par.Item2;
                } else if(Par.Item1 == nameof(SaveFileCountStart)) {
                    if(IsNumeric(Par.Item2)) {
                        SaveFileCountStart = int.Parse(Par.Item2);
                    }
                } else if(Par.Item1 == nameof(TotalSlots)) {
                    if(IsNumeric(Par.Item2)) {
                        TotalSlots = Clamp(int.Parse(Par.Item2), 1, 10);
                    }
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
    }
}
