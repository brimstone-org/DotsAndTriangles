using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Data
{
    [CreateAssetMenu(fileName = "ColorType", menuName = "Dots Triangles/Color")]
    public class ColorType : ScriptableObject
    {
        [SerializeField]
        Color _value;

        public Color Value
        {
            get
            {
                return _value;
            }

            set
            {
                this._value = value;
            }
        }
    }
}

