using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JaEditor
{
    internal static class Program
    {
        public static SaveFile loadedSave;

        public static string savePath = @"C:\Users\gdlea\AppData\LocalLow\MinskWorks\Jalopy\Saves\" + "Save.sfminsk";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoadData();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new JaEditor());
        }

        public static void LoadData()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            if (File.Exists(savePath))
            {
                using (FileStream fileStream = File.Open(savePath, FileMode.Open))
                {
                    loadedSave = (SaveFile)binaryFormatter.Deserialize(fileStream);
                }
            }
            else
            {
                //loadedSave = new SaveFile();
            }
        }
    }
}
