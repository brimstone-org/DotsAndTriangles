using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Core
{
    public class MusicPlayer : MonoBehaviour
    {
        public List<Utils.Sound> sounds;

        // Use this for initialization
        void Start()
        {
            Utils.SoundManager.Instance.PlayBackgroundMusic(sounds[UnityEngine.Random.Range(0, sounds.Count)].clip, Vector3.zero);
        }

        private void OnLevelWasLoaded(int level)
        {
            Start();
        }
    }

}
