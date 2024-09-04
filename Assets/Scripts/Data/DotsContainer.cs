using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;


namespace DotsTriangle.Data
{
    public class DotsContainer : ScriptableObject
    {

        Dots _dots;

        public Dots Dots
        {
            get
            {
                return _dots;
            }

            set
            {
                _dots = value;
            }
        }
    }

}
