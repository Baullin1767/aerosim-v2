using System;
using UnityEngine;

namespace Drones
{
    [Serializable]
    public class DroneItem
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public string Size { get; private set; }

        [field: SerializeField]
        public int SizeValue { get; private set; }

        [field: SerializeField]
        public int Speed { get; private set; }

        [field: SerializeField]
        public int Mass { get; private set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        [field: SerializeField]
        public GameObject GameObject { get; private set; }
    }
}