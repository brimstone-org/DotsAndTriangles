using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DotsTriangle.Data;

namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "AIDumb", menuName = "Dots Triangles/AI Dumb")]
    public class AIDumb : Mover
    {
        public Vector2 thinkingRange;

        public virtual float WaitTime(int type)
        {
            return UnityEngine.Random.Range(thinkingRange.x, thinkingRange.y);
        }

        public override IEnumerator Move()
        {
            Debug.Log("NOW: " + PlayerName);

            StartTurn();
           
            //float waitTicks = UnityEngine.Random.Range(thinkingRange.x, thinkingRange.y);

            List<Command.Action> actions = Think();

            while(actions.Count > 0)
            {
                yield return new WaitForSeconds(WaitTime(0));

                for (int i = 0; i < actions.Count - 1; i++)
                {
                    ActionManager.Do(actions[i]);

                    if (Dots.PlayerFillA > 0.51f || Dots.PlayerFillB > 0.51f)
                        yield break;

                    yield return new WaitForSeconds(WaitTime(1));
                }

                ActionManager.Do(actions[actions.Count - 1]);

                if (Dots.PlayerFillA > 0.51f || Dots.PlayerFillB > 0.51f)
                    yield break;

                if (!actions[actions.Count - 1].AllowNextMove())
                    break;

                actions = Think();
            }

            EndTurn();
        }

        public virtual List<Command.Action> Think()
        {
            List<Command.Action> l = new List<Command.Action>();
            List<Dot> allowDots = Dots.GetAvailableDots();

            /*new List<Dot>();

            Dots.DotsList.ForEach(y => {
                if (!Dots.DotCovered.Contains(y) && !Dots.Dots360.Contains(y))
                    allowDots.Add(y);
            });
            */

            Randomize(allowDots);

            if (allowDots.Count > 0)
            {
                bool found = false;
                for (int d = 0; d < allowDots.Count; d++)
                {
                    if (found) break;

                    Dot d1 = allowDots[d];

                    Command.SelectDot sd = new Command.SelectDot(Dots, PlayerType, d1, Ticker.CurrentTick);

                    List<Dot> notAllowed = Dots.NotAllowed(d1);

                    for (int i = 0; i < allowDots.Count; i++)
                    {
                        Dot y = allowDots[i];
                        if (y != d1 && !notAllowed.Contains(y))
                        {
                            Command.MakeLine dd = new Command.MakeLine(Dots, PlayerType, d1, y, Ticker.CurrentTick);
                            l.Add(sd);
                            l.Add(dd);
                            found = true;
                            break;
                        }
                    }
                }

            }

            return l;

        }

        private void Randomize(List<Dot> a)
        {
            for(int i = a.Count - 1; i > 1; i--)
            {
                int k = UnityEngine.Random.Range(0, i + 1);
                Dot temp = a[i];
                a[i] = a[k];
                a[k] = temp;
            }
        }
    }

}

