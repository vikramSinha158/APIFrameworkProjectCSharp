using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace R1.Automation.API.Core.Utilities
{
    public class Utility
    {
        

        /// <summary>This is used for load appsettings.json file</summary>
        public static IConfigurationRoot ConfigUrl
        {
            get
            {
                var config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();
                return config;
            }
        }

        /// <summary>This method is used for Get Folder Path</summary>
        /// <param name="appFolderName"></param>
        /// <returns>This returns Path of a folder</returns>
        public string GetFolderPath(string appFolderName)
        {
            var folderName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(folderName.Substring(0, folderName.LastIndexOf("\\bin")), appFolderName + "\\");
        }

        /// <summary>This Method is used for read a json file</summary>
        /// <param name="testDataPath"></param>
        /// <returns>This returns string</returns>
        public string LoadJson(string testDataPath)
        {
            using (StreamReader r = new StreamReader(testDataPath))
            {
                var json = r.ReadToEnd();
                return JObject.Parse(json).ToString();

            }
        }

        /// <summary>This method is used for get data from test data json file</summary>
        /// <param name="key"></param>
        /// <returns>This returns string value from test data json file</returns>
        public string GetTestData(String key)
        {
            JObject obs = JObject.Parse(LoadJson(GetFolderPath(ConfigUrl["AppSettings:TestDataFolderName"]) + ConfigUrl["AppSettings:TestDataFileName"]));
            return obs[key].ToString();
        }

        /// <summary>This method is used for get data from test query json file</summary>
        /// <param name="key"></param>
        /// <returns>This returns string value from test query json file</returns>
        public string GetQueryData(String key)
        {
            JObject obs = JObject.Parse(LoadJson(GetFolderPath(ConfigUrl["AppSettings:TestQueryFolderName"]) + ConfigUrl["AppSettings:TestQueryFileName"]));
            return obs[key].ToString();
        }

        /// <summary>This method is used for read a json file and replace some words</summary>
        /// <param name="testDataPath"></param>
        /// <param name="dict"></param>
        /// <returns>This returns a string</returns>
        public string LoadJsonReplace(string testDataPath, Dictionary<string, string> dict)
        {
            string jsonString = null;
            using (StreamReader r = new StreamReader(testDataPath))
            {
                var json = r.ReadToEnd();
                jsonString= JObject.Parse(json).ToString();

            }

            foreach (KeyValuePair<string, string> ele in dict)
            {
                jsonString = jsonString.Replace(ele.Key, ele.Value);
            }

            return jsonString;
        }
    }
}
