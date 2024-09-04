using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Core
{
    [System.Serializable]
    public class MeshCreator
    {
        public MeshFilter trianglePrefab;
        
        public GameObject CreateTriangleObj(Vector3 a, Vector3 b, Vector3 c, Color color, PlayerType playerType)
        {
            MeshFilter mf = MonoBehaviour.Instantiate(trianglePrefab);
            mf.mesh = CreateTriangle(a, b, c);

            MeshRenderer mr = mf.GetComponent<MeshRenderer>();
            if (mr != null && (ThemesManager.Themes)PlayerPrefs.GetInt("themeIndex") != ThemesManager.Themes.Doodle)
            {
                mr.material.SetColor("_TintColor", color);
                mr.material.color = color;
               
            }
            else if (mr != null && (ThemesManager.Themes) PlayerPrefs.GetInt("themeIndex") == ThemesManager.Themes.Doodle)
            {
                if (playerType == PlayerType.PlayerA)
                {
                    mr.material.mainTexture = ThemesManager.Instance.CurrentTheme.PlayerOneTriangleTexture;
                }
                else if (playerType == PlayerType.PlayerB)
                {
                    mr.material.mainTexture = ThemesManager.Instance.CurrentTheme.PlayerTwoTriangleTexture;
                }
                mr.material.color = new Color32(255,255,255,0);
            }

            return mf.gameObject;
        }

        public void SetColor(GameObject go, Color color)
        {
            if(go != null)
            {
                MeshRenderer mr = go.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material.SetColor("_TintColor", color);
                    mr.material.color = color;
                }
            }
           
        }

        public Mesh CreateTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            Mesh mesh = new Mesh();

            mesh.vertices = new Vector3[] { a, b, c };
            mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
            mesh.triangles = new int[] { 0, 1, 2 };

            return mesh;
        }
    }
}

