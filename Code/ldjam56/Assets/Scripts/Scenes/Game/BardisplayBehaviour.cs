using UnityEngine;

namespace Assets.Scripts
{
	public class BardisplayBehaviour : MonoBehaviour
	{
        [SerializeField]
        private RectTransform fill;

        public void UpdateDisplay(float percentage)
        {
            fill.anchorMax = new Vector2 (percentage, 1);
        }
	}
}
