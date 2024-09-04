using System.Collections;
using System.Collections.Generic;
using DotsTriangle.UI;
using DotsTriangle.Utils;
using Localization;
using UnityEngine;
using UnityEngine.UI;

public class ThemesManager : MonoBehaviour
{
    public static ThemesManager Instance;
    public enum Themes
    {
        Mountain = 0,
        Classic =1,
        Yellow=2,
        Doodle=3
    }

    [SerializeField]
    private List<Theme> _themes;

    public Theme CurrentTheme;//the current active theme
    
    //references to everything meant for switching
    

    [Header("MainMenu")]
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _logoImage;
    [SerializeField]
    private List<Image> _decorTriangles;
    [SerializeField]
    private Image _singlePlayerIcon;
    [SerializeField]
    private Image _singlePlayerBackground;
    [SerializeField]
    private Image _themeSelectIcon;
    [SerializeField]
    private Image _themeSelectBackground;
    [SerializeField]
    private Image _tutorialButtonIcon;
    [SerializeField]
    private Image _tutorialButtonBg;
    [SerializeField]
    private Image _bottomBg;
    [SerializeField]
    private Image _soundButton;
    [SerializeField]
    private Image _achievmentsButton;
    [SerializeField]
    private Image _languagesButton;
    [SerializeField]
    private Image _creditsButton;
    [SerializeField]
    private Image _contentSinglePlayer;
    [SerializeField]
    private Image _barInfoBoardSize;
    [SerializeField]
    private Image _barInfoBoard;
    [SerializeField]
    private Image _barInfoLevelSelect;
    [SerializeField]
    private Image _levelInfoBoard;
    [SerializeField]
    private Image _boardBackgroundSlider;
    [SerializeField]
    private Image _boardFillSlider;
    [SerializeField]
    private Image _levelBackgroundSlider;
    [SerializeField]
    private Image _levelFillSlider;
    [SerializeField]
    private List<Image> _dots;
    [SerializeField]
    private Image _boardSelectKnob;
    [SerializeField]
    private Image _levelSelectKnob;
    [SerializeField]
    private Image _singlePlayerButtonClose;
    [SerializeField]
    private Image _singlePlayerPanelStart;
    [SerializeField]
    private List<Image> _buttonShadows;
    [SerializeField]
    private Image _creditsMenuContent;
    [SerializeField]
    private Image _creditsMenuClose;
    [SerializeField]
    private Image _creditsMenuOK;
    [SerializeField]
    private Image _themesMenuContent;
    [SerializeField]
    private Image _themesMenuClose;
    [SerializeField]
    private Image _themesMenuOK;
    [SerializeField]
    private Image _themePopupMenuContent;
    [SerializeField]
    private Image _themePopupMenuClose;
    [SerializeField]
    private Image _themePopupMenuOK;
    [SerializeField]
    private Image _areYouSurePopupMenuContent;
    [SerializeField]
    private Image _areYouSurePopupMenuClose;
    [SerializeField]
    private Image _areYouSurePopupMenuHome;
    [SerializeField]
    private Image _achievementPopupMenuContent;
    [SerializeField]
    private Image _achievementPopupMenuClose;
    [SerializeField]
    private Image _achievementPopupMenuOk;
    [SerializeField]
    private List<Image> _achievementPopupMenubgs;
    [SerializeField]
    private List<Image> _achievementPopupMenufronts;
    [SerializeField]
    private Image _languagesPopupMenuContent;
    [SerializeField]
    private Image _languagesPopupMenuClose;
    [SerializeField]
    private Image _languagesPopupMenuOk;

    [Header("Gameplay")]
    [SerializeField]
    private Image _backgroundGamePlay;
    [SerializeField]
    private Image _bottomBgGamePlay;
    [SerializeField]
    private Image _soundButtonGameplay;
    [SerializeField]
    private Image _homeButtonGameplay;
    [SerializeField]
    private Image _undoButtonGameplay;
    [SerializeField]
    private Image _redoButtonGameplay;
    [SerializeField]
    private List<Image> _buttonShadowsGameplay;
    [SerializeField]
    private List<Image> _decorTrianglesGameplay;
    [SerializeField]
    private Image _timerBg;
    [SerializeField]
    private Image _selectionAuraPlOne;
    [SerializeField]
    private Image _selectionAuraPlTwo;
    [SerializeField]
    private Image _scoreBarBg;
    [SerializeField]
    private Image _playerOneSelected;
    [SerializeField]
    private Image _playerTwoSelected;
    [SerializeField]
    private Image _crownCoinLevel;
    [SerializeField]
    private Image _dotsContainerPanel;
    [SerializeField]
    private Image _opponentsTurnBg;
    [SerializeField]
    private Image _winPanelContent;
    [SerializeField]
    private Image _winLogo;
    [SerializeField]
    private Image _loseLogo;
    [SerializeField]
    private Image _winPanelCloseButton;
    [SerializeField]
    private Image _playerOnebgBar;
    [SerializeField]
    private Image _playerOneFillbar;
    [SerializeField]
    private Image _playerTwobgBar;
    [SerializeField]
    private Image _playerTwoFillbar;
    [SerializeField]
    private Image _winPanelHomeButton;
    [SerializeField]
    private Image _winPanelReplay;
    [SerializeField]
    private Image _winPanelReplayBg;
    [SerializeField]
    private Image _pausePanelContent;
    [SerializeField]
    private Image _pausePanelCloseButton;
    [SerializeField]
    private Image _pausePanelHomeButton;


    [Header("Tutorial")]
    [SerializeField]
    private Image _bottomBgTutorial;
    [SerializeField]
    private Image _backgroundTutorial;
    [SerializeField]
    private Image _homeButtonTutorial;
    [SerializeField]
    private Image _undoButtonTutorial;
    [SerializeField]
    private Image _redoButtonTutorial;
    [SerializeField]
    private List<Image> _buttonShadowsTutorial;
    [SerializeField]
    private List<Image> _decorTrianglesTutorial;
    [SerializeField]
    private Image _timerBgTutorial;
    [SerializeField]
    private Image _selectionAuraPlOneTutorial;
    [SerializeField]
    private Image _selectionAuraPlTwoTutorial;
    [SerializeField]
    private Image _scoreBarBgTutorial;
    [SerializeField]
    private Image _playerOneSelectedTutorial;
    [SerializeField]
    private Image _playerTwoSelectedTutorial;
    [SerializeField]
    private Image _crownCoinLevelTutorial;
    [SerializeField]
    private Image _dotsContainerPanelTutorial;
    [SerializeField]
    private Image _winPanelContentTutorial;
    [SerializeField]
    private Image _winPanelCloseButtonTutorial;
    [SerializeField]
    private Image _winPanelPlayButtonTutorial;
    [SerializeField]
    private Image _winPanelPlayButtonBg;
    [SerializeField]
    private Image _infoPanelContent;
    [SerializeField]
    private Image _infoPanelCloseButton;
    [SerializeField]
    private Image _infoPanelPlayButton;
    [SerializeField]
    private Image _infoPanelPlayButtonBg;
   

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (Instance != null)
        {
            Destroy(Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
        

        if (PlayerPrefs.HasKey("themeIndex") == false)
        {
            PlayerPrefs.SetInt("themeIndex", 0);
        }
        Themes currentTheme = (Themes)PlayerPrefs.GetInt("themeIndex");
        for (int i = 0; i < _themes.Count; i++)
        {
            if (_themes[i].ThemeType == currentTheme) //select the theme according to the input and change it
            {
                
                InitializeTheme(_themes[i]);
                return;
            }

        }
       
    }
    //sets up the theme
    public void InitializeTheme(Theme thisTheme)
    {
        CurrentTheme = thisTheme; //set the current Theme
        if (_bottomBg != null)
        {
            _bottomBg.overrideSprite = CurrentTheme.BottomBg;
        }
        if (_background != null)
        {
            _background.overrideSprite = CurrentTheme.BackGround;
        }
        if (_logoImage != null)
        {
            _logoImage.overrideSprite = CurrentTheme.LogoImage;
        }
        if (_decorTriangles.Count>0)
        {
            for (int i = 0; i < _decorTriangles.Count; i++)
            {
                _decorTriangles[i].overrideSprite = CurrentTheme.Triangle;
            }
           
        }
        if (_singlePlayerIcon != null)
        {
            _singlePlayerIcon.overrideSprite = CurrentTheme.SinglePlayerIcon;
        }
        if (_singlePlayerBackground != null)
        {
            _singlePlayerBackground.overrideSprite = CurrentTheme.SinglePlayerBg;
        }
        if (_themeSelectIcon != null)
        {
            _themeSelectIcon.overrideSprite = CurrentTheme.ThemeSelectIcon;
        }
        if (_themeSelectBackground != null)
        {
            _themeSelectBackground.overrideSprite = CurrentTheme.ThemeSelectBg;
        }
        if (_tutorialButtonIcon != null)
        {
            _tutorialButtonIcon.overrideSprite = CurrentTheme.TutorialIcon;
        }
        if (_tutorialButtonBg != null)
        {
            _tutorialButtonBg.overrideSprite = CurrentTheme.TutorialBg;
        }
        if (_soundButton != null)
        {
            if (SoundManager.Instance.MusicOn)
            {
                _soundButton.overrideSprite = CurrentTheme.SoundOnIcon;
            }
            else
            {
                _soundButton.overrideSprite = CurrentTheme.SoundOffIcon;
            }
            
        }
        if (_achievmentsButton != null)
        {
            _achievmentsButton.overrideSprite = CurrentTheme.AchievementIcon;
        }
        if (_languagesButton != null)
        {
            _languagesButton.overrideSprite = CurrentTheme.LanguagesIcon;
        }
        if (_creditsButton != null)
        {
            _creditsButton.overrideSprite = CurrentTheme.CreditsIcon;
        }
        if (_buttonShadows.Count>0)
        {
            for (int i = 0; i < _buttonShadows.Count; i++)
            {
                _buttonShadows[i].overrideSprite = CurrentTheme.ButtonShadow;
            }
           
        }
        if (_contentSinglePlayer != null)
        {
            _contentSinglePlayer.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_barInfoBoardSize != null)
        {
            _barInfoBoardSize.overrideSprite = CurrentTheme.BarInfoBoardSize;
        }
        if (_barInfoBoard != null)
        {
            _barInfoBoard.overrideSprite = CurrentTheme.BarInfoBoard;
        }
        if (_barInfoLevelSelect != null)
        {
            _barInfoLevelSelect.overrideSprite = CurrentTheme.BarInfoLevelSelect;
        }
        if (_levelInfoBoard != null)
        {
            _levelInfoBoard.overrideSprite = CurrentTheme.LevelInfoBoard;
        }
        if (_boardBackgroundSlider != null)
        {
            _boardBackgroundSlider.overrideSprite = CurrentTheme.BoardBackgroundSlider;
        }
        if (_boardFillSlider != null)
        {
            _boardFillSlider.overrideSprite = CurrentTheme.BoardFillSlider;
        }
        if (_levelBackgroundSlider != null)
        {
            _levelBackgroundSlider.overrideSprite = CurrentTheme.LevelBackgroundSlider;
        }
        if (_levelFillSlider != null)
        {
            _levelFillSlider.overrideSprite = CurrentTheme.LevelFillSlider;
        }
        if (_dots.Count > 0)
        {
            for (int i = 0; i < _dots.Count; i++)
            {
                _dots[i].overrideSprite = CurrentTheme.DotUnselected;
            }
            UIMainMenu.Instance.UpdateBoard();
            UIMainMenu.Instance.UpdateDiff();


        }
        if (_boardSelectKnob != null)
        {
            _boardSelectKnob.overrideSprite = CurrentTheme.SliderKnob;
        }
        if (_levelSelectKnob != null)
        {
            _levelSelectKnob.overrideSprite = CurrentTheme.SliderKnob;
        }
        if (_singlePlayerButtonClose != null)
        {
            _singlePlayerButtonClose.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_singlePlayerPanelStart != null)
        {
            _singlePlayerPanelStart.overrideSprite = CurrentTheme.SinglePlayerPanelStart;
        }
        if (_creditsMenuContent != null)
        {
            _creditsMenuContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_creditsMenuOK != null)
        {
            _creditsMenuOK.overrideSprite = CurrentTheme.OkButton;
        }
        if (_themesMenuContent != null)
        {
            _themesMenuContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_creditsMenuClose != null)
        {
            _creditsMenuClose.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_themesMenuClose != null)
        {
            _themesMenuClose.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_themesMenuOK != null)
        {
            _themesMenuOK.overrideSprite = CurrentTheme.OkButton;
        }
        if (_themePopupMenuContent != null)
        {
            _themePopupMenuContent.overrideSprite = CurrentTheme.ContentPanel;
        }

        if (_themePopupMenuClose != null)
        {
            _themePopupMenuClose.overrideSprite = CurrentTheme.ButtonClose;
        }

        if (_themePopupMenuOK != null)
        {
            _themePopupMenuOK.overrideSprite = CurrentTheme.OkButton;
        }
        if (_areYouSurePopupMenuContent != null)
        {
            _areYouSurePopupMenuContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_areYouSurePopupMenuClose != null)
        {
            _areYouSurePopupMenuClose.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_areYouSurePopupMenuHome != null)
        {
            _areYouSurePopupMenuHome.overrideSprite = CurrentTheme.OkButton;
        }

        if (_achievementPopupMenuContent != null)
        {
            _achievementPopupMenuContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_achievementPopupMenuClose != null)
        {
            _achievementPopupMenuClose.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_achievementPopupMenuOk != null)
        {
            _achievementPopupMenuOk.overrideSprite = CurrentTheme.OkButton;
        }
        if (_achievementPopupMenubgs.Count > 0)
        {
            for (int i = 0; i < _achievementPopupMenubgs.Count; i++)
            {
                _achievementPopupMenubgs[i].overrideSprite = CurrentTheme.BarInfoBoardSize;
            }
        }
        if (_achievementPopupMenufronts.Count > 0)
        {
            for (int i = 0; i < _achievementPopupMenufronts.Count; i++)
            {
                _achievementPopupMenufronts[i].overrideSprite = CurrentTheme.BarInfoBoard;
            }
        }
        if (_languagesPopupMenuContent != null)
        {
            _languagesPopupMenuContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_languagesPopupMenuClose != null)
        {
            _languagesPopupMenuClose.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_languagesPopupMenuOk != null)
        {
            _languagesPopupMenuOk.overrideSprite = CurrentTheme.OkButton;
        }
        

        //gameplay
        if (_bottomBgGamePlay != null)
        {
            _bottomBgGamePlay.overrideSprite = CurrentTheme.BottomBg;
        }
        if (_backgroundGamePlay != null)
        {
            _backgroundGamePlay.overrideSprite = CurrentTheme.BackgroundGameplay;
        }
        if (_soundButtonGameplay != null)
        {
            if (SoundManager.Instance.MusicOn)
            {
                _soundButtonGameplay.overrideSprite = CurrentTheme.SoundOnIcon;
            }
            else
            {
                _soundButtonGameplay.overrideSprite = CurrentTheme.SoundOffIcon;
            }
            
        }
        if (_homeButtonGameplay != null)
        {
            _homeButtonGameplay.overrideSprite = CurrentTheme.HomeButtonGameplay;
        }
        if (_undoButtonGameplay != null)
        {
            _undoButtonGameplay.overrideSprite = CurrentTheme.UndoButtonGameplay;
        }
        if (_redoButtonGameplay != null)
        {
            _redoButtonGameplay.overrideSprite = CurrentTheme.RedoButtonGameplay;
        }
        if (_buttonShadowsGameplay.Count>0)
        {
            for (int i = 0; i < _buttonShadowsGameplay.Count; i++)
            {
                _buttonShadowsGameplay[i].overrideSprite = CurrentTheme.ButtonShadow;
            }
            
        }
        if (_decorTrianglesGameplay.Count > 0)
        {
            for (int i = 0; i < _decorTrianglesGameplay.Count; i++)
            {
                _decorTrianglesGameplay[i].overrideSprite = CurrentTheme.Triangle;
            }

        }
        if (_timerBg != null)
        {
            _timerBg.overrideSprite = CurrentTheme.TimerBg;
        }
        if (_selectionAuraPlOne != null)
        {
            _selectionAuraPlOne.overrideSprite = CurrentTheme.SelectionAura;
        }
        if (_selectionAuraPlTwo != null)
        {
            _selectionAuraPlTwo.overrideSprite = CurrentTheme.SelectionAura;
        }
        if (_scoreBarBg != null)
        {
            _scoreBarBg.overrideSprite = CurrentTheme.ScoreBarBg;
        }
        if (_playerOneSelected != null)
        {
            _playerOneSelected.overrideSprite = CurrentTheme.PlayerOne;
        }
        if (_playerTwoSelected != null)
        {
            _playerTwoSelected.overrideSprite = CurrentTheme.PlayerTwo;
        }
        if (_crownCoinLevel != null)
        {
            _crownCoinLevel.overrideSprite = CurrentTheme.CrownCoinLevel;
        }
        if (_dotsContainerPanel != null)
        {
            _dotsContainerPanel.overrideSprite = CurrentTheme.DotsContainerPanel;
        }
        if (_opponentsTurnBg != null)
        {
            _opponentsTurnBg.overrideSprite = CurrentTheme.OpponentsTurnBg;
        }
        if (_winPanelContent != null)
        {
            _winPanelContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_winLogo != null)
        {
            _winLogo.overrideSprite = CurrentTheme.WinLogo;
        }
        if (_loseLogo != null)
        {
            _loseLogo.overrideSprite = CurrentTheme.LoseLogo;
        }
        if (_winPanelCloseButton != null)
        {
            _winPanelCloseButton.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_playerOnebgBar != null)
        {
            _playerOnebgBar.overrideSprite = CurrentTheme.PlayerOnebgBar;
        }
        if (_playerOneFillbar != null)
        {
            _playerOneFillbar.overrideSprite = CurrentTheme.PlayerOneFillbar;
        }
        if (_playerTwobgBar != null)
        {
            _playerTwobgBar.overrideSprite = CurrentTheme.PlayerTwobgBar;
        }
        if (_playerTwoFillbar != null)
        {
            _playerTwoFillbar.overrideSprite = CurrentTheme.PlayerTwoFillbar;
        }
        if (_winPanelHomeButton != null)
        {
            _winPanelHomeButton.overrideSprite = CurrentTheme.HomeButtonGameplay;
        }
        if (_winPanelReplay != null)
        {
            _winPanelReplay.overrideSprite = CurrentTheme.WinPanelReplay;
        }
        if (_winPanelReplayBg != null)
        {
            _winPanelReplayBg.overrideSprite = CurrentTheme.WinPanelReplayBg;
        }
        if (_pausePanelContent != null)
        {
            _pausePanelContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_pausePanelCloseButton!= null)
        {
            _pausePanelCloseButton.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_pausePanelHomeButton!= null)
        {
            _pausePanelHomeButton.overrideSprite = CurrentTheme.HomeButtonGameplay;
        }

        //tutorial
        if (_bottomBgTutorial != null)
        {
            _bottomBgTutorial.overrideSprite = CurrentTheme.BottomBg;
        }
        if (_backgroundTutorial != null)
        {
            _backgroundTutorial.overrideSprite = CurrentTheme.BackgroundGameplay;
        }
        if (_homeButtonTutorial != null)
        {
            _homeButtonTutorial.overrideSprite = CurrentTheme.HomeButtonGameplay;
        }
        if (_undoButtonTutorial != null)
        {
            _undoButtonTutorial.overrideSprite = CurrentTheme.UndoButtonGameplay;
        }
        if (_redoButtonTutorial != null)
        {
            _redoButtonTutorial.overrideSprite = CurrentTheme.RedoButtonGameplay;
        }
        if (_buttonShadowsTutorial.Count>0)
        {
            for (int i = 0; i < _buttonShadowsTutorial.Count; i++)
            {
                _buttonShadowsTutorial[i].overrideSprite = CurrentTheme.ButtonShadow;
            }
           
        }
        if (_decorTrianglesTutorial.Count > 0)
        {
            for (int i = 0; i < _decorTrianglesTutorial.Count; i++)
            {
                _decorTrianglesTutorial[i].overrideSprite = CurrentTheme.Triangle;
            }

        }
        if (_timerBgTutorial != null)
        {
            _timerBgTutorial.overrideSprite = CurrentTheme.TimerBg;
        }
        if (_selectionAuraPlOneTutorial != null)
        {
            _selectionAuraPlOneTutorial.overrideSprite = CurrentTheme.SelectionAura;
        }
        if (_selectionAuraPlTwoTutorial != null)
        {
            _selectionAuraPlTwoTutorial.overrideSprite = CurrentTheme.SelectionAura;
        }
        if (_scoreBarBgTutorial != null)
        {
            _scoreBarBgTutorial.overrideSprite = CurrentTheme.ScoreBarBg;
        }
        if (_playerOneSelectedTutorial != null)
        {
            _playerOneSelectedTutorial.overrideSprite = CurrentTheme.PlayerOne;
        }
        if (_playerTwoSelectedTutorial != null)
        {
            _playerTwoSelectedTutorial.overrideSprite = CurrentTheme.PlayerTwo;
        }
        if (_crownCoinLevelTutorial != null)
        {
            _crownCoinLevelTutorial.overrideSprite = CurrentTheme.CrownCoinLevel;
        }
        if (_dotsContainerPanelTutorial != null)
        {
            _dotsContainerPanelTutorial.overrideSprite = CurrentTheme.DotsContainerPanel;
        }
        if (_winPanelContentTutorial != null)
        {
            _winPanelContentTutorial.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_winPanelCloseButtonTutorial != null)
        {
            _winPanelCloseButtonTutorial.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_winPanelPlayButtonTutorial != null)
        {
            _winPanelPlayButtonTutorial.overrideSprite = CurrentTheme.SinglePlayerIcon;
        }
        if (_winPanelPlayButtonBg != null)
        {
            _winPanelPlayButtonBg.overrideSprite = CurrentTheme.WinPanelReplayBg;
        }
        if (_infoPanelContent != null)
        {
            _infoPanelContent.overrideSprite = CurrentTheme.ContentPanel;
        }
        if (_infoPanelCloseButton != null)
        {
            _infoPanelCloseButton.overrideSprite = CurrentTheme.ButtonClose;
        }
        if (_infoPanelPlayButton != null)
        {
            _infoPanelPlayButton.overrideSprite = CurrentTheme.SinglePlayerIcon;
        }
        if (_infoPanelPlayButtonBg != null)
        {
            _infoPanelPlayButtonBg.overrideSprite = CurrentTheme.WinPanelReplayBg;
        }

        TranslatedText[] texts = (TranslatedText[])FindObjectsOfType(typeof(TranslatedText));
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].UpdateText();
        }

    }
}
