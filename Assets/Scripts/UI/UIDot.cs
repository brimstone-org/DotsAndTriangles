using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DotsTriangle.Core;
using DotsTriangle.Data;

using System;

namespace DotsTriangle.UI
{
    public class UIDot : MonoBehaviour
    {
        public Button btn;
        public Image circle;
        public Text text;
        public Image selected;
        private Dot _dot;
        private bool _isRegistered;
        private System.Action<UIDot> _action;
        private GameColors _gameColors;
        public Canvas DotCanvas;

        public Dot Dot
        {
            get
            {
                return _dot;
            }

            set
            {
                _dot = value;
            }
        }

        public void Init(Dot dot, GameColors colors, System.Action<UIDot> action)
        {
            circle.overrideSprite = ThemesManager.Instance.CurrentTheme.GameboardDot;
            selected.overrideSprite = ThemesManager.Instance.CurrentTheme.GameboardDotSelected;
            selected.gameObject.SetActive(false);
            _gameColors = colors;
          //  SetColor(_gameColors.dotsDefault);
            text.text = "(" + dot.x + "," + dot.y + ")";
          

            Dot = dot;
            if (!_isRegistered)
            {
                _action = action;
                btn.onClick.AddListener(DoClick);
                _isRegistered = true;
            }
        }

        public void IsSlected(bool yes)
        {
            if (yes)
            {
                circle.color = _gameColors.dotsDefaultSelected;
                selected.gameObject.SetActive(true);
            }
            else
            {
                circle.color = _gameColors.dotsDefault;
                selected.gameObject.SetActive(false);

            }
        }

        public void Show(bool yes, bool withDeactivation = false)
        {
            if (yes)
            {
                circle.color = _gameColors.dotsDefault;
                DotCanvas.overrideSorting = true;
            }
            else

            {
                Color c = _gameColors.dotsDefault;
                circle.color = new Color(c.r, c.g, c.b, 0.2f);
                DotCanvas.overrideSorting = false;
            }
            if (withDeactivation)
                gameObject.SetActive(yes);
        }

        public void EnableClick(bool yes)
        {
            btn.gameObject.SetActive(yes);
        }

        public void SetColor(Color color)
        {
            circle.color = color;
        }

        private void DoClick()
        {
            if(_action != null)
            {
                _action(this);
            }
        }
    }
}

