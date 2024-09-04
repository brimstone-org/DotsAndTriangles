using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DotsTriangle.Data;

namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "AIOportunistic", menuName = "Dots Triangles/AI Oportunistic")]
    public class AIOportunistic : AIDumb
    {
        // TO-DO: sometimes skips a triangle to close
        public override List<Command.Action> Think()
        {
            List<ConnectedLine> cl = Dots.GetAlmostTriangles();
            List<Command.Action> l = new List<Command.Action>();
            
            if (cl.Count > 0)
            {
                ConnectedLine a = cl[UnityEngine.Random.Range(0, cl.Count)];
                List<Dot> dots = a.MissingLine();

                Command.SelectDot sd = new Command.SelectDot(Dots, PlayerType, dots[0], Ticker.CurrentTick);               
                Command.MakeLine dd = new Command.MakeLine(Dots, PlayerType, dots[0], dots[1], Ticker.CurrentTick);

                l.Add(sd);
                l.Add(dd);

                return l;
            }

            return base.Think();
        }
    }

}

