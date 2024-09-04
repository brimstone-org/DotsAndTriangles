using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotsTriangle.Data;
using UnityEngine.UI;
using DotsTriangle.Core;
using System;
using System.Linq;
using DotsTriangle.Core.Command;
using DotsTriangle.Utils;

namespace DotsTriangle.UI
{
    /// <summary>
    /// Handles the dots UI and comunication with the logic
    /// </summary>
    public class DotsVizualiser : MonoBehaviour
    {

        public static DotsVizualiser Instance;
        [Header("Data")]
        public GameColors gameColors;
        //public PlayerType player;
        public MeshCreator meshCreator;
        public GameManager gm;
        public UpdaterType uiUpdater;
        public FloatType fillPlayerA;
        public FloatType fillPlayerB;

        [Header("UIs")]
        public Image board;
        public float xPadding;
        public float yPadding;
        public GameObject btnUndo;
        public GameObject btnRedo;

        [Header("Containers")]
        public Transform linesContainer;
        public Transform triangleContainer;
        public Transform dotsContainer;
        public Transform boardContainer;


        [Header("Prefabs")]
        public UIDot dotDefaultPrefab;
        public LineRenderer lineRendererPrefab;
        public bool CanDraw=true;

        [Header("Animations")]
        public float drawLineSpeed;
        public float triangleFadeInTime;

        [Header("Sounds")]
        public PlaySound drawLineSound;
        public PlaySound triangleSound;
        public float drawLineTime = 0.7f;
        Dots _dots;
        float _boardWidth;
        float _boardHeight;
        float _xSpace;
        float _ySpace;

        bool _gameEnded;

        Vector3 _offset;
        Dot _selectedDot;
        Line _lastLine;
        Mover _localPlayer;

        List<Dot> _dotsFoundInlines;
        List<Dot> _intersection = new List<Dot>();
        ActionsManager _actionManager;
        Ticker _ticker;

        Dictionary<Dot, UIDot> _dotUIs;
        Dictionary<Triangle, GameObject> _triangleUIs;
        Dictionary<Line, GameObject> _linesUIs;

        public event System.Action evtVisualizerReady;

        public Mover LocalPlayer
        {
            get
            {
                return _localPlayer;
            }

            set
            {
                _localPlayer = value;
            }
        }

        public void OnEnable()
        {
            gm.evtPlayerWins += PlayerWins;
        }

        private void PlayerWins(PlayerType arg1, float arg2, string arg3)
        {
            _gameEnded = true;
            _dots.DotsList.ForEach(y => {
                _dotUIs[y].EnableClick(false);
                _dotUIs[y].Show(true);
            });

        }

        public void OnDisable()
        {
            gm.evtPlayerWins -= PlayerWins;
        }

        public void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            gameColors.playerA = ThemesManager.Instance.CurrentTheme.PlayerOneColor;
            gameColors.playerB = ThemesManager.Instance.CurrentTheme.PlayerTwoColor;
            // init logic
            _dots = gm.Dots;

            _dotUIs = new Dictionary<Dot, UIDot>();
            _triangleUIs = new Dictionary<Triangle, GameObject>();
            _linesUIs = new Dictionary<Line, GameObject>();

            //
            _localPlayer = gm.playerA;

            _dotsFoundInlines = new List<Dot>();
            
            _ticker = gm.ticker;

            // ation manager
            _actionManager = gm.ActionManager;
            _actionManager.evtNewAction += NewAction;

            // board stuff
            _boardWidth = board.rectTransform.rect.width;
            _boardHeight = board.rectTransform.rect.height;
            Debug.Log(_dots.width + "////" + _dots.height);
            _xSpace = (_boardWidth - 2 * xPadding * _boardWidth) / (_dots.width - 1);
            _ySpace = (_boardHeight - 2 * yPadding * _boardHeight) / (_dots.height - 1);

            _offset = new Vector3(xPadding * _boardWidth - _boardWidth * .5f, yPadding * _boardHeight - _boardHeight * .5f, 0);
            //_offset = new Vector3(- _boardWidth * .5f, - _boardHeight * .5f, 0);


            //
            if (gm.IsMultiplayer)
            {
                btnUndo.SetActive(false);
                btnRedo.SetActive(false);

            }

            // create dots and put it on board
            _dots.DotsList.ForEach(y => CreateDefaultDot(y));

            if (evtVisualizerReady != null)
                evtVisualizerReady(); 
        }

        public Vector3 GetDotUIPos(int x, int y)
        {
            return _dotUIs[_dots.GetDot(x, y)].transform.position;
        }

        public UIDot GetDotUI(int x, int y)
        {
            return _dotUIs[_dots.GetDot(x, y)];
        }

        private void NewAction(IAction a, ActionsManager.ActionType t)
        {
            if(t == ActionsManager.ActionType.DO)
            {
                if(a.GetType() == typeof(SelectDot))
                {
                    DoSelectDot((SelectDot)a);
                }

                if(a.GetType() == typeof(MakeLine))
                {
                    MakeLine ml = (MakeLine)a;
                    if (ml.Line != null)
                        DoMakeLine(ml);
                }

                if(a.GetType() == typeof(DeselectDot))
                {
                    DoDeselectDot((DeselectDot)a);
                }
            }

            if(t == ActionsManager.ActionType.UNDO)
            {
                if (a == null) return;

                if (a.GetType() == typeof(MakeLine))
                {
                    MakeLine ml = (MakeLine)a;
                    DoDeselectDot(null);
                    UndoMakeLine(ml);
                    if (a.PlayerType == PlayerType.PlayerA && !GameManager.Single.IsMultiplayer)
                    {
                        GameManager.Single.UndosCount++;
                    }
                    
                }
            }

            if(t == ActionsManager.ActionType.REDO)
            {
                if (a == null) return;

                if (a.GetType() == typeof(MakeLine))
                {
                    MakeLine ml = (MakeLine)a;
                    DoDeselectDot(null);
                    DoMakeLine(ml);
                   
                }

                UpdateUI();
            }
        }

        public void Undo()
        {
            if (_gameEnded)
                return;
            //if (!GameManager.Single.IsMultiplayer)
            //{
                
            //    if (!GameManager.Single.UndoAllowedForVideo && GameManager.Single.UndosCount > 4)
            //    {
            //        GameManager.Single.WatchRewardedVideoPanel.SetActive(true);
            //        GameManager.Single.WatchRewardedVideoPanel.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            //        return;
            //    }
                
            //}
            _localPlayer.Undo();
        }


        public void Redo()
        {
            if (_gameEnded)
                return;

            _localPlayer.Redo();
        }
      

        /// <summary>
        /// Create a dot
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void CreateDefaultDot(Dot dot)
        {
            UIDot dotUI = Instantiate(dotDefaultPrefab, boardContainer);
            dotUI.Init(dot, gameColors, DotClicked);

            dotUI.transform.localPosition = new Vector3(dot.x * _xSpace, dot.y * _ySpace, 0) + _offset;
            dotUI.transform.parent = dotsContainer;
            _dotUIs.Add(dot, dotUI);
        }
        
        /// <summary>
        /// What happens when player A clicks a button
        /// </summary>
        /// <param name="obj"></param>
        public void DotClicked(UIDot obj)
        {
            if (!_localPlayer.IsActive ||CanDraw==false)
                return;

            Dot dotClicked = obj.Dot;
          //  Debug.Log("The player is " + _localPlayer.PlayerName);
          //  Debug.Log("The clicked dot is " + obj.Dot.GetPoint());
            // 3 cases
            // selected a new dot for 1st time
            if(_selectedDot == null && dotClicked != null)
            {
                SelectDot sd = new SelectDot(_dots, _localPlayer.PlayerType, dotClicked, _ticker.CurrentTick);
                _localPlayer.MakeAction(sd);
                return;
            }

            // dot is selected and you make the line
            if (_selectedDot != null && dotClicked != null && dotClicked != _selectedDot)
            {
                MakeLine ml = new MakeLine(_dots, _localPlayer.PlayerType, _selectedDot, dotClicked, _ticker.CurrentTick);
                _localPlayer.MakeAction(ml);
                return;
            }

            // selected the same point; disable it
            if (_selectedDot != null & dotClicked == _selectedDot)
            {
                DeselectDot dd = new DeselectDot(_dots, _localPlayer.PlayerType, _selectedDot, _ticker.CurrentTick);
                _localPlayer.MakeAction(dd);
                return;
            }           
        }
        
        /// <summary>
        /// Make line
        /// </summary>
        /// <param name="ml"></param>
        private void DoMakeLine(MakeLine ml)
        {
            if(_selectedDot != null)
            {
                _dotUIs[_selectedDot].IsSlected(false);
                _selectedDot = null;
            }

            // draw line
            // draw triangles
            List<IEnumerator> anims = new List<IEnumerator>();
            anims.Add(DrawLine(ml));
            anims.Add(DrawTriangles(ml));
            StartChainAnims(anims);

            ml.DotsCovered.ForEach(y => _dotUIs[y].Show(false));
            ShowUIDots(true);

            // Check if we need to update UI
            if (ml.Triangles.Count > 0)
            {
                UpdateUI();
            }
        }

        IEnumerator DrawTriangles(MakeLine ml)
        {
            while (CanDraw == false)
            {
                yield return null;
            }
            CanDraw = false;
            List<GameObject> objs = new List<GameObject>();
            List<Line> lines = new List<Line>();
            Color tColor = GetColor(ml.PlayerType);
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            for (int i = 0; i < ml.Triangles.Count; i++)
            {
                Triangle t = ml.Triangles[i];
                Vector3 a = _dotUIs[t.a].transform.position;
                Vector3 b = _dotUIs[t.b].transform.position;
                Vector3 c = _dotUIs[t.c].transform.position;

                a.z = triangleContainer.position.z;
                b.z = triangleContainer.position.z;
                c.z = triangleContainer.position.z;

                GameObject go = meshCreator.CreateTriangleObj(a, b, c, new Color(tColor.r, tColor.g, tColor.a, 0), ml.PlayerType);
             
                
                go.transform.parent = triangleContainer;
                objs.Add(go);

                lines.Add(t.l1);
                lines.Add(t.l2);
                lines.Add(t.l3);
                List<Vector2> listOfTrianglePoints = new List<Vector2>();
                if (!listOfTrianglePoints.Contains(t.l1.a.GetPoint()))
                {
                    listOfTrianglePoints.Add(t.l1.a.GetPoint());
                }
                if (!listOfTrianglePoints.Contains(t.l1.b.GetPoint()))
                {
                    listOfTrianglePoints.Add(t.l1.b.GetPoint());
                }
                if (!listOfTrianglePoints.Contains(t.l2.a.GetPoint()))
                {
                    listOfTrianglePoints.Add(t.l2.a.GetPoint());
                }
                if (!listOfTrianglePoints.Contains(t.l2.b.GetPoint()))
                {
                    listOfTrianglePoints.Add(t.l2.b.GetPoint());
                }
                if (!listOfTrianglePoints.Contains(t.l3.a.GetPoint()))
                {
                    listOfTrianglePoints.Add(t.l3.a.GetPoint());
                }
                if (!listOfTrianglePoints.Contains(t.l3.b.GetPoint()))
                {
                    listOfTrianglePoints.Add(t.l3.b.GetPoint());
                }
                foreach (var line in _linesUIs)
                {
                    //check if there is a line within this triangle
                    if (!lines.Contains(line.Key) && ContainsPoint(listOfTrianglePoints, line.Key.a.GetPoint()) && ContainsPoint(listOfTrianglePoints, line.Key.b.GetPoint()))
                    {
                        if (!t.InsideLines.Contains(line.Key))
                        t.InsideLines.Add(line.Key);//add the inside line.
                        line.Value.GetComponent<LineRenderer>().enabled = false;
                    }
                }
                _triangleUIs.Add(t, go);
                SetToTriangleContainer(t.l1);
                SetToTriangleContainer(t.l2);
                SetToTriangleContainer(t.l3);
                renderers.Add(go.GetComponent<MeshRenderer>());
            }

            StartCoroutine(AnimTriangles(objs, lines, tColor , renderers));
        }
        /// <summary>
        /// checks if a point is inside a triangle
        /// </summary>
        /// <param name="polyPoints"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool ContainsPoint(List<Vector2> polyPoints, Vector2 p)
        {
            var j = polyPoints.Count - 1;
            var inside = false;
            for (int i = 0; i < polyPoints.Count; j = i++)
            {
                var pi = polyPoints[i];
                var pj = polyPoints[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                    (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;

                if ((Mathf.FloorToInt(p.x) == Mathf.FloorToInt(pi.x) && Mathf.FloorToInt(p.y) == Mathf.FloorToInt(pi.y)) || (Mathf.FloorToInt(p.x) == Mathf.FloorToInt(pj.x) && Mathf.FloorToInt(p.y) == Mathf.FloorToInt(pj.y)))
                {
                    inside = true;
                }
            }
            return inside;
        }

        public static bool IsInPolygon(List<Vector2> poly, Vector2 p)
        {
            Vector2 p1, p2;
            bool inside = false;

            if (poly.Count < 3)
            {
                return inside;
            }

            var oldPoint = new Vector2(
                poly[poly.Count - 1].x, poly[poly.Count - 1].y);

            for (int i = 0; i < poly.Count; i++)
            {
                var newPoint = new Vector2(poly[i].x, poly[i].y);

                if (newPoint.x > oldPoint.x)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.x < p.x) == (p.x <= oldPoint.x)
                    && (p.y - (long)p1.y) * (p2.x - p1.x)
                    < (p2.y - (long)p1.y) * (p.x - p1.x))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }

        IEnumerator AnimTriangles(List<GameObject> objs, List<Line> lines, Color c, List<MeshRenderer> mrs)
        {
            if (objs.Count == 0)
            {
                CanDraw = true;
                yield break;
            }
                  

                if (triangleSound != null)
                    triangleSound.Play();

                lines.ForEach(y => SetLineColor(y, c));
                float timePassed = 0;
                Color initial = new Color(c.r, c.g, c.b, 0);
                Color final = c;
                while (timePassed < triangleFadeInTime)
                {
                    timePassed += Time.deltaTime;
                    Color i = Color.Lerp(initial, final, timePassed / triangleFadeInTime);
                    Color32 doodle = Color32.Lerp(new Color32(255,255,255,0), new Color32(255, 255, 255, 255), timePassed / triangleFadeInTime);
                        if ( (ThemesManager.Themes)PlayerPrefs.GetInt("themeIndex") != ThemesManager.Themes.Doodle)
                    {
                        objs.ForEach(y => meshCreator.SetColor(y, i));
                    }
                    else if (mrs.Count>0 && (ThemesManager.Themes) PlayerPrefs.GetInt("themeIndex") == ThemesManager.Themes.Doodle)
                    {
                        mrs.ForEach(y=>y.material.color = doodle);
                    }
                   
                    lines.ForEach(y => SetLineColor(y, c));
                    yield return new WaitForEndOfFrame();
                }
                CanDraw = true;
                lines.ForEach(y => SetLineColor(y, c));
         
           
        }

        void StartChainAnims(List<IEnumerator> anims)
        {
            StartCoroutine(Chain(anims));
        }

        IEnumerator Chain(List<IEnumerator> anims)
        {
            for (int i = 0; i < anims.Count; i++)
                yield return anims[i];
        }

        void SetToTriangleContainer(Line l1)
        {
            if (_linesUIs.ContainsKey(l1))
            {
                GameObject go = _linesUIs[l1];
                go.transform.parent = triangleContainer;
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0);
            }
        }

        void SetLineColor(Line l1, Color tColor)
        {
            if (_linesUIs.ContainsKey(l1))
            {
                GameObject go = _linesUIs[l1];
                LineRenderer lr = go.GetComponent<LineRenderer>();
                if(lr != null)
                {
                    lr.startColor = tColor;
                    lr.endColor = tColor;
                }
            }
        }

        /// <param name="ml"></param>
        void UndoMakeLine(MakeLine ml)
        {
            if (_linesUIs.ContainsKey(ml.Line))
            {
                GameObject go = _linesUIs[ml.Line];
                _linesUIs.Remove(ml.Line);
                Destroy(go);
            }


            ml.Triangles.ForEach(y => {
                if (_triangleUIs.ContainsKey(y))
                {
                    GameObject go = _triangleUIs[y];
                    foreach (var line in y.InsideLines)
                    {
                        _linesUIs[line].GetComponent<LineRenderer>().enabled = true;
                    }
                    _triangleUIs.Remove(y);
                    Destroy(go);

                    // TO-DO: search for other triangles that container this lines
                    RestoreLineColor(y.l1);
                    RestoreLineColor(y.l2);
                    RestoreLineColor(y.l3);

                }

            });

            ml.DotsCovered.ForEach(y => 
            {
                _dotUIs[y].Show(true);
                _dotUIs[y].EnableClick(true);
            });

            ShowUIDots(true);
            UpdateUI();
        }

        void RestoreLineColor(Line l)
        {
            List<Triangle> ts = _dots.TrianglesThatHasLine(l);
            //if (ts.Count == 0)
            //    SetLineColor(l, gameColors.lineColor);
            if(ts.Count >0)
            {
                ts.ForEach(k =>
                {
                    SetLineColor(k.l1, GetColor(_dots.IntToPlayerType(k.playerId)));
                    SetLineColor(k.l2, GetColor(_dots.IntToPlayerType(k.playerId)));
                    SetLineColor(k.l3, GetColor(_dots.IntToPlayerType(k.playerId)));

                });
            }
        }

        Color GetColor(PlayerType pt)
        {
            return pt == _localPlayer.PlayerType ? gameColors.playerA : gameColors.playerB;
        }

        private void UpdateUI()
        {
            fillPlayerA.Value = _dots.PlayerFillA;
            fillPlayerB.Value = _dots.PlayerFillB;

            uiUpdater.Fire();
        }

       

        /// <summary>
        /// Select the current dot
        /// </summary>
        /// <param name="sd"></param>
        private void DoSelectDot(SelectDot sd)
        {
            _selectedDot = sd.Selected;

            // make more gray
            _dotUIs[_selectedDot].IsSlected(true);

            // get list for invalid
            if (sd.PlayerType == _localPlayer.PlayerType)
                _intersection = _dots.NotAllowed(_selectedDot);
            else
                _intersection.Clear();
            ShowUIDots(false);

        }

        /// <summary>
        /// Deselect the current ui
        /// </summary>
        private void DoDeselectDot(DeselectDot dd)
        {
            if (_selectedDot != null)
                _dotUIs[_selectedDot].IsSlected(false);
            _selectedDot = null;

            ShowUIDots(true);
        }

        /// <summary>
        /// Draw a line
        /// </summary>
        /// <param name="newLine"></param>
        IEnumerator DrawLine(MakeLine ml)
        {
            // recolor the last line to default line color
            //if (_lastLine != null && _lastLine.isPartOfTriangle == 0)
            //    SetLineColor(_lastLine, gameColors.lineColor);

            while (CanDraw==false)
            {
                yield return null;
            }
            _lastLine = ml.Line;

            LineRenderer lr = Instantiate(lineRendererPrefab, linesContainer);
            lr.positionCount = 2;
            UIDot uiA = _dotUIs[ml.Line.a];
            UIDot uiB = _dotUIs[ml.Line.b];

            _linesUIs.Add(ml.Line, lr.gameObject);

         
            Vector3 start = new Vector3(uiA.transform.position.x, uiA.transform.position.y, linesContainer.position.z);
            Vector3 final = new Vector3(uiB.transform.position.x, uiB.transform.position.y, linesContainer.position.z);
           
            _dotsFoundInlines = _dots.DotsConnected();
            CanDraw = false;

            //return ;
            StartCoroutine(AnimDrawLine(start, final, lr, ml));
        }


        IEnumerator AnimDrawLine(Vector3 initial, Vector3 final, LineRenderer lr, MakeLine ml)
        {
           
           // Debug.Log("Drawing a line");
                if (drawLineSound != null)
                    drawLineSound.Play();
                lr.startColor = GetColor(ml.PlayerType);
                lr.endColor = GetColor(ml.PlayerType);

                //float timePassed = 0;
                lr.SetPosition(0, initial);
                Vector3 interm = initial;
                Vector3 velocity = Vector3.zero;
                float timePassed = 1f;
                if (ml.PlayerType == PlayerType.PlayerB && !GameManager.Instance.NewTutorialMarker)
                {
                //Debug.Log("Tutorial is active");
                    timePassed = 0.3f;
                }
                while (interm != final)
                {
                    // timePassed += Time.deltaTime;

                    lr.SetPosition(1, interm);
                    //interm = Vector3.Lerp(initial, final, timePassed/drawLineTime);
                    interm = Vector3.SmoothDamp(interm, final, ref velocity, Time.deltaTime * timePassed, drawLineSpeed);
                    yield return new WaitForEndOfFrame();
                }
            CanDraw = true;
                lr.SetPosition(1, final);
            
            

            //lr.startColor = gameColors.lineColor;
            //lr.endColor = gameColors.lineColor;
        }

        /// <summary>
        /// Shows or hides the list of invalid dots
        /// </summary>
        /// <param name="inlineDots"></param>
        /// <param name="show"></param>
        private void ShowUIDots(bool show)
        {
            _dots.DotsList.ForEach(y => {
                _dotUIs[y].EnableClick(true);
                _dotUIs[y].Show(true, true);
            });


            if (show)
            {
                _intersection.ForEach(y => {
                    _dotUIs[y].EnableClick(true);
                    _dotUIs[y].Show(true);
                });

              

                _dotsFoundInlines.ForEach(y => _dotUIs[y].Show(true, true));

                _dots.DotCovered.ForEach(y => {
                    _dotUIs[y].EnableClick(false);
                    _dotUIs[y].Show(false, true);
                });

                _dots.Dots360.ForEach(y => 
                {
                    _dotUIs[y].EnableClick(false);
                    _dotUIs[y].Show(false, true);
                });
               

                _intersection.Clear();
            }
            else
            {
                
              

                _dotsFoundInlines.ForEach(y => _dotUIs[y].Show(true, true));

                _intersection.ForEach(y =>
                {
                    _dotUIs[y].EnableClick(false);
                    _dotUIs[y].Show(false);
                });

                _dots.DotCovered.ForEach(y => {
                    _dotUIs[y].EnableClick(false);
                    _dotUIs[y].Show(false, true);
                });

                _dots.Dots360.ForEach(y =>
                {
                    _dotUIs[y].EnableClick(false);
                    _dotUIs[y].Show(false, true);
                });
            }
        }
    }

}
