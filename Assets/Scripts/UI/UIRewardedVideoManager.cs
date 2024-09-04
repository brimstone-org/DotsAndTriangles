using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRewardedVideoManager : MonoBehaviour
{
    public static UIRewardedVideoManager Instance;

    public int UndoClicks; //how many times the player clicked the undo function


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
