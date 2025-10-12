using UnityEngine;
using static Define;


namespace GameItem
{
    public static class ItemFactory
    {
        public static Item CreateItem(ItemData data)
        {
            if (data == null)
            { 
                Debug.Log("[ItemFactory]: Item Data  is Null");
                return null;
            }

            Item item = data.Type switch
            { 
                ItemType.Weapon or ItemType.Tool => new EquipItem(),
                ItemType.Consume => new ConsumeItem(),
                ItemType.Material=> new MaterialItem(), 
                ItemType.Additive => new MiscItem(),
                _ => new MiscItem()
            };

            item.SetItem(data);
            return item;
        }
    }

    
}