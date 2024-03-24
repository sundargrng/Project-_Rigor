using System.IO;
using UnityEngine;



namespace SaveLoadSystem
{
    public static class SaveGameManager
    {
        public static SaveData CurrentSaveData = new SaveData();

        public const string SaveDirectory = "/SaveData/";

        public const string FileName = "SaveGame.don";

        public static bool SaveGame()
        {
            var dir = Application.persistentDataPath + SaveDirectory;

            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonUtility.ToJson(CurrentSaveData, true);
            File.WriteAllText(dir + FileName, json);

            GUIUtility.systemCopyBuffer = dir;

            return true;
        }

        public static void LoadGame()
        {
            string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

            SaveData tempData = new SaveData();

            if(File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                tempData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                Debug.LogError("SaveData does not exists");
            }

            CurrentSaveData = tempData;
        }
    }
}
