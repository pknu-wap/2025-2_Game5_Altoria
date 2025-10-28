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

    public enum ContentType
    {
        None=0,
        Farm=1,
        Fish=2,
        Fell=3,// ¹ú¸ñ
        Animal=4,
        Mining=5,
        Plant=6,
        COUNT=7,
        Craft=8,
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
    public enum PlayerState
    {
        Idle,
        Move,
        Jump,
        Interacting,
        Attack,
        Die,
    }
    
}
