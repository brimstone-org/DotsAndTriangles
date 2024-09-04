using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Core.Command
{
    public interface IAction
    {
        PlayerType PlayerType { get; set; }
        void Do();
        void Undo();
        void Redo();
        bool IsAnimation();
        bool AllowNextMove();
        bool HasExtraMove();
        
        void Deserialize(byte[] data);
        byte[] Serialize();
    }
}

