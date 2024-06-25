using Mage.Common;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Mage.Draw
{
    public class DrawController : MonoBehaviour
    {
        [SerializeField] private float _drawingDistance = 5f;

        private IDrawServiceInternal _drawService;

        private Dictionary<int, LineRenderer> _touchLineRenderers = new Dictionary<int, LineRenderer>();
        private Dictionary<int, List<Vector3>> _touchPoints = new Dictionary<int, List<Vector3>>();

        private List<LineRenderer> _lineRenderers = new List<LineRenderer>();

        private bool _isBlock = false;

        private void Update()
        {
            if (!_isBlock)
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    BeginDrawing(Input.mousePosition, 0);
                }
                else if (Input.GetMouseButton(0))
                {
                    AddPoint(Input.mousePosition, 0);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    EndDrawing(0);
                }
#else
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {
                        BeginDrawing(touch.position, touch.fingerId);
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        AddPoint(touch.position, touch.fingerId);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        EndDrawing(touch.fingerId);
                    }
                }
#endif
            }
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        [Inject]
        private void SetDependencies(IDrawServiceInternal drawService)
        {
            _drawService = drawService;
            Subscribe();
        }

        private void Subscribe()
        {
            if (_drawService != null)
            {
                _drawService.BlockDrawEvent += OnBlockDraw;
                _drawService.ClearDrawEvent += OnClearDraw;
            }
        }

        private void Unsubscribe()
        {
            if (_drawService != null)
            {
                _drawService.BlockDrawEvent -= OnBlockDraw;
                _drawService.ClearDrawEvent -= OnClearDraw;
            }
        }

        private void OnClearDraw()
        {
            foreach (var lr in _lineRenderers)
            {
                Destroy(lr.gameObject);
            }

            _lineRenderers.Clear();
        }

        private void OnBlockDraw(bool isBlock)
        {
            _isBlock = isBlock;
        }

        //private void BeginDrawing(Vector2 touchPosition)
        //{
        //    isDrawing = true;
        //    points.Clear();

        //    GameObject lineObj = new GameObject("Line");
        //    lineObj.transform.SetParent(transform);
        //    currentLineRenderer = lineObj.AddComponent<LineRenderer>();
        //    lineRenderers.Add(currentLineRenderer);

        //    AddPoint(touchPosition);
        //}

        //private void AddPoint(Vector2 touchPosition)
        //{
        //    if (!isDrawing) return;

        //    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        //    Vector3 worldPoint = ray.GetPoint(drawingDistance);

        //    if (points.Count > 0 && Vector3.Distance(points[points.Count - 1], worldPoint) < 0.01f) return;

        //    points.Add(worldPoint);

        //    currentLineRenderer.positionCount = points.Count;
        //    currentLineRenderer.SetPositions(points.ToArray());
        //}

        //private void EndDrawing()
        //{
        //    List<Vector2> list1 = new List<Vector2>()
        //    {
        //        new Vector2(0,0),
        //        new Vector2(1,0),
        //        new Vector2(1,1),
        //        new Vector2(0,1),
        //        new Vector2(0,0)
        //    };

        //    List<Vector2> list2 = points.ConvertAll(x => new Vector2(x.x, x.y));
        //    shapeComparison.CompareShapes(list1, list2);

        //    isDrawing = false;
        //    points.Clear();
        //}

        private void BeginDrawing(Vector2 touchPosition, int fingerId)
        {
            _drawService?.StartLine();

            GameObject lineObj = new GameObject($"Line_{fingerId}");
            lineObj.transform.SetParent(transform);
            LineRenderer newLineRenderer = lineObj.AddComponent<LineRenderer>();
            _touchLineRenderers[fingerId] = newLineRenderer;

            List<Vector3> newPointsList = new List<Vector3>();
            _touchPoints[fingerId] = newPointsList;
            AddPoint(touchPosition, fingerId);
        }

        private void AddPoint(Vector2 touchPosition, int fingerId)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            Vector3 worldPoint = ray.GetPoint(_drawingDistance);

            List<Vector3> currentPoints = _touchPoints[fingerId];
            if (currentPoints.Count > 0 && Vector3.Distance(currentPoints[currentPoints.Count - 1], worldPoint) < 0.01f) return;

            currentPoints.Add(worldPoint);

            LineRenderer lr = _touchLineRenderers[fingerId];
            lr.positionCount = currentPoints.Count;
            lr.SetPositions(currentPoints.ToArray());
        }

        private void EndDrawing(int fingerId)
        {
            List<Vector3> currentPoints = _touchPoints[fingerId];

            _drawService?.InputLine(new LineData(currentPoints.ConvertAll(point => new Vector2(point.x, point.y))));

            _touchPoints.Remove(fingerId);
            _touchLineRenderers.Remove(fingerId);
        }
    }
}