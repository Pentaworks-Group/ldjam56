using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Assets.Scripts.Scenes.Game.InputPad
{
	public class ActionDisableBehaviour : MonoBehaviour
    {
        private PlayerInput input;
        private InputAction action;
        private string bindingString;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            action = input.actions["Look"];
            bindingString = action.bindings[1].path;
        }

        private void SetActionBinding()
        {
            if (Base.Core.Game.Options.ShowTouchPads)
            {
                action.ApplyBindingOverride(1, string.Empty);
            }
            else
            {
                action.ApplyBindingOverride(1, bindingString);
            }
        }

        private void OnEnable()
        {
            Base.Core.Game.OnOptionsEdited.AddListener(SetActionBinding);
            SetActionBinding();
        }

        private void OnDisable()
        {
            Base.Core.Game.OnOptionsEdited.RemoveListener(SetActionBinding);
        }


        //var actopm = input.actions["Look"];
        //actopm.ApplyBindingOverride(1, string.Empty);
    }
}
