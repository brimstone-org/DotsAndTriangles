using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DotsTriangle.Data
{
    [CreateAssetMenu(fileName = "GoToScene", menuName = "Dots Triangles/GoToScene")]
    public class GoToScene : ScriptableObject
    {
        public string scene;

        public void Go()
        {
            if(scene != "")
            {
                SceneManager.LoadScene(scene);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}

