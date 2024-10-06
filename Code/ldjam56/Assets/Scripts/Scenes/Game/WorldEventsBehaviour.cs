using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class WorldEventsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text scoreText;

        [SerializeField]
        private MoverBehaviour beeMover;

        private int score = 0;


        private void Start()
        {
            UpdateScore();
        }

        public void WasCaptured(EventEntityBehaviour entity)
        {
            score += 1;
            UpdateScore();
            beeMover.AddSpeedBoost(3, 5);
        }


        private void UpdateScore()
        {
            scoreText.text = score.ToString();
        }

        private void ChooseNewLocation()
        {
            int randX = Random.Range(-4, 4);
            int randY = Random.Range(-4, 4);
            var vector3 = new Vector3(randX, 0, randY);
            Debug.Log("new position: " + vector3);
            transform.position = vector3;
        }
    }
}