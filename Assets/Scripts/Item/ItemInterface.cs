using UnityEngine;

namespace GameItem
{
    public interface IItem
    {
       ItemData ItemData { get; }
    }
    public interface ISlottable { }
    public interface IStackable { }
    public interface IEquippable { void Equip(); void Unequip(); }
    public interface IConsumable { void Consume(); }

    public interface IIUpgradable 
    { 
        public int Level { get; }
        public void Upgrade();
    }
    public interface IDestroyableItem { }

}