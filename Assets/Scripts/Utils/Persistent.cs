using UnityEngine;
using System.Collections;

namespace DotsTriangle.Utils
{
    public class Persistent<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;

        public static T Instance
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


        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                DontDestroyOnLoad(gameObject);


            }
            else
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DontDestroyOnLoad(instance.gameObject);
                }

            }
        }
    }
}