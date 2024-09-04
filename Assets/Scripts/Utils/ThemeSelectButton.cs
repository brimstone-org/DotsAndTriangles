using System.Collections;
using System.Collections.Generic;
using DotsTriangle.Utils;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSelectButton : MonoBehaviour
{
    [SerializeField]
    private Theme ThisTheme;
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Image _screenshot;
    [SerializeField]
    private Image _screenshotTarget;
    [SerializeField]
    private GameObject _screenshotPanel;
    [SerializeField]
    private Button _themeSelectButton;
    [SerializeField]
    private GameObject _myAura;
    [SerializeField]
    private List<GameObject> _allAuras;


    private void OnEnable()
    {
        _myAura.SetActive(false);
        if (ThemesManager.Instance.CurrentTheme.ThemeType == ThisTheme.ThemeType)
        {
            _myAura.SetActive(true);
        }
    }

    public void OnClick()
    {
        foreach (var aura in _allAuras)
        {
            aura.SetActive(false);
        }
        _myAura.SetActive(true);
        switch (ThisTheme.ThemeType)
        {
            case ThemesManager.Themes.Classic:
                _title.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                if (ThemesManager.Instance.CurrentTheme.ThemeType == ThemesManager.Themes.Doodle)
                {
                    _title.color = Color.white;
                }
                else
                {
                    _title.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                }
                _title.text = LanguageManager.Get("classic");
                break;
            case ThemesManager.Themes.Mountain:
                _title.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                if (ThemesManager.Instance.CurrentTheme.ThemeType == ThemesManager.Themes.Doodle)
                {
                    _title.color = Color.white;
                }
                else
                {
                    _title.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                }
                _title.text = LanguageManager.Get("mount");
                break;
            case ThemesManager.Themes.Yellow:
                _title.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                if (ThemesManager.Instance.CurrentTheme.ThemeType == ThemesManager.Themes.Doodle)
                {
                    _title.color = Color.white;
                }
                else
                {
                    _title.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                }
                _title.text = LanguageManager.Get("gold");
                break;
            case ThemesManager.Themes.Doodle:
                _title.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
                if (ThemesManager.Instance.CurrentTheme.ThemeType == ThemesManager.Themes.Doodle)
                {
                    _title.color = Color.white;
                }
                else
                {
                    _title.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
                }
                _title.text = LanguageManager.Get("doodle");
                break;

        }
        _screenshotTarget.overrideSprite = _screenshot.overrideSprite;
        _themeSelectButton.onClick.AddListener(()=> SelectThisTheme(ThisTheme));
        _screenshotPanel.GetComponent<Animator>().Play("PanelShow");
    }

    public void SelectThisTheme(Theme thisTheme)
    {
        ThemesManager.Instance.InitializeTheme(thisTheme);
        PlayerPrefs.SetInt("themeIndex", (int)thisTheme.ThemeType);
        _screenshotPanel.GetComponent<Animator>().Play("PanelHide");
    }
}
