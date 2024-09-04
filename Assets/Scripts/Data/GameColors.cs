using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Data
{
    [CreateAssetMenu(fileName = "GameColors", menuName = "Dots Triangles/GameColors")]
    public class GameColors : ScriptableObject
    {
        public Color dotsDefault;
        public Color dotsDefaultSelected;
        public Color playerA;
        public Color playerB;
        public Color uiButton;
        public Color lineColor;
    }
}

