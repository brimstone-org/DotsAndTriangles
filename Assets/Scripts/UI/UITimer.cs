using DotsTriangle.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotsTriangle.UI
{
    public class UITimer : MonoBehaviour
    {
        public Text text;
        public Ticker ticker;
        // Use this for initialization


        private void OnEnable()
        {
            //ticker.evtNewTick += UpdateTimer;
            text.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
        }


        private void OnDisable()
        {
            //ticker.evtNewTick -= UpdateTimer;
        }

        private void Update()
        {
            float arg2 = ticker.CurrentTick * ticker.tickDelta.Value; 
            string ss = Convert.ToInt32(arg2 % 60).ToString("00");
            string mm = (Math.Floor(arg2 / 60) % 60).ToString("00");
            text.text = mm + ":" + ss;
        }


        private void UpdateTimer(int arg1, float arg2)
        {
            string ss = Convert.ToInt32(arg2 % 60).ToString("00");
            string mm = (Math.Floor(arg2 / 60) % 60).ToString("00");
            text.text = mm + ":" + ss;
        }
    }
}

