using System;
using System.Collections.Generic;

using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Bee
{
    public class BeeColliderBehaviour : MonoBehaviour
    {
        private readonly static IList<String> bonks = new List<String>()
        {
            "Bonk_1",
            "Bonk_2"
        };

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != 9)
            {
                if (collision.relativeVelocity.magnitude > 10)
                {
                    GameFrame.Base.Audio.Effects.Play("Button_Squash");

                    Debug.LogWarning("Game Over!");

                    //var mover = GetComponent<MoverBehaviour>();
                    //mover.enabled = false;
                    //gameObject.SetActive(false);
                    //Destroy(gameObject);
                }
                else
                {
                    GameFrame.Base.Audio.Effects.Play(bonks.GetRandomEntry());
                }
            }
        }
    }
}
