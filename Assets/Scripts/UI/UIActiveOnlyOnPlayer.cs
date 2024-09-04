using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;
using UnityEngine.UI;
using System;

namespace DotsTriangle.UI
{
    public class UIActiveOnlyOnPlayer : MonoBehaviour
    {
        public GameManager gm;
        public PlayerType playerType;
        public Button btn;


        List<ButtonChild> bcs = new List<ButtonChild>();

        public struct ButtonChild
        {
            public Image obj;
            public Color initialColor;
        }
        private void OnEnable()
        {
            //Init();
            gm.evtPlayerChanged += PlayerChanged;
        }

        private void Init()
        {
            Image[] msc = btn.transform.GetComponentsInChildren<Image>();
            for(int i = 0; i < msc.Length; i++)
            {
                ButtonChild bc = new ButtonChild();
                bc.obj = msc[i];
                bc.initialColor = msc[i].color;
                bcs.Add(bc);
            }
        }

        private void OnDisable()
        {
            gm.evtPlayerChanged -= PlayerChanged;
        }

        private void PlayerChanged(PlayerType arg1, int arg2)
        {
            if(arg1 != playerType)
            {
                btn.interactable = false;
                bcs.ForEach(y => y.obj.color = btn.colors.disabledColor);
            }
            else
            {
                btn.interactable = true;
                bcs.ForEach(y => y.obj.color = y.initialColor);
            }
        }
    }

}
