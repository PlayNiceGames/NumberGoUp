using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace GameSave
{
    public class GameSaveService : MonoBehaviour
    {
        private const string CurrentSaveKey = "save_endless_mode";
        private const string LastSavesKey = "last_saves_endless_mode";

        [SerializeField] private int _maxLastSavesCount;

        public GameData CurrentSave { get; private set; }

        private LinkedList<GameData> _lastSaves;
        private JsonSerializerSettings _settings;

        private void Awake()
        {
            _settings = GetSerializerSettings();

            CurrentSave = ReadObjectFromPlayerPrefs<GameData>(CurrentSaveKey);
            _lastSaves = ReadObjectFromPlayerPrefs<LinkedList<GameData>>(LastSavesKey) ?? new LinkedList<GameData>();
        }

        private void Save()
        {
            string currentSaveData = JsonConvert.SerializeObject(CurrentSave, _settings);
            PlayerPrefs.SetString(CurrentSaveKey, currentSaveData);

            string lastSavesData = JsonConvert.SerializeObject(_lastSaves, _settings);
            PlayerPrefs.SetString(LastSavesKey, lastSavesData);

            PlayerPrefs.Save();
        }

        public void PushSave(GameData data)
        {
            AddToLastSaves(CurrentSave);

            CurrentSave = data;

            Save();
        }

        private void AddToLastSaves(GameData data)
        {
            _lastSaves.AddFirst(data);

            while (_lastSaves.Count > _maxLastSavesCount)
                _lastSaves.RemoveLast();
        }

        private T ReadObjectFromPlayerPrefs<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return default;

            string jsonData = PlayerPrefs.GetString(key);

            try
            {
                T data = JsonConvert.DeserializeObject<T>(jsonData, _settings);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                DeleteCurrentSave();

                return default;
            }
        }

        public GameData PopLastSave()
        {
            GameData save = _lastSaves.First?.Value;

            if (save == null)
                return null;

            _lastSaves.RemoveFirst();

            CurrentSave = save;

            Save();

            return save;
        }

        public int LastSavesCount()
        {
            return _lastSaves.Count;
        }

        public void DeleteCurrentSave()
        {
            PlayerPrefs.DeleteKey(CurrentSaveKey);
            PlayerPrefs.DeleteKey(LastSavesKey);
            PlayerPrefs.Save();

            CurrentSave = null;
            _lastSaves = new LinkedList<GameData>();
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