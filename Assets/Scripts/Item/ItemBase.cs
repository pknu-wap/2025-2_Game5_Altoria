using GameData;
using JetBrains.Annotations;
using UnityEngine;


namespace GameItem
{
    public abstract class Item:IItem,ISlottable,IDestroyableItem
    {
        public ItemData ItemData { get; private set; }
        public void SetItem(ItemData data) { ItemData = data; }
        public virtual void UseItem(IPlayer player) => ApplyItem(player);
        public abstract void ApplyItem(IPlayer player);
    }
    public class EquipItem : Item,IEquippable,IIUpgradable //장비아이템
    {
        public int Level { get; private set; }

        public override void ApplyItem(IPlayer player)
        {

        }

        public void Equip()
        {
            Debug.Log($"장비 착용: {ItemData.Name}");
            // TODO: 플레이어 스탯 반영 등
        }

        public void Unequip()
        {
            Debug.Log($"장비 해제: {ItemData.Name}");
        }

        public void Upgrade()
        {
            
        }
    }
    public class ConsumeItem : Item, IConsumable,IStackable //소비아이템
    {
        public override void ApplyItem(IPlayer player)
        {

        }

        public void Consume()
        {
           //TODO: 아이템 깎기 
        }
    }
    public class MaterialItem:Item,IStackable //재료아이템
    {
        public override void ApplyItem(IPlayer player) { }
    }
    public class MiscItem:Item, IStackable //잡템
    {
        public override void ApplyItem(IPlayer player) { }
    }

}

       
