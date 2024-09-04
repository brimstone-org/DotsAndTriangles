using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;
using System;
using UnityEngine.UI;

namespace DotsTriangle.UI
{
    public class UIBoard : MonoBehaviour
    {
        public GameObject singleplayerTimer;
        public GameObject multiplayerTimer;
        public Text playerAName;
        public Text playerBName;

        bool _isMultiplayer;

        private void OnEnable()
        {
            GameManager.Single.multiplayerManager.evtStartGame += MultiplayerConnected;
        }



        private void OnDisable()
        {
            GameManager.Single.multiplayerManager.evtStartGame -= MultiplayerConnected;
        }

        public void Start()
        {
            if (!GameManager.Single.IsMultiplayer)
            {
                Debug.Log("A");
                singleplayerTimer.SetActive(true);
                multiplayerTimer.SetActive(false);
            }
            else
            {
                Debug.Log("B");
                IsMultiplayer();
            }
                
        }

        private void MultiplayerConnected()
        {
            playerAName.text = GameManager.Single.multiplayerManager.PlayerAName();
            playerBName.text = GameManager.Single.multiplayerManager.PlayerBName();

            playerAName.gameObject.SetActive(true);
            playerBName.gameObject.SetActive(true);

        }

        private void IsMultiplayer()
        {
           
            _isMultiplayer = true;
            singleplayerTimer.SetActive(false);
            multiplayerTimer.SetActive(true);

            playerAName.gameObject.SetActive(false);
            playerBName.gameObject.SetActive(false);

        }
    }

}
