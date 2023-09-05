using System;
using UnityEngine;

namespace GameSave
{
    public class GameSaveService : MonoBehaviour
    {
        private const string EndlessModeSaveKey = "save_endless_mode";

        public GameData LastSave { get; private set; }

        private void Awake()
        {
            LastSave = ReadFromPlayerPrefs();
        }

        public void Save(GameData data)
        {
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(EndlessModeSaveKey, jsonData);

            LastSave = data;
        }

        private GameData ReadFromPlayerPrefs()
        {
            if (!PlayerPrefs.HasKey(EndlessModeSaveKey))
                return null;

            string jsonData = PlayerPrefs.GetString(EndlessModeSaveKey);

            try
            {
                GameData data = JsonUtility.FromJson<GameData>(jsonData);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}