
using UnityEngine;

public class EventEntityBehaviour : MonoBehaviour
{
    [SerializeField]
    private WorldEventsBehaviour worldBehaviour;


    private void OnCollisionEnter(Collision collision)
    {
        worldBehaviour.WasCaptured(this);   
        Destroy(gameObject);
    }

  

}
