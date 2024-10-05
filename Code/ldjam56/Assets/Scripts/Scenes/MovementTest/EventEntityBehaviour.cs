
using UnityEngine;

public class EventEntityBehaviour : MonoBehaviour
{
    [SerializeField]
    private WorldEventsBehaviour worldBehaviour;

    [SerializeField]
    private GameObject gotchaParticles;
    [SerializeField]
    private GameObject sphere;
    [SerializeField]
    private GameObject particles;


    private void OnCollisionEnter(Collision collision)
    {
        worldBehaviour.WasCaptured(this);
        gotchaParticles.SetActive(true);
        sphere.SetActive(false);
        particles.SetActive(false);
        Destroy(gameObject, 3f);
    }

  

}
