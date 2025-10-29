using UnityEngine;


public interface IAmbient
{
    void Ambient();
}
public interface IMovable
{
    void MoveTo(Vector3 dir, float speed);
    void Stop();
}