using UnityEngine;

public class Define
{
    public enum SceneType
    {
        None,
        Lobby,
        GameScene,
        TestFC_1,
        TestMap,
    };
    public enum ItemType
    {
        None,
        Weapon,
        Tool,
        Consume,
        Material,
        Additive,// ��Ÿ 

    }
    public enum ItemGrade
    {
        None,
        Normal,
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
        Fell=3,// ����
        Animal=4,
        Mining=5,
        Plant=6,
        Upgrade=7,
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
