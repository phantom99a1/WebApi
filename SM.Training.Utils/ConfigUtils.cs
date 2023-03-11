using Microsoft.Extensions.Configuration;
using SM.Training.SharedComponent.Constants;
using SoftMart.Core.Dao;
using SoftMart.Kernel.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SM.Training.Utils
{
    public class ConfigUtils
    {
        private static Configuration ConfigurationManager { get; set; }

        public static void InitConfiguration(IConfiguration configuration)
        {
            ConfigurationManager = new Configuration()
            {
                ConnectionStrings = new List<Configuration.ConnectionStringInfo>(),
                AppSettings = new List<Configuration.AppSettingInfo>()
            };

            // Build chuỗi kết nối
            var connSettings = configuration.GetSection("ConnectionSettings");
            foreach (var setting in connSettings.GetChildren())
            {
                ConfigurationManager.ConnectionStrings.Add(new Configuration.ConnectionStringInfo()
                {
                    Name = setting.Key,
                    ConnectionString = setting["connectionString"],
                    ProviderName = setting["providerName"],
                    Port = setting["port"]
                });
            }

            // Build các setting khác
            var appSettings = configuration.GetSection("AppSettings");
            foreach (var setting in appSettings.GetChildren())
            {
                ChangeSetting(setting.Key, setting.Value);
            }
        }

        /// <summary>
        /// Xác định file Config từ các nguồn
        /// 1. Từ biến param env
        /// 2. Từ file config map trong folder \configuration\appsettings.json
        /// 3. Từ file \appsettings.json
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetConfigFilePath(string[] args)
        {
            // Hướng dẫn ConfigMap trên nền K8S: https://medium.com/@fbeltrao/automatically-reload-configuration-changes-based-on-kubernetes-config-maps-in-a-net-d956f8c8399a

            // File config mặc định
            string configurationFile = string.Empty;

            // File config truyền vào thông qua configMap
            if (args != null && args.Length > 0)
            {
                configurationFile = args[0].Trim();

                ConfigUtils.AppendStartLog($"Có param truyền vào, lấy file config path từ param, File path: {configurationFile}");

                if (File.Exists(configurationFile) == false)
                {
                    ConfigUtils.AppendStartLog("Configuration file passed from start arguments is not found: " + configurationFile);
                    throw new Exception("Configuration file passed from start arguments is not found: " + configurationFile);
                }
            }
            else // Tìm trong thư mục Configuration xem có không
            {
                configurationFile = Path.Combine("configuration", "appsettings.json");

                // Nếu không có thì dùng file trong thư mục gốc
                if (File.Exists(configurationFile) == false)
                {
                    ConfigUtils.AppendStartLog("Không có file config lấy từ configmap");

                    configurationFile = Path.Combine("appsettings.json");

                    ConfigUtils.AppendStartLog("Lấy file config " + configurationFile);

                    // Lỗi Trên WinService không thấy file
                    // Nếu vẫn không có thì trả ra lỗi
                    if (File.Exists(configurationFile) == false)
                    {
                        ConfigUtils.AppendStartLog("Configuration file is not found at all");
                        //throw new Exception("Configuration file is not found at all");
                    }
                }
            }

            ConfigUtils.AppendStartLog($"ConfigFile Map: {configurationFile}");

            return configurationFile;
        }

        public static void AppendStartLog(string log, Exception ex = null)
        {
            var fileName = $"FlexCollectionLogs.{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            var path = Path.Combine("Logs", fileName);

            Directory.CreateDirectory("Logs");

            File.AppendAllText(path, "\r\n");
            File.AppendAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffff")}: {log}");

            if (ex != null)
            {
                File.AppendAllText(path, "\r\n");
                File.AppendAllText(path, ex.StackTrace);
            }
        }

        /// <summary>
        /// Cật nhật lại setting theo giá trị cấu hình trong database
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="settingValue"></param>
        public static void ChangeSetting(string settingKey, string settingValue)
        {
            if (ConfigurationManager == null || ConfigurationManager.AppSettings == null)
            {
                return;
            }

            // Lấy setting hiện có trong file config
            var setting = ConfigurationManager.AppSettings.Find(en => en.Key == settingKey);

            // Nếu chưa có thì tạo mới, có rồi thì bỏ qua để đảm bảo setting trong file được ưu tiên hơn
            if (setting == null)
            {
                ConfigurationManager.AppSettings.Add(new Configuration.AppSettingInfo() { Key = settingKey, Value = settingValue });
            }
        }

        public static SqlPagingMode PagingMode
        {
            get
            {
                if (_PagingMode == null)
                {
                    _PagingMode = GetConfigInt("PagingMode");

                    if (Enum.IsDefined(typeof(SqlPagingMode), _PagingMode) == false)
                    {
                        throw new Exception("PagingMode is invalid: Using 1 for RowNumber, 2 for Offset");
                    }
                }
                return (SqlPagingMode)_PagingMode;
            }
        }
        private static int? _PagingMode;

        #region Connectionstring
        private static string _ApplicationDataConnection = null;
        public static string ApplicationDataConnection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ApplicationDataConnection))
                {
                    _ApplicationDataConnection = ConfigurationManager.ConnectionStrings
                                                 .FirstOrDefault(c => c.Name == SMX.ConnectionString.ApplicationData).ConnectionString;

                    //try
                    //{
                    //    _ApplicationDataConnection = EncryptionUtils.Instance.Decrypt(_ApplicationDataConnection);
                    //}
                    //catch { }
                }

                return _ApplicationDataConnection;
            }
        }

        public static Configuration.ConnectionStringInfo GetConnectionString(string connKey)
        {
            if (string.IsNullOrWhiteSpace(connKey) == false)
            {
                var conn = ConfigurationManager.ConnectionStrings.FirstOrDefault(c => c.Name == connKey);
                if (conn != null)
                {
                    return conn;
                }
                throw new Exception("Không tồn tại ConnectionString: " + connKey);
            }
            else
            {
                throw new Exception("ConnectionString key empty.");
            }
        }

        #endregion Connectionstring

        public static string GetConfig(string key)
        {
            var item = ConfigurationManager.AppSettings.FirstOrDefault(c => c.Key == key);
            if (item == null || item.Value == null)
                return null;

            string value = item.Value.Trim();
            //try
            //{
            //    value = EncryptionUtils.Instance.Decrypt(value);
            //}
            //catch { }

            return value;
        }

        public static int GetConfigInt(string key)
        {
            string value = GetConfig(key);
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                throw new SMXException(string.Format("Giá trị cấu hình của key {0} không đúng kiểu Int", key));
            }
        }

        #region Configuration
        [XmlRoot("configuration")]
        public class Configuration
        {
            [XmlArray("connectionStrings")]
            [XmlArrayItem("add")]
            public List<ConnectionStringInfo> ConnectionStrings { get; set; }

            [XmlArray("appSettings")]
            [XmlArrayItem("add")]
            public List<AppSettingInfo> AppSettings { get; set; }

            #region Classes
            public class ConnectionStringInfo
            {
                [XmlAttribute("name")]
                public string Name { get; set; }

                [XmlAttribute("connectionString")]
                public string ConnectionString { get; set; }

                [XmlAttribute("providerName")]
                public string ProviderName { get; set; }

                [XmlAttribute("port")]
                public string Port { get; set; }
            }

            public class AppSettingInfo
            {
                [XmlAttribute("key")]
                public string Key { get; set; }

                [XmlAttribute("value")]
                public string Value { get; set; }
            }
            #endregion
        }
        #endregion
    }
}