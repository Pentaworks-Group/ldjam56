using UnityEngine;

namespace Assets.Scripts
{
    public class BardisplayBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RectTransform fill;

        public void UpdateDisplay(float percentage)
        {
            if (percentage > 1)
            {
                percentage = 1;
            }
            fill.anchorMax = new Vector2(percentage, 1);
        }
    }
}
