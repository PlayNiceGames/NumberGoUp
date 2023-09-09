using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace GameSave
{
    public class GameSaveService : MonoBehaviour
    {
        private const string EndlessModeSaveKey = "save_endless_mode";

        public GameData LastSave { get; private set; }

        private JsonSerializerSettings _settings;

        private void Awake()
        {
            _settings = GetSerializerSettings();

            LastSave = ReadFromPlayerPrefs();
        }

        public void Save(GameData data)
        {
            string jsonData = JsonConvert.SerializeObject(data, _settings);
            PlayerPrefs.SetString(EndlessModeSaveKey, jsonData);
            PlayerPrefs.Save();

            LastSave = data;
        }

        private GameData ReadFromPlayerPrefs()
        {
            if (!PlayerPrefs.HasKey(EndlessModeSaveKey))
                return null;

            string jsonData = PlayerPrefs.GetString(EndlessModeSaveKey);

            try
            {
                GameData data = JsonConvert.DeserializeObject<GameData>(jsonData, _settings);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                DeleteCurrentSave();

                return null;
            }
        }

        public void DeleteCurrentSave()
        {
            PlayerPrefs.DeleteKey(EndlessModeSaveKey);
            PlayerPrefs.Save();

            LastSave = null;
        }

        private JsonSerializerSettings GetSerializerSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;

            return settings;
        }
    }
}