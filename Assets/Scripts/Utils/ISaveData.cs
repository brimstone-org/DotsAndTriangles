using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Utils
{
    public class ISaveData : ScriptableObject
    {
        public virtual void GetKey<T>(string key, ref T value, T defValue) { }
        public virtual void SetKey<T>(string key, T value) { }

    }
}

