using UnityEngine;

namespace Assets.Scripts
{
	public class SpheresForTestingBehaviour : MonoBehaviour
	{
        [SerializeField]
        int radius;

        [SerializeField]
        GameObject sphere;

        private void Start()
        {
            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    var t = Instantiate(sphere);
                    t.transform.position = new Vector3(x * 100, 6, y * 100);    
                }
            }
        }
    }
}
