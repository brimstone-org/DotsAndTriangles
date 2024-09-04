using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Core.Command
{
    public class MakeLine : Action
    {
        Dot _a;
        Dot _b;
        Line _line;
        List<Triangle> _triangles = new List<Triangle>();
        List<Dot> _dotsCovered = new List<Dot>();
        List<Dot> _dots360 = new List<Dot>();

        public static byte ID
        {
            get
            {
                return 4;
            }
        }

        public Line Line
        {
            get
            {
                return _line;
            }

            set
            {
                _line = value;
            }
        }

        public List<Triangle> Triangles
        {
            get
            {
                return _triangles;
            }

            set
            {
                _triangles = value;
            }
        }

        public List<Dot> DotsCovered
        {
            get
            {
                return _dotsCovered;
            }

            set
            {
                _dotsCovered = value;
            }
        }

        public Dot A
        {
            get
            {
                return _a;
            }

            set
            {
                _a = value;
            }
        }

        public Dot B
        {
            get
            {
                return _b;
            }

            set
            {
                _b = value;
            }
        }

        public MakeLine()
        {

        }

        public MakeLine(Dots dots, PlayerType playerType, Dot a, Dot b, int tick) : base(dots, playerType, tick)
        {
            _a = a;
            _b = b;
        }

        public override bool AllowNextMove()
        {
            return _triangles.Count > 0;
        }

        public override bool HasExtraMove()
        {
            return _triangles.Count > 0;
        }

        public override void Do()
        {
            _line = _dots.AddLine(_a, _b, _playerType);
            //Debug.Log("LINE - " + _line);
            if(_line != null)
            {
                _triangles = _dots.CheckForTriangle(_line);
                //Debug.Log("Triangles count " + _triangles.Count);
                _triangles.ForEach(t =>
                {
                    t.l1.isPartOfTriangle++;
                    t.l2.isPartOfTriangle++;
                    t.l3.isPartOfTriangle++;

                    List<Dot> newDots = _dots.AddTriangle(t, _dots360);

                    if (newDots != null)
                    {
                        _dotsCovered.AddRange(newDots);
                        newDots.ForEach(y =>
                        {
                            if (!_dots.DotCovered.Contains(y))
                                _dots.DotCovered.Add(y);
                        });
                    }

                    _dots360.ForEach(y =>
                    {
                        if (!_dots.Dots360.Contains(y))
                            _dots.Dots360.Add(y); 
                    });
                });
            }
        }

        public override void Undo()
        {
            if(_line != null)
            {
                _dots.Lines.Remove(_line);
                _triangles.ForEach(t => {
                    t.l1.isPartOfTriangle--;
                    t.l2.isPartOfTriangle--;
                    t.l3.isPartOfTriangle--;

                    _dots.RemoveTriangle(t);
                });

                _dotsCovered.ForEach(y => _dots.DotCovered.RemoveAll(k => k == y));

                _dots360.ForEach(y =>
                {
                    _dots.Dots360.Remove(y);
                });
            }
        }

        public override void Redo()
        {
            if (_line != null)
            {
                _dots.Lines.Add(_line);
                _triangles.ForEach(t => {
                    t.l1.isPartOfTriangle++;
                    t.l2.isPartOfTriangle++;
                    t.l3.isPartOfTriangle++;
                    _dots.AddTriangle(t, null);
                });

                _dotsCovered.ForEach(y => _dots.DotCovered.Add(y));

                _dots360.ForEach(y =>
                {
                    if (!_dots.Dots360.Contains(y)) _dots.Dots360.Add(y);
                });
            }
        }

        public override string ToString()
        {
            string s = "";
            _triangles.ForEach(y => s += y.ToString() + ",");
            return _playerType + "[Line: " + _a + " - " + _b  + " Triangles: " + s + " | Time: " + _tick + "]";
        }

        public override byte[] Serialize()
        {
            Byte[] tickInBytes = Utils.Convert.ConvertIntToByteArray(_tick);
            return new byte[] { ID, (byte)PlayerType, (byte)_a.x, (byte)_a.y, (byte)_b.x, (byte)_b.y, tickInBytes[0], tickInBytes[1], tickInBytes[2], tickInBytes[3] };
        }

        public override void Deserialize(byte[] data)
        {
            if (data.Length == 0)
                return;

            byte id = data[0];

            if (id == ID && data.Length >= 10)
            {
                PlayerType pt = (PlayerType)data[1];
                PlayerType = pt;

                int ax = data[2];
                int ay = data[3];

                _a = GameManager.Single.Dots.GetDot(ax, ay);
                //_a = new Dot(ax, ay);

                int bx = data[4];
                int by = data[5];
                _b = GameManager.Single.Dots.GetDot(bx, by);
                //_b = new Dot(bx, by);

                int tick = Utils.Convert.CovertByteArrayToInt(data, 6);
                _tick = tick;
            }
        }
    }
}
