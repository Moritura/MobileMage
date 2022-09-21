using System;
using System.Linq;
using UnityEngine;

namespace Draw
{
    public class InputData : MonoBehaviour
    {
        public event Action<Vector2Int> StartInputEvent;
        public event Action<Vector2Int> InputEvent;
        public event Action EndInputEvent;

        [SerializeField] private bool useLog;

        private void Start()
        {
            if (useLog)
            {
                StartInputEvent += (x) => Debug.Log("Start " + x);
                InputEvent += (x) => Debug.Log("Draw " + x);
                EndInputEvent += () => Debug.Log("End " + Input.mousePosition);
            }
        }

#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
        Touch? previousTouch = null;
#endif

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetMouseButtonDown(0))
            {
                StartInputEvent?.Invoke(new Vector2Int((int)Input.mousePosition.x, (int)Input.mousePosition.y));
            }

            if (Input.GetMouseButton(0))
            {
                InputEvent?.Invoke(new Vector2Int ((int)Input.mousePosition.x, (int)Input.mousePosition.y));
            }

            if (Input.GetMouseButtonUp(0))
            {
                EndInputEvent?.Invoke();
            }
#else
            // in phone
            // need test

            if (previousTouch == null && Input.touchCount > 0)
            {
                previousTouch = Input.touches[0];
                StartInputEvent?.Invoke(new Vector2Int ((int)previousTouch.position.x, (int)previousTouch.position.y));
                InputEvent?.Invoke(previousTouch.Value.position);
            }
            else if (previousTouch != null)
            {
                Touch currentTouch = Input.touches.FirstOrDefault((x) => x.fingerId == previousTouch.Value.fingerId);
                if (currentTouch.fingerId == previousTouch.Value.fingerId)
                {
                    InputEvent?.Invoke(new Vector2Int ((int)currentTouch.position.x, (int)currentTouch.position.y));
                    previousTouch = currentTouch;
                }
                else
                {
                    EndInputEvent?.Invoke();
                    previousTouch = null;
                }
            }
#endif
        }
    }
}
