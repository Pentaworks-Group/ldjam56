using Assets.Scripts.Scenes.Game.Bee;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Hazards
{
    public class DirectionalDraft : HazardBaseBehaviour
    {

        private Vector3 gravity;
        private float strength = 5f;

        private void Start()
        {
            Init();
        }

        protected override void OnEnter(Collider collision)
        {
            mover.SetGravity(gravity);
        }

        protected override void OnExit(Collider collision)
        {
            mover.SetGravity(-gravity);
        }

        public override void Init()
        {
            var rand = Random.Range(0, 3);
            transform.localScale += new Vector3(rand, rand, rand);
            transform.rotation = Random.rotation;
            gravity = transform.forward * strength;
        }

    }
}