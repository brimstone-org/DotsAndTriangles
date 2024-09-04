using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core.Command;

namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "Mover", menuName = "Dots Triangles/Mover Base")]
    public class Mover : ScriptableObject
    {
        public event System.Action evtNameUpdated;
        [SerializeField]
        string _playerName = "Player";
        Ticker _ticker;
        PlayerType _playerType;

        protected bool _isActive;
        protected bool _allowNextMove;
        protected ActionsManager _actionManager;
        bool _isMultiplayer;
        Dots _dots;
       

        public virtual PlayerType PlayerType
        {
            get
            {
                return _playerType;
            }

            set
            {
                _playerType = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
            }
        }

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

        public Ticker Ticker
        {
            get
            {
                return _ticker;
            }

            set
            {
                _ticker = value;
            }
        }

        public virtual bool IsMultiplayer
        {
            get
            {
                return false;
            }
        }

        public string PlayerName
        {
            get
            {
                return _playerName;
            }
            set
            {
                _playerName = value;
                if (evtNameUpdated != null)
                    evtNameUpdated();
            }
        }

        public float Fill()
        {
            return GameManager.Single.Dots.FillFor(PlayerType);
        }

        public virtual void Init(Dots dots, ActionsManager am, Ticker ticker)
        {
            ActionManager = am;
            _dots = dots;
            //_actions = new List<Action>();
            _ticker = ticker;
        }

        protected virtual void StartTurn()
        {
            _isActive = true;
            _allowNextMove = true;

            //Debug.Log("Move + " + _playerType);
            PlayerStartTurn st = new PlayerStartTurn(Dots, PlayerType, _ticker.CurrentTick);
            _actionManager.Do(st);
        }

        protected virtual void EndTurn()
        {
            //Debug.Log("Next Player");
            PlayerEndTurn et = new PlayerEndTurn(Dots, PlayerType, _ticker.CurrentTick);
            _actionManager.Do(et);

            _isActive = false;
        }

        public virtual IEnumerator Move()
        {
           
            StartTurn();

            while (_allowNextMove)
                yield return null;
                

            EndTurn();
        }

        public virtual void MakeAction(IAction action)
        {
            _actionManager.Do(action);
            _allowNextMove = action.AllowNextMove();

            if (Dots.PlayerFillA > 0.51f || Dots.PlayerFillB > 0.51f)
                _allowNextMove = false;
        }

        public virtual void Undo()
        {
            _actionManager.Undo(PlayerType);
        }

        public virtual void Redo()
        {
            _actionManager.Redo(PlayerType);
        }


    }
}
