using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;


namespace DotsTriangle.Data
{
    [CreateAssetMenu(fileName = "TickerContainer", menuName = "Dots Triangles/Ticker Container")]
    public class TickerContainer : ScriptableObject
    {

        Ticker _tickerContainer;

        public Ticker Ticker
        {
            get
            {
                return _tickerContainer;
            }

            set
            {
                _tickerContainer = value;
            }
        }
    }

}
