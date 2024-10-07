using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game.Bee
{
	public class BoosterBehaviour : MonoBehaviour
	{
        [SerializeField]
        private BardisplayBehaviour bardisplayBehaviour;

        private InputAction boostAction;
        private MoverBehaviour moverBehaviour;

        private float remainingBoost = 10;
        private float maxBar = 10;
        private float boostConsumption = 1;
        private float boostStrength = 5;


        private void Awake()
        {
            moverBehaviour = GetComponent<MoverBehaviour>();

            boostAction = InputSystem.actions.FindAction("Boost");            
        }

        private void Start()
        {
            enabled = false;
            bardisplayBehaviour.UpdateDisplay(remainingBoost / maxBar);
        }

        private void OnEnable()
        {
            boostAction.performed += StartBoost;
            boostAction.canceled += StopBoost;
        }

        private void OnDestroy()
        {
            boostAction.performed -= StartBoost;
            boostAction.canceled -= StopBoost;
        }

        private void Update()
        {
            if (remainingBoost > 0)
            {
                remainingBoost -= boostConsumption * Time.deltaTime;
                bardisplayBehaviour.UpdateDisplay(remainingBoost/maxBar);
            }
            else
            {
                remainingBoost = 0;
                StopBoost(default);
            }
        }

        public void AddBoostPower(float power)
        {
            remainingBoost += power;
            bardisplayBehaviour.UpdateDisplay(remainingBoost / maxBar);
        }

        private void StartBoost(InputAction.CallbackContext context)
        {
            moverBehaviour.AdjustSpeed(boostStrength);
            enabled = true;
        }

        private void StopBoost(InputAction.CallbackContext context)
        {
            moverBehaviour.AdjustSpeed(1/boostStrength);
            enabled = false;
        }
    }
}
