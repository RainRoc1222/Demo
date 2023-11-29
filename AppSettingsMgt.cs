using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace MyTcpServerAndClient
{
    public class AppSettingsMgt
    {
        public const string SettingFile = "AppData\\AppSettings.Json";

        public static AppSettings AppSettings { get; set; }

        static AppSettingsMgt()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                var setting = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                AppSettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(SettingFile), setting);
            }
            catch (Exception ex)
            {
                AppSettings = new AppSettings()
                {
                    IPAddress = "127.0.0.1",
                    Port = 6101
                };
                Save();
            }
        }

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(AppSettings, Formatting.Indented);
            File.WriteAllText(SettingFile, json);
        }
    }
}
