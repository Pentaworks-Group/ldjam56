using Assets.Scripts.Scenes.TerrainTest;

using UnityEngine;


public class HazardBaseBehaviour : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        OnEnter(other);        
    }

    private void OnTriggerExit(Collider other)
    {
        OnExit(other);        
    }
    protected virtual void OnEnter(Collider other) { }
    protected virtual void OnExit(Collider other) { }
}
