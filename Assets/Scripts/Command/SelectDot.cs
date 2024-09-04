using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;
using System;

namespace DotsTriangle.Core.Command
{
    public class SelectDot : Action
    { 
        protected Dot _dot;

        public static byte ID
        {
            get
            {
                return 2;
            }
        }

        public SelectDot()
        {

        }

        public SelectDot(Dots dots, PlayerType playerType, Dot dot, int tick) : base(dots, playerType, tick)
        {
            _dot = dot;
        }

        public Dot Selected
        {
            get
            {
                return _dot;
            }

            set
            {
                _dot = value;
            }
        }

        public override bool IsAnimation()
        {
            return true;
        }

        public override string ToString()
        {
            return "[SelectDot | " + PlayerType + " | " + _dot + " | Time: " + _tick + "]";
        }

        public override byte[] Serialize()
        {
            Byte[] tickInBytes = Utils.Convert.ConvertIntToByteArray(_tick);
            return new byte[] { ID, (byte)PlayerType, (byte)_dot.x, (byte)_dot.y, tickInBytes[0], tickInBytes[1], tickInBytes[2], tickInBytes[3] };
        }

        public override void Deserialize(byte[] data)
        {
            if (data.Length == 0)
                return;

            byte id = data[0];

            if (id == ID && data.Length >= 8)
            {
                PlayerType pt = (PlayerType)data[1];
                int x = data[2];
                int y = data[3];

                int tick = Utils.Convert.CovertByteArrayToInt(data, 4);
                PlayerType = pt;

                _dot = new Dot(x, y);
                _tick = tick;
            }
        }
    }

}
