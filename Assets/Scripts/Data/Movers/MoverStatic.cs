using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DotsTriangle.Data;
using DotsTriangle.Core.Command;

namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "MoverStatic", menuName = "Dots Triangles/Mover Static")]
    public class MoverStatic : AIDumb
    {       
        public List<Vector4> moveList;

        int _lastMoveIndex = 0;

        public override void Init(Dots dots, ActionsManager am, Ticker ticker)
        {
            base.Init(dots, am, ticker);
            _lastMoveIndex = 0;
        }

        public override List<Command.Action> Think()
        {
            List<Command.Action> l = new List<Command.Action>();

            if (_lastMoveIndex > moveList.Count - 1)
                return l;


            Vector4 v = moveList[_lastMoveIndex];
            Dot a = Dots.GetDot((int)v.x, (int)v.y);
            Dot b = Dots.GetDot((int)v.z, (int)v.w);

            Command.SelectDot sd = new Command.SelectDot(Dots, PlayerType, a, Ticker.CurrentTick);
            Command.MakeLine dd = new Command.MakeLine(Dots, PlayerType, a, b, Ticker.CurrentTick);
            l.Add(sd);
            l.Add(dd);

            _lastMoveIndex++;
            return l;
        }

    }

}

