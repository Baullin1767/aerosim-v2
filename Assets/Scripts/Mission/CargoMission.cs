using System.Collections;
using Drone2;

namespace Mission
{
    public abstract class CargoMission : CombatMission
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            StartCoroutine(SetCargo());
        }
        
        private IEnumerator SetCargo()
        {
            yield return null;
            yield return null;
            var cargoController = FindObjectOfType<CargoController>();
            if (cargoController != null) cargoController.HasCargo(true);
        }
    }
}