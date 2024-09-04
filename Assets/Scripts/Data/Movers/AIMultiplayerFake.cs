using System.Collections;
using System.Collections.Generic;
using DotsTriangle.Core.Command;
using DotsTriangle.Data;
using UnityEngine;

namespace DotsTriangle.Core
{
    [CreateAssetMenu(fileName = "AIMultiplayerFake", menuName = "Dots Triangles/AI Multiplayer Fake")]
    public class AIMultiplayerFake : AIMixed
    {
        public Vector2 selectTime;
        public override float WaitTime(int type)
        {
            if(type == 0)
                base.WaitTime(type);

            if (type == 1)
                return UnityEngine.Random.Range(selectTime.x, selectTime.y);

            return base.WaitTime(type);
        }
    }
}
