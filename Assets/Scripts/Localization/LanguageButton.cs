using System.Collections;
using System.Collections.Generic;
using DotsTriangle.Utils;
using Localization;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    [SerializeField]
    private Image _selected;
    [SerializeField]
    private Text _langText;
    public enum Languages
    {
        English,
        German,
        French,
        Italian,
        Spanish,
        Portuguese

    }
    [SerializeField]
    private Languages _thisLanguage;
    [SerializeField]
    private List<Image> _selectedLanguages;

    [SerializeField]
    private Button _selectLanguage;

    [SerializeField]
    private Animator _panelAnim;

    void OnEnable()
    {
        GetComponent<Image>().overrideSprite = ThemesManager.Instance.CurrentTheme.BarInfoBoardSize;
        _selected.overrideSprite = ThemesManager.Instance.CurrentTheme.BarInfoBoard;
        _langText.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
        _langText.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
        switch (LanguageManager.Instance.language)
        {
            case SystemLanguage.English:

                if (_thisLanguage == Languages.English)
                {
                    for (int i = 0; i < _selectedLanguages.Count; i++)
                    {
                        _selectedLanguages[i].enabled = false;
                    }
                    _selected.enabled = true;
                }
                break;
            case SystemLanguage.German:
                if (_thisLanguage == Languages.German)
                {
                    for (int i = 0; i < _selectedLanguages.Count; i++)
                    {
                        _selectedLanguages[i].enabled = false;
                    }
                    _selected.enabled = true;
                }
                break;
            case SystemLanguage.Italian:
                if (_thisLanguage == Languages.Italian)
                {
                    for (int i = 0; i < _selectedLanguages.Count; i++)
                    {
                        _selectedLanguages[i].enabled = false;
                    }
                    _selected.enabled = true;
                }
                break;
            case SystemLanguage.French:
                if (_thisLanguage == Languages.French)
                {
                    for (int i = 0; i < _selectedLanguages.Count; i++)
                    {
                        _selectedLanguages[i].enabled = false;
                    }
                    _selected.enabled = true;
                }
                break;
            case SystemLanguage.Spanish:
                if (_thisLanguage == Languages.Spanish)
                {
                    for (int i = 0; i < _selectedLanguages.Count; i++)
                    {
                        _selectedLanguages[i].enabled = false;
                    }
                    _selected.enabled = true;
                }
                break;
            case SystemLanguage.Portuguese:
                if (_thisLanguage == Languages.Portuguese)
                {
                    for (int i = 0; i < _selectedLanguages.Count; i++)
                    {
                        _selectedLanguages[i].enabled = false;
                    }
                    _selected.enabled = true;
                }
                break;
        }
    }

    public void ClickThis()
    {
        _selectLanguage.onClick.RemoveAllListeners();
        _selectLanguage.onClick.AddListener(()=>SelectThisLanguage(_selectLanguage));
        for (int i = 0; i < _selectedLanguages.Count; i++)
        {
            _selectedLanguages[i].enabled = false;
        }
        _selected.enabled = true;
    }

    public void SelectThisLanguage(Button selectButton)
    {
        //reset all buttons' colors
       
        switch (_thisLanguage)
        {
            case Languages.English:
                LanguageManager.Instance.SetLanguage(SystemLanguage.English);
                PlayerPrefs.SetString("Language", "");
                break;
            case Languages.German:
                LanguageManager.Instance.SetLanguage(SystemLanguage.German);
                PlayerPrefs.SetString("Language", "_de");
                break;
            case Languages.French:
                LanguageManager.Instance.SetLanguage(SystemLanguage.French);
                PlayerPrefs.SetString("Language", "_fr");
                break;
            case Languages.Italian:
                LanguageManager.Instance.SetLanguage(SystemLanguage.Italian);
                PlayerPrefs.SetString("Language", "_it");
                break;
            case Languages.Spanish:
                LanguageManager.Instance.SetLanguage(SystemLanguage.Spanish);
                PlayerPrefs.SetString("Language", "_es");
                break;
            case Languages.Portuguese:
                LanguageManager.Instance.SetLanguage(SystemLanguage.Portuguese);
                PlayerPrefs.SetString("Language", "_pt");
                break;
                 
        }
        TranslatedText[] textsToUpdate = FindObjectsOfType(typeof(TranslatedText)) as TranslatedText[];
        for (int i = 0; i < textsToUpdate.Length; i++)
        {
            if (textsToUpdate[i].gameObject.activeSelf)
            {
                textsToUpdate[i].UpdateText();
            }
        }
        selectButton.GetComponent<PlaySound>().Play();
        _panelAnim.Play("PanelHide");
    }
}
