using UnityEngine;

namespace Assets.Scripts.Scenes.Game.InputPad
{
    public class PadEnablerBehaviour : MonoBehaviour
    {
        private GameObject pad;

        private void OnEnable()
        {
            Base.Core.Game.OptionsEditedEvent.AddListener(UpdatePadDisplay);
            pad = transform.GetChild(0).gameObject;
            UpdatePadDisplay();
        }

        private void OnDisable()
        {
            Base.Core.Game.OptionsEditedEvent.RemoveListener(UpdatePadDisplay);
        }

        private void UpdatePadDisplay()
        {

            pad.SetActive(Base.Core.Game.Options.ShowTouchPads);

        }
    }
}