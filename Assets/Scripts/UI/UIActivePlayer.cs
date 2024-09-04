using DotsTriangle.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DotsTriangle.UI
{
    public class UIActivePlayer : MonoBehaviour
    {
        public GameManager gm;
        public PlayerType player;
        public Image img;
        public float timeToFade = 0.4f;
        //Mover _mover;
        IEnumerator _fadeIn;
        bool _isActive;

        private void OnEnable()
        {
            GameManager.Single.evtPlayerChanged += PlayerChanged;
        }

        private void PlayerChanged(PlayerType arg1, int arg2)
        {
            //Debug.Log("!!!!!!!!!!Changing the player " + (arg1.ToString()));
            if (arg1 == player)
            {
                if (!_isActive)
                {
                    StopAllCoroutines();
                    StartCoroutine(Fade(1));
                    _isActive = true;
                }

            }
            else
            {
                if (_isActive)
                {
                    StopAllCoroutines();
                    StartCoroutine(Fade(0));
                    _isActive = false;
                }
            }
        }

        private void OnDisable()
        {
            if (GameManager.Single != null)
                GameManager.Single.evtPlayerChanged -= PlayerChanged;
        }

        private void Start()
        {
            /*
            if (gm.playerA.PlayerType == player)
                _mover = gm.playerA;
            else
                _mover = gm.playerB;
            */
            img.color = new Color(img.color.r, img.color.g, img.color.b, GameManager.Single.ActiveMover.PlayerType == player ? 1 : 0);
            _isActive = GameManager.Single.ActiveMover.PlayerType == player;
        }

        /*
        void Update()
        {
            if (gm.playerA.PlayerType == player)
                _mover = gm.playerA;
            else
                _mover = gm.playerB;

            if (_mover.IsActive)
            {
                if (!_isActive)
                {
                    StopAllCoroutines();
                    StartCoroutine(Fade(1));
                    _isActive = true;
                }
               
            }
            else
            {
                if (_isActive)
                {
                    StopAllCoroutines();
                    StartCoroutine(Fade(0));
                    _isActive = false;
                }
            }
        }
        */
        private IEnumerator Fade(int v)
        {
            float timePassed = 0;
            float aInit = img.color.a;
            while(timePassed < timeToFade)
            {
                timePassed += Time.deltaTime;
                img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(aInit, v, timePassed / timeToFade));

                yield return new WaitForEndOfFrame();
            }
        }
    }

}
