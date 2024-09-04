using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Core.Command
{
    /// <summary>
    /// Handles command pattern of the game actions
    /// </summary>
    public class ActionsManager
    {
        public enum ActionType { DO, UNDO, REDO}
        public int maxActions;
        public Stack<IAction> stack;
        public Stack<IAction> undos;
        public event System.Action<IAction, ActionType> evtNewAction;
        public event System.Action<PlayerType, int> evtExtraMove;
        public Ticker ticker;

        protected int _index;

        public ActionsManager()
        {
            stack = new Stack<IAction>();
            undos = new Stack<IAction>();
        }

        public bool IsUndoAvailable()
        {
            return stack.Count > 0;
        }

        public bool IsRedoAvailable()
        {
            return undos.Count > 0;
        }

        public void Do(IAction action)
        {
            stack.Push(action);
            undos.Clear();
            action.Do();

            if (evtNewAction != null)
                evtNewAction(action, ActionType.DO);

            if (action.HasExtraMove() && evtExtraMove != null)
                evtExtraMove(action.PlayerType, ticker.CurrentTick);
        }

        public List<IAction> Undo(PlayerType pt)
        {
           
            if (IsUndoAvailable())
            {
                List<IAction> list = new List<IAction>();

                while (stack.Count > 0)
                {
                    IAction a = stack.Pop();
                    list.Add(a);
                    undos.Push(a);
                    a.Undo();
                    
                    if (evtNewAction != null)
                        evtNewAction(a, ActionType.UNDO);

                    if (a.PlayerType != pt || a.IsAnimation())
                        continue;
                    else
                        return list;
                }
              
                return list;
            }
            return null;
        }

        // TO-DO: bug; redo not working always properly
        public List<IAction> Redo(PlayerType pt)
        {
            if (IsRedoAvailable())
            {
                List<IAction> list = new List<IAction>();
                while(undos.Count > 0)
                {
                    IAction a = undos.Pop();
                    list.Add(a);
                    stack.Push(a);
                    a.Redo();
                    
                    if (evtNewAction != null)
                        evtNewAction(a, ActionType.REDO);

                    if (a.PlayerType == pt || a.IsAnimation())
                        continue;
                    else
                        return list;
                }
               
                return list;
            }

            return null;
        }
    }
}

