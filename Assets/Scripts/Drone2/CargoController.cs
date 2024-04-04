using UnityEngine;

namespace Drone2
{
    public class CargoController : MonoBehaviour
    {
        [SerializeField]
        private Cargo cargo;

        private bool _hasCargo;

        public void HasCargo(bool has)
        {
            _hasCargo = has;
            cargo.gameObject.SetActive(false);
        }

        private void OnDrop()
        {
            if (!_hasCargo) return;
            if (cargo != null) cargo.Drop();
        }
    }
}