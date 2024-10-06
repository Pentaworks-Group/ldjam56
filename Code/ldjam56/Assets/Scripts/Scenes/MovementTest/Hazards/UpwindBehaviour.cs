using Assets.Scripts.Scenes.TerrainTest;

using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;

using UnityEngine;


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
