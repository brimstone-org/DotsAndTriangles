using DotsTriangle.Core;
using DotsTriangle.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotsTriangle.UI
{
    public class UIBarFill : MonoBehaviour
    {

        public bool playerA;
        public ColorType color;
        public Image mask;
        public List<MaskableGraphic> images;
        public UpdaterType uiUpdater;
        private Sprite _playerFill;

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
            if (playerA)
            {
                images[0].GetComponent<Image>().overrideSprite = ThemesManager.Instance.CurrentTheme.PlayerOneProgressBar;
            }
            else
            {
                images[0].GetComponent<Image>().overrideSprite = ThemesManager.Instance.CurrentTheme.PlayerTwoProgressBar;
            }
            //images.ForEach( y => y.color = color.Value);
            mask.color = color.Value;
            float fill = playerA ? GameManager.Single.playerA.Fill() : GameManager.Single.playerB.Fill();
            mask.fillAmount = fill;
        }

        private void OnDisable()
        {
           uiUpdater.evtFired -= UpdateUI;
        }
    }
}

