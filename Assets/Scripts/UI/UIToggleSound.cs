using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotsTriangle.UI
{
    public class UIToggleSound : MonoBehaviour
    {
        public Utils.Sound tap;
        public Image soundImage;
        public Sprite soundOn;
        public Sprite soundOff;

        bool _isOff;
        // Use this for initialization
        void Start()
        {
            //PlayerPrefs.DeleteAll();
            if (Utils.SoundManager.Instance.SfxOn)
            {
                soundImage.overrideSprite = ThemesManager.Instance.CurrentTheme.SoundOnIcon;
                _isOff = false;
            }
            else
            {
                soundImage.overrideSprite = ThemesManager.Instance.CurrentTheme.SoundOffIcon;
                _isOff = true;
            }
        }

        public void Toggle()
        {
            
            if (_isOff)
            {
                Utils.SoundManager.Instance.MusicOn = true;
                Utils.SoundManager.Instance.SfxOn = true;

                Utils.SoundManager.Instance.PlaySound(tap.clip, Vector3.zero);
            }

            else
            {
                Utils.SoundManager.Instance.MusicOn = false;
                Utils.SoundManager.Instance.SfxOn = false;
            }

            _isOff = !_isOff;


            Start();
        }
    }

}
