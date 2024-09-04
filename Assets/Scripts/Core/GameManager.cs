using DotsTriangle.Core.Command;
using DotsTriangle.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using DotsTriangle.UI;
using UnityEngine;
using DotsTriangle.Utils;
using Localization;

namespace DotsTriangle.Core
{
    public class GameManager : ManagerMono<GameManager>
    {
        public static GameManager Instance;

        [Header("Settings")]
        public int width;
        public int height;
        public GameColors gameColors;
        public ColorType colorPlayerA;
        public ColorType colorPlayerB;
        public FloatType fillPlayerA;
        public FloatType fillPlayerB;
        public ISaveData data;
        public FloatType aiDifficulty;
        public Animator PauseMenu;
       
        [Header("Players")]
        public Mover playerA;
        public Mover playerB;
        public bool UndoAllowedForVideo; //if we can perform unlimited Undos for watching a video
        public int UndosCount; //how many times he clicked on undo

        public GameObject WatchRewardedVideoPanel;
        public bool GameOver;

        [Header("Movers")]
        public Mover forAI;
        public Mover forAIMultiplayerFake;
        public Mover forMultiplayer;
        public Mover forLocal;
        public Mover forLocalTutorial;
        public Mover forAITutorial;

        [Header("Ads")]
        public float timeToWaitForAds = 1.5f;

        [Header("Links")]
        public Ticker ticker;
        public UpdaterType uiUpdater;
        public MultiplayerManager multiplayerManager;

        [Header("Leaderboards")]
        public string multiplayerBoard;
        public string singleplayerBoard;
        public int pointForMultiplayer = 750;
        public int pointsForAILevel = 10;
        public int pointsForBoardSize = 10;

        public event System.Action<PlayerType, float, string> evtPlayerWins;
        public event System.Action<PlayerType, int> evtPlayerChanged;
        public event System.Action<PlayerType, int> evtExtraLine;
        public event System.Action evtIsMultiplayerGame;

        bool _isMultiplayer;
        bool _isPlayerA;
        ActionsManager _actionManager;
        Dots _dots;
        int _isSinglePlayer;

        public Dots Dots
        {
            get
            {
                return _dots;
            }

            set
            {
                _dots = value;
            }
        }

        public bool NewTutorialMarker;
        public bool IsTutorial
        {
            get
            {
                int tutorial = 0;
                int tutorial_force = 0;
                data.GetKey(Constants.TUTORIAL_DONE, ref tutorial, 0);
                data.GetKey(Constants.TUTORIAL_FORCE, ref tutorial_force, 0);
                return tutorial == 0 || tutorial_force == 1;
            }
        }

        public void ResetTutorialForced()
        {
            data.SetKey(Constants.TUTORIAL_FORCE, 0);
        }

        public bool IsMultiplayer
        {
            get 
            {
                return playerA.IsMultiplayer || playerB.IsMultiplayer;
            }
        }

        public ActionsManager ActionManager
        {
            get
            {
                return _actionManager;
            }

            set
            {
                _actionManager = value;
            }
        }

        public void TutorialDone()
        {
            data.SetKey(Constants.TUTORIAL_DONE, 1);
            data.SetKey(Constants.IS_SINGLEPLAYER, 1);
        }

        public bool IsLocal(Mover mover)
        {
            return mover == forLocal;
        }

        public void Awake()
        {
           
            data.SetKey(Constants.DUMMY, 0);
            if (Instance == null)
            {
                Instance = this;
            }
            int boardSize = width;
            _isSinglePlayer = 1;
            data.GetKey(Constants.IS_SINGLEPLAYER, ref _isSinglePlayer, _isSinglePlayer);

            // single player
            if (_isSinglePlayer == 1)
            {
                int isTutorialDone = 0;
                data.GetKey(Constants.TUTORIAL_DONE, ref isTutorialDone, isTutorialDone);

                if(!IsTutorial)
                {
                    playerA = forLocal;
                    SetNameForLocal(playerA, true);

                    playerB = forAI;
                    SetNameForAI(playerB);

                    data.GetKey(DotsTriangle.Constants.BOARD_SIZE, ref boardSize, boardSize);
                }
                else
                {
                    boardSize = 5;

                    playerA = forLocalTutorial;
                    SetNameForLocal(playerA, true);

                    playerB = forAITutorial;
                    SetNameForAI(playerB);
                } 
            }

            // multiplayer
            if (_isSinglePlayer == 0)
            {
                playerA = forLocal;
                SetNameForLocal(playerA, false);

                playerB = forMultiplayer;
                SetNameForMultiplayer(playerB);
            }

            width = (int)boardSize;
            height = (int)boardSize;

            ActionManager = new ActionsManager();
            ActionManager.ticker = ticker;

            Dots = new Dots(width, height, "Main");

            colorPlayerA.Value = gameColors.playerA;
            colorPlayerB.Value = gameColors.playerB;

            fillPlayerA.Value = 0;
            fillPlayerB.Value = 0;

            uiUpdater.Fire();

            playerA.PlayerType = PlayerType.PlayerA;
            playerB.PlayerType = PlayerType.PlayerB;

            playerA.Init(Dots, ActionManager, ticker);
            playerB.Init(Dots, ActionManager, ticker);

            playerA.IsActive = false;
            playerB.IsActive = false;

            playerA.evtNameUpdated += uiUpdater.Fire;
            playerB.evtNameUpdated += uiUpdater.Fire;


            if (playerA.IsMultiplayer || playerB.IsMultiplayer)
            {
                Debug.Log("Is Multiplayer");
                _isMultiplayer = true;
                multiplayerManager.evtStartGame += StartMultiplayerGame;
                ActionManager.evtNewAction += multiplayerManager.NewAction;
                ActionManager.evtExtraMove += multiplayerManager.StartTimer;
                multiplayerManager.Connect();
            }
            else
            {
                _isMultiplayer = false;
                _isPlayerA = true;
                StartCoroutine(MoveLoop());
            }
            //AdsManager.Instance.ConsecutiveGamesPlayed++;
        }

        private void SetNameForMultiplayer(Mover playerB)
        {

        }

        private void SetNameForAI(Mover p)
        {
            float difficulty = 1;

            data.GetKey(Constants.DIFFICULTY, ref difficulty, difficulty);
            p.PlayerName = "[AI] Lvl " + (difficulty + 1);

            Debug.Log(p.PlayerName);
        }

        private void SetNameForLocal(Mover playerA, bool v)
        {
            if (v)
                playerA.PlayerName = "You";
            else
            {
                string newName = "You";
                data.GetKey(Constants.LOCALPLAYER_NAME, ref newName, newName);
                playerA.PlayerName = newName;
            }
               
        }

        public void FakePlay()
        {
            playerB.evtNameUpdated -= uiUpdater.Fire;

            playerB = forAIMultiplayerFake;
            playerB.Init(Dots, ActionManager, ticker);
            playerB.PlayerType = PlayerType.PlayerB;
            playerB.IsActive = false;
            playerB.evtNameUpdated += uiUpdater.Fire;

            playerB.PlayerName = "Player#" + UnityEngine.Random.Range(0, 1000000);
            aiDifficulty.Value = UnityEngine.Random.Range(0f, 1f);

            _isMultiplayer = true;
            _isPlayerA = true;         
        }

        private void StartMultiplayerGame()
        {
            multiplayerManager.evtStartGame -= StartMultiplayerGame;
            if (multiplayerManager.IsMasterClient)
            {
                Debug.Log("Master Client");
                Debug.Log(playerA.PlayerType);
                _isPlayerA = true;
                //playerA.IsActive = true;
                //playerB.IsActive = false;

                playerA.PlayerType = PlayerType.PlayerA;
                playerB.PlayerType = PlayerType.PlayerB;
            }
            else
            {
                Debug.Log("NOT Master Client");
                Debug.Log(playerA.PlayerType);
                _isPlayerA = false;
                //playerA.IsActive = false;
                //playerB.IsActive = true;

                playerA.PlayerType = PlayerType.PlayerB;
                playerB.PlayerType = PlayerType.PlayerA;
            }

            StartCoroutine(MoveLoop());
        }


        public float GetFill(Mover player)
        {
            if (player.PlayerType == PlayerType.PlayerA)
                return Dots.PlayerFillA;
            else
                return Dots.PlayerFillB;
        }

        private IEnumerator MoveLoop()
        {
            ticker.StartTicker();
            while (true)
            {
                if (VerifyWinner())
                    break;

                if (_isPlayerA)
                {
                    if (evtPlayerChanged != null)
                    {
                        while (DotsVizualiser.Instance.CanDraw == false)
                        {
                            yield return null;
                           
                        }
                        evtPlayerChanged(playerA.PlayerType, ticker.CurrentTick);
                    }
                      
                    yield return playerA.Move();

                    if (VerifyWinner())
                        break;

                }
                else
                {
                    if (evtPlayerChanged != null)
                    {
                        
                        evtPlayerChanged(playerB.PlayerType, ticker.CurrentTick);
                    }
                       
                    yield return playerB.Move();

                    if (VerifyWinner())
                        break;
                }

                _isPlayerA = !_isPlayerA;
            }
        }

        private bool VerifyWinner()
        {

            if (Dots.PlayerFillA > 0.51f)
            {
                Mover winner = GetPlayer(PlayerType.PlayerA);
                string verb = LanguageManager.Get("opwins");
                if(winner == forLocal)
                {
                    //GiveLeaderPoints(winner, GetTheOtherPlayer(winner));
                    verb = LanguageManager.Get("youwin");
                }
                PlayerWins(winner, Dots.PlayerFillA, /*winner.PlayerName + " " +*/ verb + "!");
                return true;
            }

            if (Dots.PlayerFillB > 0.51f)
            {
                Mover winner = GetPlayer(PlayerType.PlayerB);
                string verb = LanguageManager.Get("opwins");
                if (winner == forLocal)
                {
                    //GiveLeaderPoints(winner, GetTheOtherPlayer(winner));
                    verb = LanguageManager.Get("youwin");
                }
                PlayerWins(winner, Dots.PlayerFillB, winner.PlayerName + " " + verb + "!");
                return true;
            }

            return false;
        }

        private void GiveLeaderPoints(Mover winner, Mover loser)
        {
        //    Debug.Log("TTTTT!");
        //    if (winner == forLocal && loser == forMultiplayer)
        //    {
        //        Debug.Log("GIVE POINTS!");
        //        GPGSManager.Instance.ReportScore(pointForMultiplayer, multiplayerBoard);
        //    }

        //    if (winner == forLocal && loser == forAI)
        //    {
        //        float difficulty = 1;
        //        data.GetKey(Constants.DIFFICULTY, ref difficulty, difficulty);
        //        GPGSManager.Instance.ReportScore(width * pointsForBoardSize + pointsForAILevel * (int)difficulty , singleplayerBoard);
        //    }
        }

        public void PlayerWins(Mover player, float covered, string reason)
        {
            StopAllCoroutines();
            ticker.StopAll();

            if (evtPlayerWins != null)
                evtPlayerWins(player.PlayerType, covered, reason);

            //StartCoroutine(ShowAds());
           
        }

        public void ShowRewardAd()
        {
        //   // Debug.Log("Showing Rewarded Ad");
        //    if (Rewards.Instance.IsAvailable())
        //    {
        //        //Debug.Log("Found available ad");
        //        Rewards.Instance.PlayAd();
        //        Rewards.OnAdCompleted += ResumeUndoAfterWatchingVideo;
        //        AdMobRewards.Instance.Cache(); //load next ad
        //    }
        //    else
        //    {
        //        WatchRewardedVideoPanel.SetActive(false);
        //        WatchRewardedVideoPanel.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        //    }
           
        }
        /// <summary>
        /// resumes game with undo after watching video
        /// </summary>
        private void ResumeUndoAfterWatchingVideo(string watchad)
        {
            WatchRewardedVideoPanel.SetActive(false);
            WatchRewardedVideoPanel.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            UndoAllowedForVideo = true;
           // Rewards.OnAdCompleted -= ResumeUndoAfterWatchingVideo;
            StartCoroutine(ShortDelayBeforeUndoAfterAd());
            // DotsVizualiser.Instance.Undo();
        }

        private IEnumerator ShortDelayBeforeUndoAfterAd()
        {
            yield return new WaitForSeconds(0.7f);
            DotsVizualiser.Instance.Undo();
        }
       

        //private IEnumerator ShowAds()
        //{
            
        //    //yield return new WaitForSeconds(timeToWaitForAds);
        //    //if (AdsManager.Instance.ConsecutiveGamesPlayed % 2 == 0)
        //    //{
        //    //    Debug.Log("Show interstitial");
        //    //    AdsManager.Instance.ConsecutiveGamesPlayed = 0;
        //    //    AdsManager.Instance.ShowAds();
        //    //}
          
        //    // Ads.Linkers.AdsIntegrator.Instance.ShowAds(null);
        //}

        public Mover IsNotMultiplayer()
        {
            if (!playerA.IsMultiplayer)
                return playerA;
            if (!playerA.IsMultiplayer)
                return playerB;
            return null;
        }

        public Mover GetTheOtherPlayer(Mover player)
        {
            if (playerA == player)
                return playerB;
            else
                return playerA;
        }

        public Mover GetPlayer(PlayerType player)
        {
            if (playerA.PlayerType == player)
                return playerA;
            else
                return playerB;
        }

        public Mover ActiveMover
        {
            get
            {
                if (_isPlayerA)
                    return playerA;
                else
                    return playerB;
            }
        }

        private void OnDestroy()
        {
            playerA.evtNameUpdated -= uiUpdater.Fire;
            playerB.evtNameUpdated -= uiUpdater.Fire;
        }
    }

}
