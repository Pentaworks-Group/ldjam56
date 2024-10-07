using Assets.Scripts.Scenes.Game.Bee;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Hazards
{
    public class UpwindBehaviour : HazardBaseBehaviour
    {
        [SerializeField]
        private MoverBehaviour mover;

        private Vector3 gravity = new Vector3(0, 5f, 0);


        protected override void OnEnter(Collider collision)
        {
            mover.SetGravity(gravity);
        }

        protected override void OnExit(Collider collision)
        {
            mover.SetGravity(-gravity);
        }

    }
}