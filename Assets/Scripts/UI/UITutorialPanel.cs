using DotsTriangle.Core;
using DotsTriangle.Data;
using DotsTriangle.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotsTriangle.UI
{
    public class UITutorialPanel : MonoBehaviour
    {
        public float timeToWait = 0.5f;
        public GameManager gm;
        public GoToScene goToGameplay;
        public GoToScene goToMainMenu;
        public ISaveData data;


        public Animator animator;

        private void OnDisable()
        {
            gm.evtPlayerWins -= WinPlayer;
        }

        private void OnEnable()
        {
            gm.evtPlayerWins += WinPlayer;
        }

        private void WinPlayer(PlayerType arg1, float ratio, string reason)
        {
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(timeToWait);
            animator.Play("PanelShow");
        }

        public void GoToSinglePlayer()
        {
            data.SetKey(Constants.IS_SINGLEPLAYER, 1);
            data.SetKey(Constants.SHOW_SINGLEPLAYER_PANEL_FORCED, 1);
            goToMainMenu.Go();

        }

        public void GoToMultiplayer()
        {
            data.SetKey(Constants.IS_SINGLEPLAYER, 0);
            data.SetKey(Constants.SHOW_MULTIPLAYER_PANEL_FORCED, 1);
            goToMainMenu.Go();
        }
    }

}
