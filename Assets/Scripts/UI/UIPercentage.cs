using DotsTriangle.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DotsTriangle.Core;

namespace DotsTriangle.UI
{
    public class UIPercentage : MonoBehaviour
    {

        //public FloatType fill;
        public bool playerA;
        public ColorType color;
        public Image circle;
        public Text text;
        public Text playerName;
        public UpdaterType uiUpdater;

        private void Start()
        {
            UpdateUI();
        }

        private void OnEnable()
        {
            uiUpdater.evtFired += UpdateUI;
        }

        private void UpdateUI()
        {
           // circle.color = color.Value;
            text.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
            text.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
            playerName.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
            //playerName.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
            if (playerName != null)
                playerName.color = color.Value;

            playerName.text = playerA ? GameManager.Single.playerA.PlayerName : GameManager.Single.playerB.PlayerName;

            if (playerName.text == GameManager.Single.playerA.PlayerName)
            {
                circle.overrideSprite = ThemesManager.Instance.CurrentTheme.PlayerOne;
                playerName.color = ThemesManager.Instance.CurrentTheme.PlayerOneColor; 
            }
            else
            {
                circle.overrideSprite = ThemesManager.Instance.CurrentTheme.PlayerTwo;
                playerName.color = ThemesManager.Instance.CurrentTheme.PlayerTwoColor;
            }
            

            float fill = playerA ? GameManager.Single.playerA.Fill() : GameManager.Single.playerB.Fill();
            text.text = (int)(fill * 100) + "%";
        }

        private void OnDisable()
        {
            uiUpdater.evtFired -= UpdateUI;
        }
    }
}

