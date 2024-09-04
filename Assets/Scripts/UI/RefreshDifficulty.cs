using System.Collections;
using System.Collections.Generic;
using DotsTriangle.UI;
using UnityEngine;

public class RefreshDifficulty : MonoBehaviour
{
    [SerializeField] private UIMainMenu _menuCOntroller;

    void OnEnable()
    {
        //Debug.Log("enabling");
        _menuCOntroller.UpdateDiff();
    }
}
