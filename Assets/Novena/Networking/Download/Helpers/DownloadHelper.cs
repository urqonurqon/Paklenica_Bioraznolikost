using System.IO;
using UnityEngine;

namespace Novena.Networking.Download.Helpers
{
    public static class DownloadHelper
    {
        /// <summary>
        /// Check if data.json exist in folder
        /// </summary>
        /// <param name="downloadCode"></param>
        /// <returns></returns>
        public static bool CheckIfGuideExist(string downloadCode)
        {
            bool output = false;

            string persistentPath = Application.persistentDataPath;
            string filePath = Path.Combine(persistentPath, downloadCode, "data.json");

            if (File.Exists(filePath))
            {
                output = true;
            }
            
            return output;
        }
    }
}
