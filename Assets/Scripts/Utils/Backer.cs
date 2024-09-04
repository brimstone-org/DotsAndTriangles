using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DotsTriangle.Utils
{
    public class Backer : Persistent<Backer>
    {
        public string lastScene = null;
        public string currentScene = null;
        public bool block;

        public event System.Action evtGoBack;
        public List<System.Action> backs = new List<Action>();

        public bool LoadLastScene()
        {
            if (lastScene == null || lastScene == "")
                return false;

            SceneManager.LoadScene(lastScene);
            return true;
        }

        public void Register(System.Action action)
        {
            backs.Add(action);
        }

        public void UnRegister(System.Action action)
        {
            backs.Remove(action);
        }

        public override void Awake()
        {
            base.Awake();

            SceneManager.sceneLoaded += SceneChanged;
        }

        private void SceneChanged(Scene arg0, LoadSceneMode arg1)
        {
            lastScene = currentScene;
            currentScene = arg0.name;
        }

        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android || Application.isEditor)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    if(backs.Count > 0)
                    {
                        backs[backs.Count - 1]();
                        backs.RemoveAt(backs.Count - 1);
                    }
                }
            }
        }
    }
}
