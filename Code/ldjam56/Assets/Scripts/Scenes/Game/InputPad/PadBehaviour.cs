using Assets.Scripts.Scenes.Game;

using UnityEngine;
using UnityEngine.InputSystem;

public class PadBehaviour : MonoBehaviour
{

    [SerializeField]
    private MoverBehaviour mover;

    private InputAction touchAction;

    private float radius;
    private Vector2 center;

    private void Start()
    {
        touchAction = InputSystem.actions.FindAction("Touch");
        center = transform.position;
        var rect = GetComponent<RectTransform>();
        System.Single x = rect.sizeDelta.x;
        this.radius = x * x;

    }

    // Update is called once per frame
    void Update()
    {
        if (touchAction.IsInProgress())
        {
            var touchPoint = touchAction.ReadValue<Vector2>();
            Debug.Log("Touch" + touchPoint);
            var diff = touchPoint - center;
            if (diff.sqrMagnitude > radius)
            {

                Debug.Log("innn" + touchPoint);
            }            
        }
    }
}
