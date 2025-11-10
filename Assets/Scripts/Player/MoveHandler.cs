using UnityEngine;

public class MoveHandler
{
    private readonly IMove move;
    private Vector2 inputDir;
    private Transform cameraTransform;

    public MoveHandler(IMove move)
    {
        this.move = move;
        cameraTransform = Camera.main?.transform;
    }

    
    public void SetInput(Vector2 dir)
    {
        inputDir = dir;
    }


    public void Tick()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;

        Vector3 moveDir = Vector3.zero;

        if (inputDir.sqrMagnitude > 0.01f)
        {
            moveDir = CalculateCameraRelativeDirection(inputDir);
            move.SetMoveInput(moveDir);
        }
        else
        {
            move.SetMoveInput(Vector3.zero);
        }

        move.Tick();
    }

    private Vector3 CalculateCameraRelativeDirection(Vector2 inputDir)
    {
        if (cameraTransform == null)
            return new Vector3(inputDir.x, 0, inputDir.y);

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        return (camForward * inputDir.y + camRight * inputDir.x).normalized;
    }
}
