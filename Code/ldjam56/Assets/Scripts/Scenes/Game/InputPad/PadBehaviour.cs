using Assets.Scripts.Scenes.Game;

using UnityEngine;
using UnityEngine.InputSystem;

public class PadBehaviour : MonoBehaviour
{

    [SerializeField]
    private MoverBehaviour mover;

    private InputAction touchAction;

    private void Start()
    {
        touchAction = InputSystem.actions.FindAction("Touch");
    }

    // Update is called once per frame
    void Update()
    {
        if (touchAction.IsInProgress())
        {
            Debug.Log("Touch" + touchAction.ReadValue<Vector2>());
            Debug.Log("transform" + transform.position + " : ");
        }
    }
}
