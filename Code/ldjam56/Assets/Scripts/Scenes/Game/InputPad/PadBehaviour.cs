using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Assets.Scripts.Scenes.Game.InputPad
{
    public class PadBehaviour : MonoBehaviour
    {

        private GameObject knob;
        private CommandRelayBehaviour commandRelayBehaviour;

        private float radius;
        private Vector2 center;

        private bool isWorking = false;
        private Finger currentFinger;

        private void Awake()
        {
            knob = transform.Find("Knob").gameObject;
            commandRelayBehaviour = GetComponent<CommandRelayBehaviour>();
        }

        private void Start()
        {
            center = transform.position;
            var rect = GetComponent<RectTransform>();
            System.Single x = rect.sizeDelta.x;
            this.radius = x * x;

        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += HandleFingerDown;
            ETouch.Touch.onFingerUp += HandleLoseFinger;
            ETouch.Touch.onFingerMove += HandleFingerMove;
        }

        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= HandleFingerDown;
            ETouch.Touch.onFingerUp -= HandleLoseFinger;
            ETouch.Touch.onFingerMove -= HandleFingerMove;
            EnhancedTouchSupport.Disable();
        }

        private void HandleFingerDown(Finger touchingFinger)
        {
            if (currentFinger == null)
            {
                var touchPoint = touchingFinger.screenPosition;
                var diff = touchPoint - center;
                if (diff.sqrMagnitude < radius)
                {
                    isWorking = true;
                    currentFinger = touchingFinger;
                    knob.transform.localPosition = diff;
                    commandRelayBehaviour.SetDirection(new Vector3(diff.x, 0, diff.y));

                }
            }
        }

        private void HandleLoseFinger(Finger lostFinger)
        {
            if (lostFinger == currentFinger)
            {
                Clear();
            }
        }

        private void HandleFingerMove(Finger touchingFinger)
        {
            if (touchingFinger == currentFinger)
            {
                UpdatePosition(touchingFinger);
            }
        }

        private void UpdatePosition(Finger touchingFinger)
        {
            var touchPoint = touchingFinger.screenPosition;
            var diff = touchPoint - center;
            if (diff.sqrMagnitude < radius)
            {
                isWorking = true;
                knob.transform.localPosition = diff;
                commandRelayBehaviour.SetDirection(new Vector3(diff.x, 0, diff.y));

            }
            else if (isWorking)
            {
                Clear();
            }
        }

        private void Clear()
        {
            currentFinger = null;
            isWorking = false;
            commandRelayBehaviour.Stop();
            knob.transform.localPosition = Vector3.zero;
        }
    }
}