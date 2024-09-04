using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Utils;

namespace DotsTriangle.Core
{
    /// <summary>
    /// Class that handles the dots
    /// </summary>
    [System.Serializable]
    public class Dots
    {
        public int width;
        public int height;
        public string name;
        List<Dot> _dotsList;

        List<Line> _lines = new List<Line>();
        List<Triangle> _triangles = new List<Triangle>();
        List<Dot> _dotCovered = new List<Dot>();
        List<Dot> _dots360 = new List<Dot>();

        float playerFillA = 0;
        float playerFillB = 0;
        float totalArea;

        #region Properties
        public List<Dot> DotsList
        {
            get
            {
                return _dotsList;
            }

            set
            {
                _dotsList = value;
            }
        }

        public List<Line> Lines
        {
            get
            {
                return _lines;
            }

            set
            {
                _lines = value;
            }
        }

        public List<Dot> DotCovered
        {
            get
            {
                return _dotCovered;
            }

            set
            {
                _dotCovered = value;
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

        public float PlayerFillA
        {
            get
            {
                return playerFillA;
            }

            set
            {
                playerFillA = value;
            }
        }

        public float PlayerFillB
        {
            get
            {
                return playerFillB;
            }

            set
            {
                playerFillB = value;
            }
        }

        public List<Dot> Dots360
        {
            get
            {
                return _dots360;
            }

            set
            {
                _dots360 = value;
            }
        }
        #endregion

        #region Public Methods
        public Dots(int width, int height, string name = "Dots")
        {
            this.name = name;

            this.width = width;
            this.height = height;

            DotsList = new List<Dot>(width * height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {

                    Dot newDot = new Dot(i, j);
                    DotsList.Add(newDot);
                }
            }

            totalArea = TotalArea();
        }

        public Dots(Dots org, string name = "Dots(Coppy)")
        {
            this.name = name;

            width = org.width;
            height = org.height;
            _dotsList = new List<Dot>(org._dotsList);
            _lines = new List<Line>(org._lines);
            _triangles = new List<Triangle>(org._triangles);
            _dotCovered = new List<Dot>(org._dotCovered);

            playerFillA = org.playerFillA;
            PlayerFillB = org.PlayerFillB;
            totalArea = org.totalArea;
        }

        public float FillFor(PlayerType player)
        {
            if (player == PlayerType.PlayerA)
                return playerFillA;
            else
                return playerFillB;
        }

        public List<Dot> NotAllowed(Dot dot)
        {
            List<Dot> list = new List<Dot>();

            for (int i = 0; i < _dotsList.Count; i++)
            {
                Dot canditate = _dotsList[i];
                if (canditate == dot)
                    continue;

                for(int l = 0; l < _lines.Count; l++)
                {
                    Line line = _lines[l];

                    if (Intersection.DoIntersect(dot.GetPoint(), canditate.GetPoint(), line.a.GetPoint(), line.b.GetPoint()))
                    {
                        list.Add(canditate);
                    }
                }


                for (int x = - width / 2 - 1; x < width / 2 + 1; x++)
                {
                    for (int y = -height / 2 - 1; y < height / 2 + 1; y++)
                    {
                        if(!(x == 0  && y == 0))
                        {
                            
                            Vector2 offset = 1.5f * new Vector2(x, y);
                            Vector2 virtaulPoint = dot.GetPoint() + offset;
                            if (virtaulPoint.x >= 0 && virtaulPoint.x < width && virtaulPoint.y >= 0 && virtaulPoint.y < height)
                            {
                                if (Intersection.DoIntersect(dot.GetPoint(), canditate.GetPoint(), dot.GetPoint(), virtaulPoint))
                                {
                                    list.Add(canditate);
                                }
                            }
                           
                        }
                    }
                }
            }

            list.AddRange(_dotCovered);
            list.AddRange(_dots360);

            return list;
        }

        public List<Dot> Allowed(Dot d)
        {
            List<Dot> na = NotAllowed(d);
            List<Dot> a = new List<Dot>();
            DotsList.ForEach(y => { if (!na.Contains(y)) a.Add(y); });
            return a;
        }

        public List<Dot> GetAvailableDots()
        {
            List<Dot> available = new List<Dot>();
            DotsList.ForEach(y => { if (!DotCovered.Contains(y) && !Dots360.Contains(y)) available.Add(y); });

            return available;
        }

        public List<Triangle> CheckForTriangle(Line line)
        {
            List<Triangle> t = new List<Triangle>();

            for(int i = 0; i < _lines.Count; i++)
            {
                Line canditate = _lines[i];
                if (canditate == line)
                    continue;

                if(line.a == canditate.a)
                {
                    Line connection = AreDotsConnected(line.b, canditate.b);
                    if (connection != null)
                    {
                        Triangle newT = new Triangle(line, canditate, connection, line.playerId);
                        if (!t.Contains(newT))
                        {
                            t.Add(newT);
                        }
                    }
                }
                if(line.a == canditate.b)
                {
                    
                    Line connection = AreDotsConnected(line.b, canditate.a);
                    if (connection != null)
                    {

                        Triangle newT = new Triangle(line, canditate, connection, line.playerId);
                        if (!t.Contains(newT))
                        {
                            t.Add(newT);
                        }
                       
                    }
                }

                if (line.b == canditate.a)
                {
                    
                    Line connection = AreDotsConnected(line.a, canditate.b);
                    if (connection != null)
                    {
                        Triangle newT = new Triangle(line, canditate, connection, line.playerId);
                        if (!t.Contains(newT))
                        {
                            t.Add(newT);
                        }                       
                    }
                }

                if (line.b == canditate.b)
                {
                    
                    Line connection = AreDotsConnected(line.a, canditate.a);
                    if (connection != null)
                    {
                        Triangle newT = new Triangle(line, canditate, connection, line.playerId);
                        if (!t.Contains(newT))
                        {
                            t.Add(newT);
                        }
                     
                    }                       
                }
            }

            for(int i = t.Count - 1; i >=0; i--)
            {
                if (ContainsTriangles(t[i]))
                    t.RemoveAt(i);
            }

            return t;
        }

        public bool ContainsTriangles(Triangle t1)
        {
            for(int i = 0; i < _triangles.Count; i++)
            {
                Triangle t2 = _triangles[i];

                if(t2 != t1)
                {
                    if (InTriangle(t2.a, t1) && InTriangle(t2.b, t1) && InTriangle(t2.c, t1))
                        return true;
                }
                
            }
            return false;
        }

        public bool InTriangle(Dot d, Triangle t)
        {
            if (d == t.a || d == t.b || d == t.c)
                return true;

            return Intersection.PointInTriangle2(d.GetPoint(), t.a.GetPoint(), t.b.GetPoint(), t.c.GetPoint());
        }

        public List<Dot> AddTriangle(Triangle triangle, List<Dot> dots360)
        {
            List<Dot> newDotsCovered = new List<Dot>();
            if (!Triangles.Contains(triangle))
            {
                Triangles.Add(triangle);

                if (triangle.playerId == PlayerTypeToInt(PlayerType.PlayerA))
                {
                    playerFillA += ComputeArea(triangle) / totalArea;
                    //did the below if to fix tutorial after implementing over 0.51% win
                    int tutorial_force = 0;
                    GameManager.Single.data.GetKey(Constants.TUTORIAL_DONE, ref tutorial_force, 0);
                    if ((GameManager.Single.IsTutorial|| tutorial_force ==1) && playerFillA >= 0.5f)
                    {
                        playerFillA = 0.512f;
                    }
                }

                if (triangle.playerId == PlayerTypeToInt(PlayerType.PlayerB))
                {
                    playerFillB += ComputeArea(triangle) / totalArea;
                }

                List<Dot> coners = new List<Dot>() { triangle.a, triangle.b, triangle.c };

                coners.ForEach(y => {
                    if (Is360Covered(y) >= 360)
                    {
                        //Debug.Log("We have 360 dot - 0");
                        if (!newDotsCovered.Contains(y))
                        {
                            //Debug.Log("We have 360 dot - 1");
                            if (dots360 != null)
                            {
                                dots360.Add(y);
                            }
                            newDotsCovered.Add(y);
                        }

                    }
                });
               

                _dotsList.ForEach(y => 
                {
                  
                    if (y != triangle.a && y != triangle.b && y != triangle.c)
                    {
                        if (Intersection.PointInTriangle2(y.GetPoint(), triangle.a.GetPoint(), triangle.b.GetPoint(), triangle.c.GetPoint()))
                        {
                            //if(!newDotsCovered.Contains(y))
                            {
                                newDotsCovered.Add(y);
                            }
                        }
                    }
                });

                return newDotsCovered;
            }

            return null;
        }

        public void RemoveTriangle(Triangle t)
        {
            if (Triangles.Contains(t))
            {
                if (t.playerId == PlayerTypeToInt(PlayerType.PlayerA))
                {
                    playerFillA -= ComputeArea(t) / totalArea;
                }

                if (t.playerId == PlayerTypeToInt(PlayerType.PlayerB))
                {
                    playerFillB -= ComputeArea(t) / totalArea;
                }

                _triangles.Remove(t);
            }
        }

        public Line AddLine(Dot selectedDot, Dot dotClicked, PlayerType player)
        {
            Line newLine = new Line(selectedDot, dotClicked, PlayerTypeToInt(player));
            if (!_lines.Contains(newLine))
            {
                _lines.Add(newLine);
                return newLine;
            }
            else
            {
                return null;
            }
           
        }

        public List<Dot> DotsConnected()
        {
            List<Dot> list = new List<Dot>();
            for (int i = 0; i < _lines.Count; i++)
            {
                Line l = _lines[i];
                if (!list.Contains(l.a))
                    list.Add(l.a);

                if (!list.Contains(l.b))
                    list.Add(l.b);

            }
            return list;
        }

        public List<Dot> DotsConnectedWith(Dot dot)
        {
            List<Dot> list = new List<Dot>();
            for(int i = 0; i < _lines.Count; i++)
            {
                Line l = _lines[i];
                if (l.a == dot && !list.Contains(l.b))
                {
                    list.Add(l.b);
                    continue;
                }

                if (l.b == dot && !list.Contains(l.a))
                    list.Add(l.a);
            }

            return list;
        }

        public override string ToString()
        {
            string n = name + "\nDotsList Count:";

            n += _dotsList.Count;
            n += "\nLines: ";
            _lines.ForEach(y => n += y.ToString() + " ");
            n += "\nTriangles Count:" + _triangles.Count + "\n";
            _triangles.ForEach(y => n += y.ToString() + " ");
            return n;
        }
        #endregion

        #region Private Methods
        private float ComputeArea(Triangle t)
        {
            Vector3 v1 = t.c.GetPoint() - t.a.GetPoint();
            Vector3 v2 = t.b.GetPoint() - t.a.GetPoint();
            return 0.5f * Vector3.Cross(v1, v2).magnitude;
        }

        private float TotalArea()
        {
            Vector2 diag = _dotsList[_dotsList.Count - 1].GetPoint() - _dotsList[0].GetPoint();
            return diag.x * diag.y;
        }

        private Line AreDotsConnected(Dot a1, Dot a2)
        {
            for (int i = 0; i < _lines.Count; i++)
            {
                Line c = _lines[i];
                if ((a1 == c.a && a2 == c.b) || (a1 == c.b && a2 == c.a))
                {
                    return c;
                }
            }

            return null;
        }

        private float Is360Covered(Dot dot)
        {
            float r = 0;
            for(int i = 0; i < Triangles.Count; i++)
            {
                Triangle t = Triangles[i];
                
                if(t.a == dot)
                {
                    Vector2 v1 = t.c.GetPoint() - t.a.GetPoint();
                    Vector2 v2 = t.b.GetPoint() - t.a.GetPoint();
                    r += Vector2.Angle(v1, v2);
                    continue;
                }

                if (t.b == dot)
                {
                    Vector2 v1 = t.a.GetPoint() - t.b.GetPoint();
                    Vector2 v2 = t.c.GetPoint() - t.b.GetPoint();
                    r += Vector2.Angle(v1, v2);
                    continue;
                }

                if (t.c == dot)
                {
                    Vector2 v1 = t.a.GetPoint() - t.c.GetPoint();
                    Vector2 v2 = t.b.GetPoint() - t.c.GetPoint();
                    r += Angle(v1, v2);
                    continue;
                }

            }

            return r;
        }

        float Angle(Vector2 a, Vector2 b)
        {
            return (float)Math.Acos(Vector2.Dot(a.normalized, b.normalized)) * (180 / Mathf.PI);
           
        }

        List<ConnectedLine> ConnectedLines(Line l)
        {
            List<ConnectedLine> cl = new List<ConnectedLine>();

            for (int i = 0; i < _lines.Count; i++)
            {
                Line y = _lines[i];
                List<Triangle> tl = TrianglesThatHasLine(l);
                List<Triangle> ty = TrianglesThatHasLine(y);

                bool ok = true;
                for(int k = 0; k < tl.Count; k++)
                {
                    for (int p = 0; p < ty.Count; p++)
                    {
                        if(tl[k] == ty[p])
                        {
                            ok = false;
                            break;
                        }
                    }
                }

                if (!ok) break;

                if (l != y)
                {
                    if (l.a == y.a)
                    {
                        cl.Add(new ConnectedLine(l, y, l.a));
                        continue;
                    }

                    if (l.a == y.b)
                    {
                        cl.Add(new ConnectedLine(l, y, l.a));
                        continue;
                    }

                    if (l.b == y.a)
                    {
                        cl.Add(new ConnectedLine(l, y, l.b));
                        continue;
                    }

                    if (l.b == y.b)
                    {
                        cl.Add(new ConnectedLine(l, y, l.b));
                        continue;
                    }
                }
            }

            return cl;
        }
        #endregion

        #region Helpers
        public List<Triangle> TrianglesThatHasLine(Line line)
        {
            List<Triangle> l = new List<Triangle>();
            _triangles.ForEach(t => {
                if (t.HasLine(line))
                    l.Add(t);
            });

            return l;
        }

        public Dot GetDot(int i, int j)
        {
            return DotsList[i + j * width];
        }

        public void SetDot(int i, int j, Dot dot)
        {
            
            DotsList[i + j * width] = dot;
        }

        public virtual int PlayerTypeToInt(PlayerType player)
        {
            if (player == PlayerType.None)
                return -1;
            if (player == PlayerType.PlayerA)
                return 0;
            if (player == PlayerType.PlayerB)
                return 1;
            return -1;
        }

        public virtual PlayerType IntToPlayerType(int player)
        {
            if (player == -1)
                return PlayerType.None;
            if (player == 0)
                return PlayerType.PlayerA;
            if (player == 1)
                return PlayerType.PlayerB;
            return PlayerType.None;
        }

        public List<ConnectedLine> GetAlmostTriangles()
        {
            List<ConnectedLine> at = new List<ConnectedLine>();

            _lines.ForEach(l => {

                List<ConnectedLine> ncl = ConnectedLines(l);

               
                ncl.ForEach(n => {
                   
                    List<Dot> md = n.MissingLine();
                    //Debug.Log("PAIR:" + md[0] + "|" + md[1]);
                    List<Dot> allowed = NotAllowed(md[0]);
                    //allowed.ForEach(y => Debug.Log(y));
                    if(!allowed.Contains(md[1]))
                        at.Add(n);
                });

               
            });

            return at;
        }      
        #endregion
    }

    public enum PlayerType
    {
        PlayerA, PlayerB, None
    }

    [System.Serializable]
    public class Dot : ICloneable
    {
        public int x;
        public int y;
        //public int playerId;

        public Dot(int x, int y)
        {
            this.x = x;
            this.y = y;
            //this.playerId = playerId; 
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Dot d = (Dot)obj;

            return d.x == x && d.y == y;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public Vector2 GetPoint()
        {
            return new Vector2(x, y);
        }

        public override string ToString()
        {
            return "(" + x + "," + y+")";
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
    }

    [System.Serializable]
    public class Line : ICloneable
    {
        public Dot a;
        public Dot b;
        public int playerId;
        public int isPartOfTriangle = 0;

        public Line(Dot a, Dot b, int playerId)
        {
            this.a = a;
            this.b = b;
            this.playerId = playerId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Line l = (Line)obj;

            return (l.a == a && l.b == b) || (l.b == a && l.a == b);
        }

        public Vector2 Direction()
        {
            return (a.GetPoint() - b.GetPoint()).normalized;
        }

        public override int GetHashCode()
        {
            var hashCode = 75695798;
            hashCode = hashCode * -1521134295 + EqualityComparer<Dot>.Default.GetHashCode(a);
            hashCode = hashCode * -1521134295 + EqualityComparer<Dot>.Default.GetHashCode(b);
            hashCode = hashCode * -1521134295 + playerId.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            return MemberwiseClone();


        }
        public override string ToString()
        {
            return "[" + a.ToString() + "->" + b.ToString() + "]";
        }
    }

    [System.Serializable]
    public class Triangle : ICloneable
    {
        public Dot a;
        public Dot b;
        public Dot c;
        public Line l1;
        public Line l2;
        public Line l3;

        public List<Line> InsideLines = new List<Line>(); //this list holds the line that are inside the triangle

        public int playerId;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            
            Triangle p = (Triangle)obj;

            return (a == p.a || a == p.b || a == p.c) && 
                (b == p.a || b == p.b || b == p.c) && 
                (c == p.a || c == p.b || c == p.c);

        }
        public override string ToString()
        {
            return "[" + a + " " + b + " " + c + "]";
        }

        public override int GetHashCode()
        {
            var hashCode = -878747968;
            hashCode = hashCode * -1521134295 + EqualityComparer<Dot>.Default.GetHashCode(a);
            hashCode = hashCode * -1521134295 + EqualityComparer<Dot>.Default.GetHashCode(b);
            hashCode = hashCode * -1521134295 + EqualityComparer<Dot>.Default.GetHashCode(c);
            hashCode = hashCode * -1521134295 + playerId.GetHashCode();
            return hashCode;
        }

        public Triangle(Line l1, Line l2, Line l3, int playerId)
        {
            this.l1 = l1;
            this.l2 = l2;
            this.l3 = l3;
            this.playerId = playerId;

            a = l1.a;
            b = l1.b;

            if(l2.a == l1.a)
            {
                c = l2.b;
            }
            else
            {
                if(l2.a == l1.b)
                {
                    c = l2.b;
                }
                else
                {
                    c = l2.a;
                }
            }    
        }

        public bool HasLine(Line l)
        {
            return l == l1 || l == l2 || l == l3;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class ConnectedLine
    {
        public Line l1, l2;
        public Dot connected;

        public ConnectedLine(Line l1, Line l2, Dot connected)
        {
            this.l1 = l1;
            this.l2 = l2;
            this.connected = connected;
        }
        
        public List<Dot> MissingLine()
        {
            List<Dot> d = new List<Dot>();
            if (l1.a != connected)
                d.Add(l1.a);
            else
                d.Add(l1.b);

            if (l2.a != connected)
                d.Add(l2.a);
            else
                d.Add(l2.b);

            return d;
        }
    }
}

