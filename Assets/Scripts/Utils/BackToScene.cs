using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DotsTriangle.Utils
{
    public class BackToScene : MonoBehaviour
    {
        public string sceneName;

        private void OnEnable()
        {
            Backer.Instance.Register(GoBack);
        }

        private void OnDisable()
        {
            if (Backer.Instance != null)
                Backer.Instance.UnRegister(GoBack);
        }

        private void GoBack()
        {
            SceneManager.LoadScene(sceneName);  
        }
    }
}

