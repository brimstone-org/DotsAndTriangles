using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DotsTriangle.Data;
using DotsTriangle.Core.Command;


namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "AIStrategic", menuName = "Dots Triangles/AI Strategic")]
    public class AIStrategic : AIDumb
    {
        int calls = 0;
        public override List<Command.Action> Think()
        {
            List<Command.Action> acts = AnalyseMoves();
            //Debug.Log(acts.Count);
            if(acts != null)
            {
                Debug.Log(acts[0]);
                Debug.Log(acts[1]);

                return acts;
            }

            Debug.Log("Wrong!");
            return base.Think();
        }

        private List<Command.Action> AnalyseMoves()
        {
            calls = 0;
            Debug.Log(Dots);
            Dots copyDots = new Dots(Dots);
            ActionsManager am = new ActionsManager();

            MakeLine bestLine = null;

            // Can close?
            bestLine = CloseIfYouCan(copyDots, am);

            // find a good line
            if(bestLine == null)
            {
                float lastScore = float.MinValue;
                List<Dot> foras = copyDots.GetAvailableDots();
                Shuffle(foras);

                for (int i = 0; i < foras.Count; i++)
                {
                    Dot a = foras[i];

                    List<Dot> forbs = copyDots.Allowed(a);
                    Shuffle(forbs);
                    for (int k = 0; k < forbs.Count; k++)
                    {
                        Dot b = forbs[k];
                        if (b != a)
                        {
                            // make first line
                            MakeLine l = new MakeLine(copyDots, PlayerType, a, b, Ticker.CurrentTick);
                            am.Do(l);

                            float score = Evaluate(copyDots, am);
                            float canCloseScore = CanCloseOpponent(copyDots, a, b, am);

                            // exit early if opponent can't close
                            if (canCloseScore == 0)
                                return MakeActionList(l);

                            score += canCloseScore;

                            if (score > lastScore)
                            {
                                bestLine = l;
                                lastScore = score;
                            }
                            calls++;

                            while (am.IsUndoAvailable())
                                am.Undo(PlayerType);
                        }
                    }
                }
                
            Debug.Log("Best score " + lastScore);
            }
           

            Debug.Log("Calls " + calls);
            if(bestLine != null)
                return MakeActionList(bestLine);

            return null;
        }

        List<Command.Action> MakeActionList(MakeLine l)
        {
            List<Command.Action> c = new List<Core.Command.Action>();
            Dot a = l.Line.a;
            Dot b = l.Line.b;


            SelectDot sd = new Command.SelectDot(Dots, PlayerType, a, Ticker.CurrentTick);
            MakeLine ml = new Command.MakeLine(Dots, PlayerType, a, b, Ticker.CurrentTick);

            c.Add(sd);
            c.Add(ml);

            return c;
        }

        float PlayerFill(Dots d, PlayerType t)
        {
            return t == PlayerType.PlayerA ? d.PlayerFillA : d.PlayerFillB;
        }

        MakeLine CloseIfYouCan(Dots copyDots, ActionsManager am)
        {
            List<Dot> dil = new List<Dot>();

            copyDots.Lines.ForEach(y => {
                if(!copyDots.DotCovered.Contains(y.a) && !copyDots.DotCovered.Contains(y.b))
                {
                    dil.Add(y.a);
                    dil.Add(y.b);
                }
             
            });

            float fill = PlayerFill(copyDots, PlayerType);
            for (int i = 0; i < dil.Count; i++)
            {
                Dot a = dil[i];
                List<Dot> na = copyDots.NotAllowed(a);
                for (int k = 0; k < dil.Count; k++)
                {
                    Dot b = dil[k];
                    if (a == b)
                        continue;
                    if (na.Contains(b))
                        continue;
                    calls++;
                    MakeLine l = new MakeLine(copyDots, PlayerType, a, b, Ticker.CurrentTick);
                    am.Do(l);

                    if (fill < PlayerFill(copyDots, PlayerType))
                    {
                        am.Undo(PlayerType);
                        return l;
                    }
                    am.Undo(PlayerType);
                }
            }
            return null;
        }

        float CanCloseOpponent(Dots copyDots, Dot la, Dot lb, ActionsManager am)
        {
            PlayerType opponent = PlayerType == PlayerType.PlayerA ? PlayerType.PlayerB : PlayerType.PlayerA;

            List<Dot> dil = new List<Dot>();
            copyDots.Lines.ForEach(y => {
                dil.Add(y.a);
                dil.Add(y.b);
            });

            if (!dil.Contains(la) && !dil.Contains(lb))
                return 0;

            float worst = 0;
            float initialOpponentFill = PlayerFill(copyDots, opponent);
            for (int i = 0; i < 2; i++)
            {
                Dot a = i == 0 ? la : lb;
                List<Dot> na = copyDots.NotAllowed(a);
                for(int k = 0; k < dil.Count; k++)
                {
                    Dot b = dil[k];
                    if (a == b)
                        continue;
                    if (na.Contains(b))
                        continue;
                    calls++;
                    MakeLine l = new MakeLine(copyDots, opponent, a, b, Ticker.CurrentTick);
                    am.Do(l);

                    if(initialOpponentFill < PlayerFill(copyDots, opponent))
                    {
                       
                        float newWorst = initialOpponentFill - PlayerFill(copyDots, opponent);
                        if (newWorst < worst)
                            worst = newWorst;
                        //Debug.Log("Can Close:" + newWorst + " " + l);
                    }
                   
                    am.Undo(opponent);
                }
            }

            //Debug.Log("Calls 2:" + calls);
            return worst;
        }

        float Evaluate(Dots copyDots, ActionsManager am)
        {
            float score = 0;
            if(PlayerType == PlayerType.PlayerA)
            {
                score += copyDots.PlayerFillA;
                score -= copyDots.PlayerFillB;
            }
            else
            {
                score -= copyDots.PlayerFillA;
                score += copyDots.PlayerFillB;
            }

            return score;
        }

        void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}

