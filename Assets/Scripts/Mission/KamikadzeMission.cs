using System.Collections;
using Drone2;

namespace Mission
{
    public abstract class KamikadzeMission : CombatMission
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            StartCoroutine(SetCargo());
        }

        public override void SetDroneSettings()
        {
            base.SetDroneSettings();
            
            DroneBridge.Instance.SetKamikadzeMode(true);
        }
        
        private IEnumerator SetCargo()
        {
            yield return null;
            yield return null;
            var cargoController = FindObjectOfType<CargoController>();
            if (cargoController != null) cargoController.HasCargo(false);
        }
    }
}