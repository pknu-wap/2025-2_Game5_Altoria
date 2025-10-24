using UnityEngine;

public class Define
{
    public enum SceneType
    {
        None,
        Lobby,
        GameScene,
        TestFC_1,
    };
    public enum ItemType
    {
        None,
        Weapon,
        Tool,
        Consume,
        Material,
        Additive,// ±âÅ¸ 

    }
    public enum ItemGrade
    {
        None,
        Nomal,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum Content
    {
        None,
        Farm,
        Fish,
        Fell, // ¹ú¸ñ
        Animal,
        Mining,
        Plant,
        COUNT,
    }
    public enum AreaType
    {
        None,
        A, 
        B, 
        C, 
        D, 
        E,
        COUNT
    }
}
