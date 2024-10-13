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
            bardisplayBehaviour.UpdateDisplay(bee.BoostRemaining / bee.BoostBarMaximum);
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
            if (bee.BoostRemaining > 0)
            {
                bee.BoostRemaining -= bee.BoostConsumption * Time.deltaTime;
                bardisplayBehaviour.UpdateDisplay(bee.BoostRemaining / bee.BoostBarMaximum);
            }
            else
            {
                bee.BoostRemaining = 0;
                StopBoost(default);
            }
        }

        public void AddBoostPower(float power)
        {
            bee.BoostRemaining += power;
            bardisplayBehaviour.UpdateDisplay(bee.BoostRemaining / bee.BoostBarMaximum);
        }

        public void ToggleBoost()
        {
            if (enabled)
            {
                StopBoost(default);
            }
            else
            {
                StartBoost(default);
            }
        }

        private void StartBoost(InputAction.CallbackContext context)
        {
            if (!enabled && bee.BoostRemaining > 0)
            {
                moverBehaviour.AdjustSpeed(bee.BoostStrength);
                enabled = true;
                bardisplayBehaviour.IsActive(true);
            }
        }

        private void StopBoost(InputAction.CallbackContext context)
        {
            if (enabled)
            {
                moverBehaviour.AdjustSpeed(1 / bee.BoostStrength);
                enabled = false;
                bardisplayBehaviour.IsActive(false);
            }
        }
    }
}
