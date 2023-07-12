using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace GameAudio
{
    public class SoundsDatabase : ScriptableObject
    {
        [field: SerializeField] public AudioClip Music { get; private set; }
        [field: SerializeField] public AudioClip Click { get; private set; }
        [field: SerializeField] public AudioClip Eraser { get; private set; }
        [field: SerializeField] public AudioClip GameOver { get; private set; }
        [field: SerializeField] public AudioClip BoardResize { get; private set; }

        [SerializeField] private SerializedDictionary<int, AudioClip> _merge;

        public AudioClip GetMergeSound(int newValue)
        {
            AudioClip clip = _merge.Values.FirstOrDefault();

            foreach (KeyValuePair<int, AudioClip> pair in _merge)
            {
                if (pair.Key <= newValue)
                    clip = pair.Value;
            }


            return clip;
        }
    }
}