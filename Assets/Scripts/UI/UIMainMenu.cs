using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DotsTriangle;
using DotsTriangle.Data;
using System;
using Localization;

namespace DotsTriangle.UI
{

    public class UIMainMenu : MonoBehaviour
    {

        public Slider boardSizeSlider;
        public Slider difficultySlider;
        public Text boardSize;
        public Text levelDifficulty;
        public InputField localPlayerName;
        public string gamePlay = "GamePlay";
        public List<int> boardSizes;
        public DotsTriangle.Utils.ISaveData data;
        public List<string> difficultLevelsInfo;
        public Animator singlePlayerPanelAnim;
        public Animator multiplayerPanelAnim;
        public Animator AreYouSurePanelAnim;
        public Animator AchievementsPanelAnim;
        public Animator LanguagesPanelAnim;

        public GoToScene goToTutorial;
        public GoToScene goToGameplay;
        public Utils.PlaySound tap;
        public string musicLink;
        public int noDifficultyLevels;
        public FloatType difficultySO;
        public Button OpenGDPR;

        public static UIMainMenu Instance;
        float _difficulty;
        int _boardsize;
        int _boardsizeIndex;
        string _localPlayerName;
        int _soundOff;
        bool _init;

        private Color32 _dotsGreyColor = new Color32(135, 135, 135, 255);
        private Color32 _dotsYellowColor = new Color32(229, 159, 38, 255);
        [SerializeField]
        private Image[] _boardDots;
        [SerializeField]
        private Image[] _difficultyDots;

        //achievements
        public Image Achievement50;
        public Image Achievement100;
        public Image Achievement150;

        public int Victories;//Achiev50completed, Achiev100completed, Achiev150completed;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
                Instance = this;
            }
            else
            {
                Instance = this;
            }
            
        }
        public void Start()
        {
            Time.timeScale = 1;
            _init = true;
            //get translations for the level descriptions
            difficultLevelsInfo[0] = LanguageManager.Get("noob");
            difficultLevelsInfo[1] = LanguageManager.Get("rook");
            difficultLevelsInfo[2] = LanguageManager.Get("beg");
            difficultLevelsInfo[3] = LanguageManager.Get("int");
            difficultLevelsInfo[4] = LanguageManager.Get("int");
            difficultLevelsInfo[5] = LanguageManager.Get("int");
            difficultLevelsInfo[6] = LanguageManager.Get("adv");
            difficultLevelsInfo[7] = LanguageManager.Get("adv");
            difficultLevelsInfo[8] = LanguageManager.Get("mas");
            difficultLevelsInfo[9] = LanguageManager.Get("sure");
           // OpenGDPR.onClick.AddListener(()=>GDPRConsentPanel.Instance.OpenPopUp());
            Application.targetFrameRate = 60;

            boardSizeSlider.minValue = 0;
            boardSizeSlider.maxValue = boardSizes.Count - 1;

            difficultySlider.minValue = 0;
            difficultySlider.maxValue = noDifficultyLevels - 1;

            data.GetKey(Constants.BOARD_SIZE_INDEX, ref _boardsizeIndex, 3);
            _boardsize = boardSizes[_boardsizeIndex];
            data.SetKey(Constants.BOARD_SIZE, _boardsize);
            boardSizeSlider.value = _boardsizeIndex;

            data.GetKey(Constants.DIFFICULTY, ref _difficulty, (int)Mathf.Lerp(0, noDifficultyLevels, 0.2f));
            difficultySlider.value = _difficulty;
            difficultySO.Value = (_difficulty + 1) / noDifficultyLevels;

            GenLocalPlayerName();

            // update texts
            UpdateDiff();
            UpdateBoard();

            // force single player panel
            int forcedSingle = 0;
            data.GetKey(Constants.SHOW_SINGLEPLAYER_PANEL_FORCED, ref forcedSingle, 0);
            if (forcedSingle == 1)
            {
                data.SetKey(Constants.SHOW_SINGLEPLAYER_PANEL_FORCED, 0);
                ClickSingleplayer();
            }

            // force multiplayer panel
            int forcedMultiplayer = 0;
            data.GetKey(Constants.SHOW_MULTIPLAYER_PANEL_FORCED, ref forcedMultiplayer, 0);
            if (forcedMultiplayer == 1)
            {
                data.SetKey(Constants.SHOW_MULTIPLAYER_PANEL_FORCED, 0);
                ClickMultiplayer();
            }

            _init = false;

            //Achievemetns
            if (PlayerPrefs.HasKey("Victories") == false)
            {
                PlayerPrefs.SetInt("Victories", 0);
            }
            Victories = PlayerPrefs.GetInt("Victories");
            //Victories = 49;
            //if (PlayerPrefs.HasKey("Achievement50") == false)
            //{
            //    PlayerPrefs.SetInt("Achievement50", 0);
            //}
            //Achiev50completed = PlayerPrefs.GetInt("Achievement50");
            //if (PlayerPrefs.HasKey("Achievement100") == false)
            //{
            //    PlayerPrefs.SetInt("Achievement100", 0);
            //}
            //Achiev100completed = PlayerPrefs.GetInt("Achievement100");
            //if (PlayerPrefs.HasKey("Achievement150") == false)
            //{
            //    PlayerPrefs.SetInt("Achievement150", 0);
            //}
            //Achiev150completed = PlayerPrefs.GetInt("Achievement150");
            // PlayerPrefs.DeleteAll();

        }
       
        public void ClickAchievementsPanel()
        {
            
            Achievement50.fillAmount = (float)Victories / 50;
            Achievement100.fillAmount = (float)Victories / 100;
            Achievement150.fillAmount = (float)Victories / 150;
            AchievementsPanelAnim.Play("PanelShow");
        }

        public void ClickLanguagesPanel()
        {

            LanguagesPanelAnim.Play("PanelShow");
        }

        private void GenLocalPlayerName()
        {
            _localPlayerName = "Player#" + UnityEngine.Random.Range(0, 1000000);
            data.GetKey(Constants.LOCALPLAYER_NAME, ref _localPlayerName, _localPlayerName);
            Graphic ph = localPlayerName.placeholder;
            if (ph.GetType() == typeof(Text))
            {
                Text t = (Text)ph;
                t.text = _localPlayerName;
            }
        }

        public void SaveLocalPlayerName(string n)
        {
            if (n != null && n != "")
            {
                _localPlayerName = n;
                data.SetKey(Constants.LOCALPLAYER_NAME, _localPlayerName);
            }
        }

        public void SinglePlayer()
        {
            data.SetKey(Constants.IS_SINGLEPLAYER, 1);
            goToGameplay.Go();
        }

        public void Multiplayer()
        {
            data.SetKey(Constants.IS_SINGLEPLAYER, 0);
            goToGameplay.Go();
        }

        public void Tutorial()
        {
            data.SetKey(Constants.IS_SINGLEPLAYER, 1);
            data.SetKey(Constants.TUTORIAL_FORCE, 1);
            goToTutorial.Go();
        }

        public void BoardSizeUpdated(float t)
        {
            if (!_init)
                tap.Play();

            _boardsizeIndex = (int)t;
            _boardsize = boardSizes[_boardsizeIndex];
            data.SetKey(DotsTriangle.Constants.BOARD_SIZE_INDEX, _boardsizeIndex);
            data.SetKey(DotsTriangle.Constants.BOARD_SIZE, _boardsize);

            UpdateBoard();
        }

        public void DifficultyUpdated(float t)
        {
            if (!_init)
                tap.Play();

            _difficulty = t;
            data.SetKey(Constants.DIFFICULTY, _difficulty);

            difficultySO.Value = (_difficulty + 1) / noDifficultyLevels;

            UpdateDiff();
        }

        public void ClickSingleplayer()
        {
            int tutorial = 0;
            data.GetKey(Constants.TUTORIAL_DONE, ref tutorial, tutorial);

            if (tutorial == 0)
            {
                //goToTutorial.Go();
                Tutorial();
            }
            else
            {
                singlePlayerPanelAnim.Play("PanelShow");
            }
        }

        public void ClickMultiplayer()
        {
            multiplayerPanelAnim.Play("PanelShow");
        }

        public void QuitPanel()
        {
            AreYouSurePanelAnim.Play("PanelShow");
        }

        public void Quit()
        {
            Debug.Log("quitting app");
            Application.Quit();
        }

        public void LaunchMusicLink()
        {
            Application.OpenURL(musicLink);
        }

       public void UpdateBoard()
        {
            boardSize.text = _boardsize + " x " + _boardsize;
            for (int i = 0; i < _boardDots.Length; i++)
            {
                if (i <= _boardsize - 3)
                {
                    _boardDots[i].overrideSprite = ThemesManager.Instance.CurrentTheme.DotSelected;
                }
                else
                {
                    _boardDots[i].overrideSprite = ThemesManager.Instance.CurrentTheme.DotUnselected;
                }
                
            }
        }

        public void UpdateDiff()
        {
            difficultLevelsInfo[0] = LanguageManager.Get("noob");
            difficultLevelsInfo[1] = LanguageManager.Get("rook");
            difficultLevelsInfo[2] = LanguageManager.Get("beg");
            difficultLevelsInfo[3] = LanguageManager.Get("int");
            difficultLevelsInfo[4] = LanguageManager.Get("int");
            difficultLevelsInfo[5] = LanguageManager.Get("int");
            difficultLevelsInfo[6] = LanguageManager.Get("adv");
            difficultLevelsInfo[7] = LanguageManager.Get("adv");
            difficultLevelsInfo[8] = LanguageManager.Get("mas");
            difficultLevelsInfo[9] = LanguageManager.Get("sure");
            int t = (int)_difficulty;
            levelDifficulty.font = ThemesManager.Instance.CurrentTheme.ThemeFont;
            levelDifficulty.color = ThemesManager.Instance.CurrentTheme.ThemeFontColor;
            levelDifficulty.text = LanguageManager.Get("level") + " " + (t + 1) + (difficultLevelsInfo.Count > t ? " - " + difficultLevelsInfo[t] : "");
            for (int i = 0; i < _difficultyDots.Length; i++)
            {
                if (i <= t)
                {
                    _difficultyDots[i].overrideSprite = ThemesManager.Instance.CurrentTheme.DotSelected;
                }
                else
                {
                    _difficultyDots[i].overrideSprite = ThemesManager.Instance.CurrentTheme.DotUnselected;
                }

            }
        }
    }
}