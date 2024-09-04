using DotsTriangle.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DotsTriangle.UI
{
    public class UIOpponentsTurn : MonoBehaviour
    {

        public CanvasGroup opponentPanel;
        public CanvasGroup playerPanel;
        public float timeAnim = 0.3f;

        public bool _isShownOpponent;

        IEnumerator _opponentCO;
        IEnumerator _playerCO;


        public void Start()
        {
            opponentPanel.alpha = 0;
            opponentPanel.gameObject.SetActive(false);

            playerPanel.alpha = 0;
            playerPanel.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameManager.Single.evtPlayerChanged += Change;
        }

        private void Change(PlayerType arg1, int arg2)
        {
            if (!GameManager.Single.IsLocal(GameManager.Single.ActiveMover))
            {
                if (!_isShownOpponent)
                {
                    ShowPanel(_opponentCO, opponentPanel);
                    HidePanel(_playerCO, playerPanel);
                    _isShownOpponent = true;

                }
            }
            else
            {
                if (_isShownOpponent)
                {
                    HidePanel(_opponentCO, opponentPanel);
                    ShowPanel(_playerCO, playerPanel);
                    _isShownOpponent = false;
                }
            }
        }

        void ShowPanel(IEnumerator co, CanvasGroup g)
        {
            if (co != null)
                StopCoroutine(co);
            co = Show(g);

            StartCoroutine(co);
        }

        void HidePanel(IEnumerator co, CanvasGroup g)
        {
            if (co != null)
                StopCoroutine(co);
            co = Hide(g);

            StartCoroutine(co);
        }

        IEnumerator Show(CanvasGroup p)
        {
            float initialA = p.alpha;
            p.gameObject.SetActive(true);

            float timePassed = 0;
            while (timePassed < timeAnim)
            {
                timePassed += Time.deltaTime;
                p.alpha = Mathf.Lerp(initialA, 1, timePassed / timeAnim);
                yield return new WaitForEndOfFrame();
            }

            opponentPanel.alpha = 1;
        }

        IEnumerator Hide(CanvasGroup p)
        {
            float initialA = p.alpha;
            p.gameObject.SetActive(true);

            float timePassed = 0;
            while (timePassed < timeAnim)
            {
                timePassed += Time.deltaTime;
                p.alpha = Mathf.Lerp(initialA, 0, timePassed / timeAnim);
                yield return new WaitForEndOfFrame();
            }

            p.alpha = 0;
            p.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            GameManager.Single.evtPlayerChanged -= Change;
        }
    }
}
