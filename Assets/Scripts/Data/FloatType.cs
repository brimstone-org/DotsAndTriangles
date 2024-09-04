using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Data
{
    [CreateAssetMenu(fileName = "FloatType", menuName = "Dots Triangles/Float")]
    public class FloatType : ScriptableObject
    {
        [SerializeField]
        float _value;

        public float Value
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

