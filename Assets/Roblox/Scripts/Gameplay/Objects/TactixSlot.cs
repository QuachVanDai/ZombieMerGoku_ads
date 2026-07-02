using ExampleProject.Gameplay.Unit;
using UnityEngine;

namespace ExampleProject
{
    public class TactixSlot : MonoBehaviour
    {
        [SerializeField] int slotIndex;
        [SerializeField] CreepUnit unit;

        public int SlotIndex
        {
            get => slotIndex;
            set => slotIndex = value;
        }

        public CreepUnit Unit
        {
            get => unit;
            set => unit = value;
        }
    }
}
