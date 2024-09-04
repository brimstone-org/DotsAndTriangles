using DotsTriangle.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotsTriangle.Core
{
    public class Ticker : MonoBehaviour
    {
        public FloatType tickDelta;
        public TickerContainer container;
        public event System.Action<int, float> evtNewTick;
        public event System.Action<int, float, float> evtPlayerTimeTick;
        //public Text text;

        int _currentTick;
        float _timePassed;
        IEnumerator _playerTicker;

        public int CurrentTick
        {
            get
            {
                return _currentTick;
            }
        }

        private void Awake()
        {
            container.Ticker = this;
        }

        public void StopAll()
        {
            StopAllCoroutines();
        }

        // Use this for initialization
        public void StartTicker()
        {
             StartCoroutine(StartTicking());
        }

        public void StartPlayerTimer(Mover player, float time, Func<float> getServerTime, System.Action<Mover> timeOffCallback)
        {
            if (_playerTicker != null)
            {
                StopCoroutine(_playerTicker);
            }
            _playerTicker = PlayerTime(player, time, getServerTime, timeOffCallback);
            StartCoroutine(_playerTicker);
        }

        private IEnumerator PlayerTime(Mover player, float time, Func<float> getServerTime, Action<Mover> timeOffCallback)
        {
            float timePassedTick = 0;
            float timePassedOverall = 0;

            int timerTick = (int)(time / tickDelta.Value);
            float lastTimePassedOverall = 0;

            while (true)
            {
                yield return new WaitForEndOfFrame();

                lastTimePassedOverall = timePassedOverall;
                timePassedOverall = getServerTime();
                timePassedTick += timePassedOverall - lastTimePassedOverall;


                if (timePassedTick >= tickDelta.Value)
                {
                    timePassedTick -= tickDelta.Value;
                    timerTick--;
                    if(evtPlayerTimeTick != null)
                    {
                        evtPlayerTimeTick(timerTick, timerTick * tickDelta.Value, timePassedOverall / time);
                    }
                }

                if (timePassedOverall > time)
                {
                    timeOffCallback(player);
                    yield break;
                }
            }
        }

        private IEnumerator StartTicking()
        {
            float timePassed = 0;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                
                timePassed += Time.deltaTime;
                
                if(timePassed >= tickDelta.Value)
                {
                    timePassed -= tickDelta.Value;
                    _currentTick++;
                }
                  
                if (evtNewTick != null)
                    evtNewTick(_currentTick, _currentTick * tickDelta.Value);
            }
        }
    }

}
