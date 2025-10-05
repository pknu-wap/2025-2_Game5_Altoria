using UnityEngine;

public class Define
{
    public enum SceneType
    {
        None,
        Lobby,
        GameScene,
    };
    public enum ItemType
    {
        None,
        Weapon,
        Tool,
        Consume,
        Additive,// ±âÅ¸ 

    }

    public enum CollectType
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
}
