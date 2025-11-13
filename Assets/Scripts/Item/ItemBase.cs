using Common;
using GameUI;
using UnityEngine;


namespace GameItem
{
    public abstract class Item : IItem, ISlottable, IDestroyableItem
    {
        public ItemData ItemData { get; private set; }
        public void SetItem(ItemData data) { ItemData = data; }
        public virtual void UseItem(IPlayer player) => ApplyItem(player);
        public abstract void ApplyItem(IPlayer player);
    }
    public class EquipItem : Item, IEquippable, IIUpgradable //장비아이템
    {
        public int Level { get; private set; }

        public override void ApplyItem(IPlayer player)
        {

        }

        public void Equip()
        {
            GameSystem.Inventory.SetEquipItem(this);
        }

        public void Unequip()
        {
            GameSystem.Inventory.SetUnequipItem(this);
        }

        public void Upgrade()
        {
            if (Level == 10)
                return;

            Level += 1;
            Manager.UI.MainHUD.GetWidget<Hotbar>().LevelChanged(ItemData.ID, Level);
        }
    }
    public class ConsumeItem : Item, IConsumable, IStackable //소비아이템
    {
        public override void ApplyItem(IPlayer player)
        {

        }

        public void Consume()
        {
            //TODO: 아이템 깎기 
        }
    }
    public class MaterialItem : Item, IStackable //재료아이템
    {
        public override void ApplyItem(IPlayer player) { }
    }
    public class MiscItem : Item, IStackable //잡템
    {
        public override void ApplyItem(IPlayer player) { }
    }

}


