using System;
using System.Collections;
using System.Collections.Generic;
using DotsTriangle.Core.Command;
using UnityEngine;


namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "Multiplayer Mover", menuName = "Dots Triangles/Multiplayer Mover")]
    public class MultiplayerMover : Mover
    {
        public override bool IsMultiplayer
        {
            get
            {
                return true;
            }
        }

        protected override void StartTurn()
        {
            base.StartTurn();
            GameManager.Single.multiplayerManager.evtActionFromMultiplayer += MakeAction;
        }

        protected override void EndTurn()
        {
            base.EndTurn();
            GameManager.Single.multiplayerManager.evtActionFromMultiplayer -= MakeAction;
        }

        public override void MakeAction(IAction obj)
        {
            base.MakeAction(obj);
            if (obj.GetType() == typeof(PlayerEndTurn) && obj.PlayerType == PlayerType)
            {
                _allowNextMove = false;
            }
        }
    }
}
