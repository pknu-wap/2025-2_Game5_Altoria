using static Define;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectTool", menuName = "GameInteract/CollectToolSO")]
public class CollectToolSO : ScriptableObject
{
    public string toolId;       // ¿¹: "basic_rod", "gold_pickaxe"
    public ContentType type;    // ¾î¶² ÄÜÅÙÃ÷¿ë µµ±¸ÀÎÁö (³¬½Ë´ë, °î±ªÀÌ µî)
    [Range(0f, 1f)]
    public float rareBonus;     // Èñ±Í ¾ÆÀÌÅÛ È®·ü º¸Á¤ (0.2 ¡æ 20% Áõ°¡)
}