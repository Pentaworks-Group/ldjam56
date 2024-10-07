using Assets.Scripts.Scenes.Game.Bee;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.InputPad
{
    public class LookRelayBehaviour : CommandRelayBehaviour
    {

        [SerializeField]
        private MoverBehaviour mover;

        public override void SetDirection(Vector3 direction)
        {
            mover.UpdateViewDirection(direction);
        }
        public override void Stop()
        {
            mover.StopViewing();
        }

    }
}