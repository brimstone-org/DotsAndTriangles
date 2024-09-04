using System.Collections;
using System.Collections.Generic;
using DotsTriangle.Core.Command;
using DotsTriangle.Data;
using UnityEngine;

namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "AIMixed", menuName = "Dots Triangles/AI Mixed")]
    public class AIMixed : AIDumb
    {
        public AIDumb aiEasy;
        public AIDumb aiHard;
        public FloatType difficulty;

        public override PlayerType PlayerType
        {
            get
            {
                return base.PlayerType;
            }

            set
            {
                base.PlayerType = value;
                aiEasy.PlayerType = value;
                aiHard.PlayerType = value;
            }
        }

        public override void Init(Dots dots, ActionsManager am, Ticker ticker)
        {
            base.Init(dots, am, ticker);
            aiEasy.Init(dots, am, ticker);
            aiHard.Init(dots, am, ticker);
        }

        public override List<Action> Think()
        {
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (difficulty.Value >= 0.8f)
            {
                return aiHard.Think();
            }
            else
            {
                if (randomValue > difficulty.Value)
                    return aiEasy.Think();
                else
                    return aiHard.Think();
            }
           
        }
    }
}
