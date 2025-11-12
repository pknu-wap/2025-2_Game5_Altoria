using UnityEngine;

public interface IDestroyable
{
    void Destroy(GameObject obj);
}
public interface IEntity
{
    Transform transform { get; }

}
public interface IPlayer { }

public interface IEnemy { }

public interface IAnimal { }
public interface INPC { }

public interface IMoveData { float Speed { get; } float RunSpeed { get; } float JumpHeight { get; } float Gravity { get; } }
public interface IPlayerData
{

}
public interface IPlayerMovable
{
    IMove Move { get; }
    IMoveData MoveData { get; }
}
public interface IMove
{
    bool IsGrounded { get; }
    void SetEntity(IEntity entity);

    void Tick();
    void SetMoveInput(Vector3 input);
    void SetDestination(Vector3 destination);
    void Jump();
    void Stop();

    Transform GetTransform();
}