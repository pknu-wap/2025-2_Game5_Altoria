using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "Scriptable Objects/MoveData")]
public class MoveData : ScriptableObject, IMoveData
{
    [field: SerializeField] public float Speed { get; private set; } = 5f;
    [field: SerializeField] public float RunSpeed { get; private set; } = 8f;
    [field: SerializeField] public float JumpHeight { get; private set; } = 3f;
    [field: SerializeField] public float Gravity { get; private set; } = -9.81f * 2f;
}