using DotsTriangle.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Localization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DotsTriangle.UI
{
    public class UIWinnerPanel : MonoBehaviour
    {
        public float timeToWait = 0.5f;
        public GameManager gm;

        public Animator animator;
        public Animator PausePanel;
        public Text title;
        public Text captionTop;
        public Text captionBottom;


        public Text playerVictoriousName;
        public Text playerDefeatedName;


        public Text playerVictoriousRatio;
        public Text playerDefeatedRatio;

        public Image playerVictoriousBar;
        public Image playerDefeatedBar;

        public Image winLogo;
        public Image defeatLogo;

        Mover _defeated;
        Mover _victorious;

        private void OnDisable()
        {
            gm.evtPlayerWins -= WinPlayer;
        }

        private void OnEnable()
        {
            gm.evtPlayerWins += WinPlayer;
        }

        public void OpenPauseMenu()
        {
            if (gm.GameOver == false)
            {
                PausePanel.Play("PanelShow");
                Time.timeScale = 0;
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
           
        }

        private void WinPlayer(PlayerType arg1, float ratio, string reason)
        {
           
            _victorious = gm.GetPlayer(arg1);
            _defeated = gm.GetTheOtherPlayer(_victorious);
            gm.GameOver = true;
            Debug.Log(_victorious.PlayerName);
            UpdateTheWinLosePanel(reason, ratio);

            StartCoroutine(Wait());
        }

        private void UpdateTheWinLosePanel(string reason, float ratio)
        {
            if (gm.IsLocal(_victorious))
            {
                title.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                if (ThemesManager.Instance.CurrentTheme.ThemeType == ThemesManager.Themes.Doodle)
                {
                    title.color = Color.white;
                }
                else
                {
                    title.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                }
                
                playerVictoriousName.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                playerVictoriousName.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                playerDefeatedName.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                playerDefeatedName.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                captionBottom.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                captionBottom.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;

                title.text = LanguageManager.Get("vic");

                playerVictoriousName.text = LanguageManager.Get("you");
                playerDefeatedName.text = _defeated.PlayerName;

                captionBottom.text = LanguageManager.Get("cong");

                winLogo.gameObject.SetActive(true);
                defeatLogo.gameObject.SetActive(false);
                UIMainMenu.Instance.Victories++;
                PlayerPrefs.SetInt("Victories", UIMainMenu.Instance.Victories);
                if (UIMainMenu.Instance.Victories == 50)
                {
                    captionBottom.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                    captionBottom.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                    captionBottom.text = LanguageManager.Get("achiev50");
                }
                else if (UIMainMenu.Instance.Victories == 100)
                {
                    captionBottom.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                    captionBottom.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                    captionBottom.text = LanguageManager.Get("achiev100");
                }
                else if (UIMainMenu.Instance.Victories == 150)
                {
                    captionBottom.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                    captionBottom.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                    captionBottom.text = LanguageManager.Get("achiev150");
                }
            }
            else
            {
                title.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                if (ThemesManager.Instance.CurrentTheme.ThemeType == ThemesManager.Themes.Doodle)
                {
                    title.color = Color.white;
                }
                else
                {
                    title.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                }
                playerVictoriousName.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                playerVictoriousName.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                playerDefeatedName.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                playerDefeatedName.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                captionBottom.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                captionBottom.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;

                title.text = LanguageManager.Get("loss");

                playerVictoriousName.text = _victorious.PlayerName;
                playerDefeatedName.text = LanguageManager.Get("you");

                captionBottom.text = LanguageManager.Get("betterluck");


                winLogo.gameObject.SetActive(false);
                defeatLogo.gameObject.SetActive(true);
            }
            captionTop.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
            captionTop.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
            captionTop.text = reason;

            float victoriousFill = gm.GetFill(_victorious);
            float defeatedFill = gm.GetFill(_defeated);

            playerVictoriousRatio.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
            playerVictoriousRatio.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
            playerVictoriousRatio.text = (int)(victoriousFill * 100) + "%";
            playerDefeatedRatio.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
            playerDefeatedRatio.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
            playerDefeatedRatio.text = (int)(defeatedFill * 100) + "%";


            playerVictoriousBar.fillAmount = victoriousFill;
            playerDefeatedBar.fillAmount = defeatedFill;
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(timeToWait);
            animator.Play("PanelShow");
        }
    }

}
