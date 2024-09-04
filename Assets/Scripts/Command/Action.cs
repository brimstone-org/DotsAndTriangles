using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;
using System;

namespace DotsTriangle.Core.Command
{
    [System.Serializable]
    public class Action : IAction
    {
        protected Dots _dots;
        protected PlayerType _playerType;
        protected int _tick;

        public PlayerType PlayerType
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

        public Action()
        {

        }

        public Action(Dots dots, PlayerType playerType, int tick)
        {
            _dots = dots;
            _playerType = playerType;
            _tick = tick;
        }

        public virtual bool AllowNextMove()
        {
            return true;
        }

        public virtual bool HasExtraMove()
        {
            return false;
        }

        public virtual bool IsAnimation()
        {
            return false;
        }
        
        public virtual void Do()
        {
            
        }

        public virtual void Redo()
        {
           
        }

        public virtual void Undo()
        {
            
        }

        public virtual void Deserialize(byte[] data)
        {

        }

        public virtual byte[] Serialize()
        {
            return new byte[] {};
        }

      
    }

}
