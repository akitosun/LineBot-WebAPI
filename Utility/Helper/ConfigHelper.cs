using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Configuration;

namespace Utility.Helper
{
    /// <summary>
    /// 應用程式組態設定存取Helper類別
    /// </summary>
    public class ConfigHelper
    {
        private static readonly Lazy<ConfigHelper> _lazyUniqueConfigHelper =
            new Lazy<ConfigHelper>(() => new ConfigHelper());

        /// <summary>
        /// 預設建構子
        /// </summary>
        private ConfigHelper()
        {
        }

        /// <summary>
        /// 取得實例
        /// </summary>
        /// <returns></returns>
        public static ConfigHelper GetInstance()
        {
            return _lazyUniqueConfigHelper.Value;
        }

        /// <summary>
        /// 讀取組態檔應用程式設定項目值
        /// </summary>
        /// <param name="key">應用程式設定項目鍵值</param>
        /// <returns>應用程式設定項目值</returns>
        public string GetAppSettingValue(string key)
        {
            string value = String.Empty;
            bool hasKey = false;

            List<Configuration> configList = OpenApConfiguration();
            if (configList != null && configList.Count >= 0)
            {
                foreach (var config in configList)
                {
                    if (config != null
                        && config.AppSettings.Settings[key] != null)
                    {
                        value = config.AppSettings.Settings[key].Value;
                        hasKey = true;
                        break;
                    }
                }
            }
            else
            {
                Configuration config = OpenWebConfiguration();

                if (config != null
                    && config.AppSettings.Settings[key] != null)
                {
                    value = config.AppSettings.Settings[key].Value;
                    hasKey = true;
                }
            }

            if (!hasKey)
            {
                throw new ApplicationException(String.Format("Can't read config file appsetting key {0}!", key));
            }

            return value;
        }

        /// <summary>
        /// 讀取組態檔資料庫連線字串設定項目值
        /// </summary>
        /// <param name="key">資料庫連線字串設定項目鍵值</param>
        /// <returns>資料庫連線字串設定項目值</returns>
        /// <remarks>自動判斷設定值是否使用AES加密，若有加密則先解密後回傳</remarks>
        public string GetConnectionStringValue(string key)
        {
            string value = String.Empty;
            bool hasKey = false;

            List<Configuration> configList = OpenApConfiguration();
            if (configList != null && configList.Count >= 0)
            {
                foreach (var config in configList)
                {
                    if (config != null
                        && !String.IsNullOrEmpty(config.ConnectionStrings.ConnectionStrings[key].ConnectionString))
                    {
                        value = config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
                        hasKey = true;
                        break;
                    }
                }
            }
            else
            {
                Configuration config = OpenWebConfiguration();

                if (config != null
                    && !String.IsNullOrEmpty(config.ConnectionStrings.ConnectionStrings[key].ConnectionString))
                {
                    value = config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
                    hasKey = true;
                }
            }

            if (!hasKey)
            {
                throw new ApplicationException(String.Format("Can't read config file appsetting key {0}!", key));
            }

            return value;
        }

        /// <summary>
        /// 開啟目前執行程式目錄下Ap組態檔清單
        /// </summary>
        /// <returns>組態檔參考</returns>
        private List<Configuration> OpenApConfiguration()
        {
            List<Configuration> configList = new List<Configuration>();
            string configPath = String.Empty;
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;

            var configFiles = Directory.EnumerateFiles(basePath, "*.config");

            foreach (string cffile in configFiles)
            {
                if (File.Exists(Path.Combine(basePath, cffile)))
                {
                    configPath = Path.Combine(basePath, cffile);

                    try
                    {
                        ExeConfigurationFileMap cfgFileMap = new ExeConfigurationFileMap();
                        cfgFileMap.ExeConfigFilename = configPath;
                        var config = ConfigurationManager.OpenMappedExeConfiguration(cfgFileMap, ConfigurationUserLevel.None);

                        if (config != null)
                        {
                            configList.Add(config);
                        }
                    }
                    catch (Exception)
                    {
                        // ignore exception
                    }
                }
            }

            return configList;
        }

        /// <summary>
        /// 開啟目前執行網站Web組態檔
        /// </summary>
        /// <returns>組態檔參考</returns>
        private Configuration OpenWebConfiguration()
        {
            Configuration config = null;
            string configPath = String.Empty;
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] configFiles = { @"Web.config" };

            foreach (string cffile in configFiles)
            {
                if (File.Exists(Path.Combine(basePath, cffile)))
                {
                    configPath = Path.Combine(basePath, cffile);

                    try
                    {
                        FileInfo configFile = new FileInfo(configPath);
                        WebConfigurationFileMap wcfm = new WebConfigurationFileMap();
                        wcfm.VirtualDirectories.Add("/", new VirtualDirectoryMapping(configFile.DirectoryName, true, configFile.Name));
                        config = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/", "web");

                        if (config != null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(String.Format("Read config file error!! path={0}, exception={1}", configPath, ex.Message));
                    }
                }
            }

            if (config == null)
            {
                config = WebConfigurationManager.OpenWebConfiguration("/");
            }

            return config;
        }
    }
}