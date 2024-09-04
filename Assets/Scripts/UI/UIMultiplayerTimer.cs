using DotsTriangle.Core;
using DotsTriangle.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotsTriangle.UI
{
    public class UIMultiplayerTimer : MonoBehaviour
    {
        public Text text;
        public Ticker ticker;
        public Image bar;
        public ColorType playerAColor;
        public ColorType playerBColor;
        public float lastSecondsRatio;
        public float blinkTime;
        public Color blinkColor;
        public Image panel;

        IEnumerator _blink;
        bool vibratedDone;

        private void OnEnable()
        {
            ticker.evtPlayerTimeTick += UpdateTimer;
        }


        private void OnDisable()
        {
            ticker.evtPlayerTimeTick -= UpdateTimer;
        }

        private void UpdateTimer(int arg1, float arg2, float ratio)
        {

            if (GameManager.Single.IsLocal(GameManager.Single.ActiveMover))
                bar.color = playerAColor.Value;
            else
                bar.color = playerBColor.Value;

            if(1 - ratio < lastSecondsRatio)
            {
                if(_blink == null)
                {
                    _blink = Blink();
                    StartCoroutine(_blink);
                }
            }
            else
            {
                vibratedDone = false;
            }

            if (arg2 < 0)
                arg2 = 0;
            string ss = Convert.ToInt32(arg2 % 60).ToString("00");
            string mm = (Math.Floor(arg2 / 60) % 60).ToString("00");
            text.text = mm + ":" + ss;
            bar.fillAmount = Mathf.Max(0, Mathf.Min(1, 1 - ratio));
        }

        private IEnumerator Blink()
        {
            if(!vibratedDone && GameManager.Single.IsLocal(GameManager.Single.ActiveMover))
            {
                Debug.Log("Vibrate");
                Handheld.Vibrate();
                vibratedDone = true;
            }

            float timePassed = 0;
            Color initial = panel.color;
            while(timePassed < blinkTime)
            {
                timePassed += Time.deltaTime;
                float t = timePassed / blinkTime;
                panel.color = Color.Lerp(blinkColor, initial, t);
                yield return new WaitForEndOfFrame();

            }

            panel.color = initial;

            _blink = null;
        }
    }
}

