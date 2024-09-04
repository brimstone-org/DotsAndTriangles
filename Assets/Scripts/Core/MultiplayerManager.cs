using DotsTriangle.Core.Command;
using DotsTriangle.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Localization;
using UnityEngine;

namespace DotsTriangle.Core
{
    public class MultiplayerManager : Photon.PunBehaviour
    {
        public string versionName = "0.1";
        public float timeForMove = 30;
        [Header("Fake")]
        public bool fakeIt;
        public float minWaitTime = 5;
        public float maxWaitTime = 20;
        public FloatType estimatedTime;
        public FloatType matchingTimePassed;

        public event System.Action<string> evtVerbose;
        public event System.Action evtConnecting;
        public event System.Action evtConnected;
        public event System.Action evtStartGame;
        public event System.Action<IAction> evtActionFromMultiplayer;
        

        static bool _isDisconected;
        bool _isConnected;
        double _startTime;
        bool _isFake;

        public void OnEnable()
        {
            GameManager.Single.evtPlayerWins += PlayerWins;
        }

        public void OnDisable()
        {
            GameManager.Single.evtPlayerWins -= PlayerWins;
        }

        public void Connect()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            PhotonNetwork.OnEventCall += OnEvent;

            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.ConnectUsingSettings(versionName);
                Debug.Log("Connecting...");

                _isConnected = true;
                // connect
                if (evtConnecting != null)
                    evtConnecting();
            }
            else
            {
                StartCoroutine(WaitUntilReadyForConnecting());
            } 
        }

        private IEnumerator WaitUntilReadyForConnecting()
        {
            bool _disconnect = false;
            while (true)
            {
                if (!PhotonNetwork.connected)
                {
                    PhotonNetwork.ConnectUsingSettings(versionName);
                    Debug.Log("Connecting...");

                    _isConnected = true;

                    if (evtConnecting != null)
                        evtConnecting();

                    yield break;

                }
                else
                {
                    if (!_disconnect)
                    {
                        PhotonNetwork.Disconnect();
                        _disconnect = true;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public bool IsMasterClient
        {
            get
            {
                if (_isFake)
                    return true;
                return PhotonNetwork.isMasterClient;
            }
        }

        public void Start()
        {
            matchingTimePassed.Value = 0;
            estimatedTime.Value = 0;
        }

        public override void OnConnectionFail(DisconnectCause cause)
        {
            Debug.Log(cause.ToString());
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            GameManager.Single.PlayerWins(GameManager.Single.IsNotMultiplayer(), 0, LanguageManager.Get("opDisc"));
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("E: Join Random Room");
            if (evtVerbose != null)
                evtVerbose(LanguageManager.Get("lobbyjoin"));
            PhotonNetwork.JoinRandomRoom();
        }

        public string PlayerBName()
        {
            if (_isFake)
                return GameManager.Single.GetTheOtherPlayer(GameManager.Single.IsNotMultiplayer()).PlayerName;

            PhotonPlayer[] players = PhotonNetwork.otherPlayers;
            if (players.Length > 0)
                return players[0].NickName;
            else
                return LanguageManager.Get("waiting");            
        }

        public string PlayerAName()
        {
            return PhotonNetwork.player.NickName;
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("E: OnConnectedToMaster");
            if (evtVerbose != null)
                evtVerbose(LanguageManager.Get("connecting"));

            Mover localPlayer = GameManager.Single.IsNotMultiplayer();
            PhotonNetwork.playerName = localPlayer.PlayerName;


            // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
            PhotonNetwork.JoinRandomRoom(null, 2);
        }

        public void PlayerWins(PlayerType pt, float ratio, string info)
        {
            Disconnect();
        }

        public void OnPhotonRandomJoinFailed()
        {
            Debug.Log("E: OnPhotonRandomJoinFailed");

            if (evtVerbose != null)
                evtVerbose(LanguageManager.Get("room"));

            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 2;

            PhotonNetwork.CreateRoom("Match#" + PhotonNetwork.countOfRooms, ro, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
          
            if (evtVerbose != null)
                evtVerbose(LanguageManager.Get("roomjoin"));

            if (evtConnected != null)
                evtConnected();

            StartCoroutine(WaitForTheOtherPlayer());

            if (evtVerbose != null)
                evtVerbose(LanguageManager.Get("waitingforplayer"));

            Debug.Log("Player: " + PhotonNetwork.player.ID + " / " + PhotonNetwork.countOfRooms +" x Room:" + PhotonNetwork.room.Name + " / count:" + PhotonNetwork.room.PlayerCount);
            Debug.Log("E: OnJoinedRoom");
        }

        private IEnumerator WaitForTheOtherPlayer()
        {
            float timePassed = 0;
            float timeToWait = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
            estimatedTime.Value = UnityEngine.Random.Range(minWaitTime, maxWaitTime);

            while (true)
            {              
                // __________________________________
                // fake it zone
                timePassed += Time.deltaTime;
                matchingTimePassed.Value = timePassed;

                yield return null;

                if (fakeIt && timePassed > timeToWait)
                {
                    GameManager.Single.FakePlay();
                    Disconnect();
                    _isFake = true;
                    GameManager.Single.evtPlayerChanged += StartTimer;

                    if (evtStartGame != null)
                        evtStartGame();

                    yield break;
                }

                if (PhotonNetwork.room != null && PhotonNetwork.room.PlayerCount == 2)
                {
                    GameManager.Single.evtPlayerChanged += StartTimer;

                    PhotonPlayer[] others = PhotonNetwork.otherPlayers;
                    if (others.Length > 0)
                    {
                        Mover opponent = GameManager.Single.GetTheOtherPlayer(GameManager.Single.IsNotMultiplayer());
                        opponent.PlayerName = others[0].NickName;
                    }

                    if (evtStartGame != null)
                        evtStartGame();

                    yield break;
                }
            }
        }

        public void StartTimer(PlayerType arg1, int arg2)
        {
            _startTime = PhotonNetwork.time;
            GameManager.Single.ticker.StartPlayerTimer(GameManager.Single.GetPlayer(arg1), timeForMove, GetServerTime, TimeOff);
        }

        private float GetServerTime()
        {
            float timePassed = (float)(PhotonNetwork.time - _startTime);

            //Debug.Log(timePassed);
            return timePassed;
        }

        private void TimeOff(Mover player)
        {
            if (player.IsMultiplayer)
                GameManager.Single.PlayerWins(GameManager.Single.GetTheOtherPlayer(player), 0, LanguageManager.Get("optime"));
            else
                GameManager.Single.PlayerWins(GameManager.Single.GetTheOtherPlayer(player), 0, LanguageManager.Get("youtime"));

        }

        public void SendMove(Command.IAction action, byte evCode)
        {
            PhotonNetwork.RaiseEvent(evCode, action.Serialize(), true, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }); 
        }

        public void NewAction(IAction a, ActionsManager.ActionType arg2)
        {
            if (_isFake)
                return;

            if (a.PlayerType != GameManager.Single.ActiveMover.PlayerType)
                return;

            if (GameManager.Single.ActiveMover.IsMultiplayer)
                return;

            byte code = 0;
            if (a.GetType() == typeof(SelectDot))
                code = SelectDot.ID;

            if (a.GetType() == typeof(DeselectDot))
                code = DeselectDot.ID;

            if (a.GetType() == typeof(PlayerStartTurn)) 
                code = PlayerStartTurn.ID;

            if (a.GetType() == typeof(PlayerEndTurn))
                code = PlayerEndTurn.ID;

            if (a.GetType() == typeof(MakeLine))
                code = MakeLine.ID;

            Debug.Log("SEND = " + a.ToString());
            SendMove(a, code);
        }

        void OnEvent(byte eventCode, object content, int senderId)
        {
            if (_isFake)
                return;

            if (!GameManager.Single.ActiveMover.IsMultiplayer)
                return;

            if(eventCode == SelectDot.ID)
            {
                SelectDot sd = new SelectDot();
                sd.Dots = GameManager.Single.Dots;
                sd.Deserialize((byte[])content);

                if (evtActionFromMultiplayer != null)
                {
                    evtActionFromMultiplayer(sd);
                }
                Debug.Log("RECEIVED = " + sd);
            }

            if (eventCode == DeselectDot.ID)
            {
                DeselectDot dd = new DeselectDot();
                dd.Dots = GameManager.Single.Dots;
                dd.Deserialize((byte[])content);

                
                if (evtActionFromMultiplayer != null)
                {
                    evtActionFromMultiplayer(dd);
                }
                Debug.Log("RECEIVED = " + dd);
            }

            if (eventCode == PlayerStartTurn.ID)
            {
                PlayerStartTurn pst = new PlayerStartTurn();
                pst.Dots = GameManager.Single.Dots;
                pst.Deserialize((byte[])content);

               
                if (evtActionFromMultiplayer != null)
                {
                    evtActionFromMultiplayer(pst);
                }
                Debug.Log("RECEIVED = " + pst);
            }

            if (eventCode == PlayerEndTurn.ID)
            {
                PlayerEndTurn pet = new PlayerEndTurn();
                pet.Dots = GameManager.Single.Dots;
                pet.Deserialize((byte[])content);

                if (evtActionFromMultiplayer != null)
                {
                    evtActionFromMultiplayer(pet);
                }
                Debug.Log("RECEIVED = " + pet);
            }

            if (eventCode == MakeLine.ID)
            {
                MakeLine ml = new MakeLine();
                ml.Dots = GameManager.Single.Dots;
                ml.Deserialize((byte[])content);

              
                if (evtActionFromMultiplayer != null)
                {
                    evtActionFromMultiplayer(ml);
                }


                Debug.Log("RECEIVED = " + ml);
                Debug.Log("DOTS - ");
                GameManager.Single.Dots.Lines.ForEach(y => Debug.Log(y));
                GameManager.Single.Dots.Triangles.ForEach(y => Debug.Log(y));
            }
        }

        private void Disconnect()
        {
            // Debug.Log("D1");
            if (_isConnected)
            {
                // Debug.Log("D2");
                PhotonNetwork.OnEventCall -= OnEvent;

                if (PhotonNetwork.connected)
                {
                    // Debug.Log("D3");
                    _isDisconected = true;
                    PhotonNetwork.Disconnect();
                }
            }

            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}
