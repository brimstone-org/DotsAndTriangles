using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Core;
using System;

namespace DotsTriangle.Core.Command
{
    public class PlayerEndTurn : Action
    {
        public static byte ID
        {
            get
            {
                return 1;
            }
        }

        public PlayerEndTurn()
        {

        }

        public PlayerEndTurn(Dots dots, PlayerType playerType, int tick) : base(dots, playerType, tick)
        {
        }

        public override string ToString()
        {
            return "[PlayerEnd | " + _playerType + " | Time: " + _tick + "]";
        }

        public override bool IsAnimation()
        {
            return true;
        }

        public override byte[] Serialize()
        {
            Byte[] tickInBytes = Utils.Convert.ConvertIntToByteArray(_tick);
            return new byte[] { ID, (byte)PlayerType, tickInBytes[0], tickInBytes[1], tickInBytes[2], tickInBytes[3] };
        }

        public override void Deserialize(byte[] data)
        {
            if (data.Length == 0)
                return;

            byte id = data[0];

            if (id == ID && data.Length >= 6)
            {
                PlayerType = (PlayerType)data[1];
                _tick = Utils.Convert.CovertByteArrayToInt(data, 2);
            }
        }
    }

}
