using Assets.Scripts.Scenes.Game.Bee;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class WorldEventsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text scoreText;

        [SerializeField]
        private MoverBehaviour beeMover;

        [SerializeField]
        private MumbleBeeBehaviour mumbleBeeTemplate;

        [SerializeField]
        private WorldBehaviour worldBehaviour;

        [SerializeField]
        private GameObject mumbleBees;

        private int score = 0;
        private int requiredScoreForMumble = 10;


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

        public void BeeCameHome()
        {
            if (score >= requiredScoreForMumble)
            {
                score -= requiredScoreForMumble;
                UpdateScore();
                SpawnMumbleBee();
            }
        }

        public void SpawnMumbleBee()
        {
            var newMumble = Instantiate(mumbleBeeTemplate, mumbleBees.transform);

            newMumble.Init(worldBehaviour.homeHive);
        }


    }
}