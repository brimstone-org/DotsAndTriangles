using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Utils
{
    public class PlaySound : MonoBehaviour
    {
        public Sound soundSO;
        public AudioClip sound;

        public void Play()
        {
            if (soundSO != null)
                SoundManager.Instance.PlaySound(soundSO.clip, Vector3.zero);
            else
                SoundManager.Instance.PlaySound(sound, Vector3.zero);
        }
    }
}

