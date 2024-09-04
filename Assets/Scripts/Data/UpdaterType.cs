using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Data
{
    [CreateAssetMenu(fileName = "UpdaterType", menuName = "Dots Triangles/Updater")]
    public class UpdaterType : ScriptableObject
    {
        public event System.Action evtFired;

        public void Fire()
        {
            if (evtFired != null)
                evtFired();
        }
    }
}

