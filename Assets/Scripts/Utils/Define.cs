using System;
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
        None,
        Farm,
        Fish,
        Fell,
        Animal,
        Mining,
        Plant,
        Upgrade,
        Craft,
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
    [Flags]
    public enum PlayerState
    {
        None = 0,


        Idle = 1 << 0,
        Move = 1 << 1,
        Jump = 1 << 2,
        Fall = 1 << 3,
        Interacting = 1<<4,
        Die = 1 << 5,

        
        Run = 1 << 10,
        Riding = 1 << 11,
        Attack = 1 << 12,
    }

    public enum CustomizationType
    {
        eyebrows,
        eyes,
        mouth,
        facialHair_,
        hair_,
        COUNT,
    }
}
