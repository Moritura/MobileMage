using System;
using System.Linq;
using UnityEngine;

namespace Draw
{
    public class InputData : MonoBehaviour
    {
        public event Action StartInputEvent;
        public event Action<Vector2> InputEvent;
        public event Action EndInputEvent;

        [SerializeField] private bool useLog;

        private void Start()
        {
            if (useLog)
            {
                StartInputEvent += () => Debug.Log("Start " + Input.mousePosition);
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
                StartInputEvent?.Invoke();
            }

            if (Input.GetMouseButton(0))
            {
                InputEvent?.Invoke(Input.mousePosition);
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
                StartInputEvent?.Invoke();
                InputEvent?.Invoke(previousTouch.Value.position);
            }
            else if (previousTouch != null)
            {
                Touch currentTouch = Input.touches.FirstOrDefault((x) => x.fingerId == previousTouch.Value.fingerId);
                if (currentTouch.fingerId == previousTouch.Value.fingerId)
                {
                    InputEvent?.Invoke(currentTouch.position);
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
