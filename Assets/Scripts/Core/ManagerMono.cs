using UnityEngine;
using System.Collections;

namespace DotsTriangle.Core
{
    public class ManagerMono<T> : MonoBehaviour where T : MonoBehaviour
    {

        protected static T instance;

        public static T Single
        {
            get
            {

                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }
                return instance;
            }
        }

    }
}


