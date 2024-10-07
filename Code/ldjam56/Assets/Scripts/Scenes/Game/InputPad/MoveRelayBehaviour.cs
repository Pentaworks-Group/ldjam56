using Assets.Scripts.Scenes.Game.Bee;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.InputPad
{
    public class MoveRelayBehaviour : CommandRelayBehaviour
    {

        [SerializeField]
        private MoverBehaviour mover;

        public override void SetDirection(Vector3 direction)
        {
            mover.UpdateMoveDirection(direction);
        }
        public override void Stop()
        {
            mover.StopMoving();
        }

    }
}