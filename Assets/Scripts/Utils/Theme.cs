using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Utils
{
    [CreateAssetMenu(menuName = "Dots Triangles/Themes")]
    public class Theme : ScriptableObject
    {
        [Header("MainMenu")]
        public Font ThemeFont;
        public Color ThemeFontColor;
        public ThemesManager.Themes ThemeType;
        public Sprite LogoImage;
        public Sprite Triangle;
        public Sprite BackGround;
        public Sprite SinglePlayerIcon;
        public Sprite SinglePlayerBg;
        public Sprite ThemeSelectIcon;
        public Sprite ThemeSelectBg;
        public Sprite TutorialIcon;
        public Sprite TutorialBg;
        public Sprite ButtonShadow;
        public Sprite BottomBg; //the bg around the bottom buttons
        public Sprite SoundOnIcon;
        public Sprite SoundOffIcon;
        public Sprite AchievementIcon;
        public Sprite LanguagesIcon;
        public Sprite CreditsIcon;
        public Sprite ContentPanel;
        public Sprite BarInfoBoardSize;
        public Sprite BarInfoBoard;
        public Sprite BarInfoLevelSelect;
        public Sprite LevelInfoBoard;
        public Sprite BoardBackgroundSlider;
        public Sprite BoardFillSlider;
        public Sprite LevelBackgroundSlider;
        public Sprite LevelFillSlider;
        public Sprite SliderKnob;
        public Sprite DotSelected;
        public Sprite DotUnselected;
        public Sprite ButtonClose;
        public Sprite SinglePlayerPanelStart;
        public Sprite OkButton;

        [Header("Gameplay and tutorial")]
        public Texture2D PlayerOneTriangleTexture;
        public Texture2D PlayerTwoTriangleTexture;
        public Color32 PlayerOneColor;
        public Color32 PlayerTwoColor;
        public Sprite BackgroundGameplay;
        public Sprite GameboardDot;
        public Sprite GameboardDotSelected;
        public Sprite HomeButtonGameplay;
        public Sprite UndoButtonGameplay;
        public Sprite RedoButtonGameplay;
        public Sprite TimerBg;
        public Sprite SelectionAura;
        public Sprite ScoreBarBg;
        public Sprite PlayerOne;
        public Sprite PlayerTwo;
        public Sprite PlayerOneProgressBar;
        public Sprite PlayerTwoProgressBar;
        public Sprite CrownCoinLevel;
        public Sprite DotsContainerPanel;
        public Sprite OpponentsTurnBg;
        public Sprite WinLogo;
        public Sprite LoseLogo;
        public Sprite PlayerOnebgBar;
        public Sprite PlayerOneFillbar;
        public Sprite PlayerTwobgBar;
        public Sprite PlayerTwoFillbar;
        public Sprite WinPanelReplay;
        public Sprite WinPanelReplayBg;


    }

}
