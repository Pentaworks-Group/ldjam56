using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game.InputPad
{
    public class PadBehaviour : MonoBehaviour
    {
        private InputAction touchAction;

        private GameObject knob;
        private CommandRelayBehaviour commandRelayBehaviour;

        private float radius;
        private Vector2 center;

        private bool isWorking = false;

        private void Awake()
        {
            knob = transform.Find("Knob").gameObject;
            commandRelayBehaviour = GetComponent<CommandRelayBehaviour>();
        }

        private void Start()
        {
            touchAction = InputSystem.actions.FindAction("Touch");
            center = transform.position;
            var rect = GetComponent<RectTransform>();
            System.Single x = rect.sizeDelta.x;
            this.radius = x * x;

        }

        void Update()
        {
            if (touchAction.IsInProgress())
            {
                var touchPoint = touchAction.ReadValue<Vector2>();
                var diff = touchPoint - center;
                if (diff.sqrMagnitude < radius)
                {
                    isWorking = true;
                    knob.transform.localPosition = diff;
                    commandRelayBehaviour.SetDirection(new Vector3(diff.x, 0, diff.y));

                }
                else if (isWorking)
                {
                    isWorking = false;
                    commandRelayBehaviour.Stop();
                    knob.transform.localPosition = Vector3.zero;
                }
            }
            else if (isWorking)
            {
                isWorking = false;
                commandRelayBehaviour.Stop();
                knob.transform.localPosition = Vector3.zero;
            }
        }
    }
}