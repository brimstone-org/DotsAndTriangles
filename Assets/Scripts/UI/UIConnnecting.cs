using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;
using System;
using UnityEngine.UI;
using DotsTriangle.Data;

namespace DotsTriangle.UI
{
    public class UIConnnecting : MonoBehaviour
    {
        public MultiplayerManager multiplayerManager;
        public Transform panel;
        public Text text;
        public float timeToHidePanel = 0.5f;
        public Text estMatchingTimeTxt;
        public Text passedTimeTxt;

        public FloatType estMatchingTime;
        public FloatType passedTime; 

        bool _isConnecting;

        private void OnEnable()
        {
            multiplayerManager.evtVerbose += Verbose;
            multiplayerManager.evtConnecting += Connecting;
            multiplayerManager.evtStartGame += StartGame;
        }

        private void StartGame()
        {
            StartCoroutine(HidePanel());
        }

        private void Update()
        {
            if (estMatchingTime.Value > 0)
                estMatchingTimeTxt.text = Format(estMatchingTime.Value);
            else
                estMatchingTimeTxt.text = "--:--";

            if (passedTime.Value > 0)
                passedTimeTxt.text = Format(passedTime.Value);
            else
                passedTimeTxt.text = "--:--";
        }

        private string Format(float v)
        {
            string ss = Convert.ToInt32(v % 60).ToString("00");
            string mm = (Math.Floor(v / 60) % 60).ToString("00");
            return mm + ":" + ss;
        }

        private IEnumerator HidePanel()
        {
            yield return new WaitForSeconds(timeToHidePanel);
            panel.gameObject.SetActive(false);
            _isConnecting = false;
        }

        private void Connecting()
        {
            if (panel != null)
            {
                panel.gameObject.SetActive(true);
                _isConnecting = true;
            }          
        }

        private void Verbose(string obj)
        {
            if (text != null)
                text.text = obj;
        }

        private void OnDisable()
        {
            multiplayerManager.evtVerbose -= Verbose;
            multiplayerManager.evtConnecting -= Connecting;
        }

        public void Start()
        {
            
            if (!_isConnecting)
                panel.gameObject.SetActive(false);
        }
    }
}

