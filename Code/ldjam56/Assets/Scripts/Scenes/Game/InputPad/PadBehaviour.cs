using Assets.Scripts.Scenes.Game;

using UnityEngine;
using UnityEngine.InputSystem;

public class PadBehaviour : MonoBehaviour
{

    [SerializeField]
    private MoverBehaviour mover;

    private InputAction touchAction;

    private GameObject knob;

    private float radius;
    private Vector2 center;

    private bool isWorking = false;

    private void Awake()
    {
        knob = transform.Find("Knob").gameObject;
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
        if (touchAction.IsPressed())
        {
            var touchPoint = touchAction.ReadValue<Vector2>();
            var diff = touchPoint - center;
            if (diff.sqrMagnitude < radius)
            {
                isWorking = true;
                knob.transform.localPosition = diff;
                mover.UpdateMoveDirection(new Vector3(diff.x, 0, diff.y));

            }
            else if (isWorking)
            {
                isWorking = false;
                mover.StopMoving();
                knob.transform.localPosition = Vector3.zero;
            }
        }
        else if (isWorking)
        {
            isWorking = false;
            mover.StopMoving();
            knob.transform.localPosition = Vector3.zero;
        }
    }
}
