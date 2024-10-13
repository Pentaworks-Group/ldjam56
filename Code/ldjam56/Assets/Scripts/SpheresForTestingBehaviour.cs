using UnityEngine;

namespace Assets.Scripts
{
    public class SpheresForTestingBehaviour : MonoBehaviour
    {
        [SerializeField]
        int radius;

        [SerializeField]
        GameObject sphere;

        [SerializeField]
        GameObject cube;

        private void Start()
        {
            const int moar = 25;
            const int koorDist = 100;
            const int moarDist = koorDist / moar;
            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    var xPos = x * koorDist;
                    var t = Instantiate(cube);
                    System.Int32 yPos = y * koorDist;
                    t.transform.position = new Vector3(xPos, 6, yPos);
                    var xMoar = xPos + koorDist;
                    var yMoar = yPos + koorDist;
                    for (int sx = xPos + moarDist; sx < xMoar; sx += moarDist)
                    {
                        var tt = Instantiate(sphere);
                        tt.name = "smool";
                        tt.transform.position = new Vector3(sx, 6, yPos);
                    }
                    for (int sy = yPos + moarDist; sy < yMoar; sy += moarDist)
                    {
                        var tt = Instantiate(sphere);
                        tt.name = "smool";
                        tt.transform.position = new Vector3(xPos, 6, sy);
                    }
                }
            }


        }
    }
}
