using Assets.Scripts.Scenes.Game.Bee;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class PlayerInputBehaviour : MonoBehaviour
    {
        [SerializeField]
        private MoverBehaviour mover;

        private InputAction moveAction;
        private InputAction moveActionNP;

        private bool isMoving = false;

        private InputAction lookAction;
        private InputAction lookActionNP;

        private bool isLooking = false;



        private void Start()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            moveActionNP = InputSystem.actions.FindAction("MoveNP");
            lookAction = InputSystem.actions.FindAction("Look");
            lookActionNP = InputSystem.actions.FindAction("LookNP");


            var clickAction = InputSystem.actions.FindAction("Click");
            clickAction.performed += LockMouse;
        }

        private void Update()
        {
            if (moveAction.IsInProgress() || moveActionNP.IsInProgress())
            {
                UpdateMoveDirection(default);
                isMoving = true;
            }
            else if (isMoving)
            {
                mover.StopMoving();
                isMoving = false;
            }
            if (lookAction.IsInProgress() || lookActionNP.IsInProgress())
            {
                var lookActionValue = lookAction.ReadValue<Vector2>();
                var lookActionNPValue = lookActionNP.ReadValue<Vector2>();

                mover.UpdateViewDirection(new Vector3(lookActionValue.x, lookActionNPValue.y, lookActionValue.y));
                isLooking = true;
            }
            else if (isLooking)
            {
                mover.StopViewing();
                isLooking = false;
            }
        }

        private void UpdateMoveDirection(InputAction.CallbackContext context)
        {
            var moveActionValue = moveAction.ReadValue<Vector2>();
            var moveActionNPValue = moveActionNP.ReadValue<Vector2>();
            mover.UpdateMoveDirection(new Vector3(moveActionValue.x, moveActionNPValue.y, moveActionValue.y));
        }

        private void LockMouse(InputAction.CallbackContext context)
        {
            if (!Base.Core.Game.Options.ShowTouchPads && !EventSystem.current.IsPointerOverGameObject())
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}