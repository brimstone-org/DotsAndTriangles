using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;
using UnityEngine.UI;
using System;
using DotsTriangle.Utils;

namespace DotsTriangle.UI
{
    public class TutorialVisualizer : MonoBehaviour
    {
        public GameManager gm;
        public DotsVizualiser dotsViz;
        public Mover player;

        public GameObject coverPointPanel;
        public Image zonePoint;
        public Button button;
        public Button undo;
        public Button redo;
        public ISaveData data;

        public List<Vector2> points;

        public Animator introPanel;

        UIDot _nextDot;
        int _index;

        private void OnEnable()
        {
            if (gm.IsTutorial)
            {
                dotsViz.evtVisualizerReady += StartTut;
                gm.evtPlayerChanged += PlayerChanged;
            }
          
        }

        private void OnDisable()
        {
            if (gm.IsTutorial)
            {
                dotsViz.evtVisualizerReady -= StartTut;
                gm.evtPlayerChanged -= PlayerChanged;
            }

        }

        // Use this for initialization
        void StartTut()
        {
            Debug.Log("TUT:" + gm.IsTutorial);
            if (gm.IsTutorial)
            {
                undo.gameObject.SetActive(false);
                redo.gameObject.SetActive(false);

                gm.playerB.PlayerName = "[AI]Tutorial";


                gm.ticker.StopAll(); 
                gm.ResetTutorialForced();

                introPanel.Play("PanelShow");
            }
        }

        public void ShowTheHand()
        {
            coverPointPanel.SetActive(true);

            Vector2 v = points[_index];

            _nextDot = dotsViz.GetDotUI((int)v.x, (int)v.y);
            Vector3 pos = dotsViz.GetDotUIPos((int)v.x, (int)v.y);

            zonePoint.transform.position = new Vector3(pos.x, pos.y, zonePoint.transform.position.z);
            button.transform.position = zonePoint.transform.position;
            zonePoint.gameObject.SetActive(true);

            gm.ticker.StartTicker();
        }

        public void ClickNextPoint()
        {
            if (!player.IsActive || DotsVizualiser.Instance.CanDraw == false)
                return;
            dotsViz.DotClicked(_nextDot);

            _index++;
            if (_index > points.Count - 1)
            {
                zonePoint.gameObject.SetActive(false);
                coverPointPanel.SetActive(false);

                gm.TutorialDone();
                return;
            }
                

            Vector2 v = points[_index];

            _nextDot = dotsViz.GetDotUI((int)v.x, (int)v.y);
            Vector3 pos = dotsViz.GetDotUIPos((int)v.x, (int)v.y);

            zonePoint.transform.position = new Vector3(pos.x, pos.y, zonePoint.transform.position.z);
            button.transform.position = zonePoint.transform.position;

            zonePoint.gameObject.SetActive(gm.ActiveMover == player);
        }


        private void PlayerChanged(PlayerType arg1, int arg2)
        {
            if (_index < points.Count - 1)
            {
                zonePoint.gameObject.SetActive(arg1 == player.PlayerType);
            }
            else
            {
                zonePoint.gameObject.SetActive(false);
            }
        }
    }
}
