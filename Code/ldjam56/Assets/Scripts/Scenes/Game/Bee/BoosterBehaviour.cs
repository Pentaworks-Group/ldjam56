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

        private Model.Bee bee;

        private void Awake()
        {
            moverBehaviour = GetComponent<MoverBehaviour>();

            boostAction = InputSystem.actions.FindAction("Boost");
        }

        private void Start()
        {
            enabled = false;
            Base.Core.Game.ExecuteAfterInstantation(OnInstantiated);
        }

        private void OnInstantiated()
        {
            bee = Base.Core.Game.State.Bee;
            bardisplayBehaviour.UpdateDisplay(bee.RemainingBoost / bee.MaxBar);
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
            if (bee.RemainingBoost > 0)
            {
                bee.RemainingBoost -= bee.BoostConsumption * Time.deltaTime;
                bardisplayBehaviour.UpdateDisplay(bee.RemainingBoost / bee.MaxBar);
            }
            else
            {
                bee.RemainingBoost = 0;
                StopBoost(default);
            }
        }

        public void AddBoostPower(float power)
        {
            bee.RemainingBoost += power;
            bardisplayBehaviour.UpdateDisplay(bee.RemainingBoost / bee.MaxBar);
        }

        private void StartBoost(InputAction.CallbackContext context)
        {
            if (bee.RemainingBoost > 0)
            {
                moverBehaviour.AdjustSpeed(bee.BoostStrength);
                enabled = true;
            }
        }

        private void StopBoost(InputAction.CallbackContext context)
        {
            if (enabled)
            {
                moverBehaviour.AdjustSpeed(1 / bee.BoostStrength);
                enabled = false
            }
        }
    }
}
