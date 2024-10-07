using Assets.Scripts.Scenes.Menues;

using UnityEngine;

namespace Assets.Scripts.Scenes.Credits
{
    public class ClickableBehaviour : MainMenuBaseBehaviour
    {
        public Transform target;

        Ray ray;
        RaycastHit hit;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == target)
                    {
                        ToMainMenu();
                    }
                }
            }
        }
    }
}
