using GameInteract;
using System.Diagnostics;

public class InputBinder
{
    private PlayerInputHandler handler;
    private IMoveInput currentReceiver;

    public InputBinder(PlayerInputHandler input) { handler = input; }
    public void Initialize(IMoveInput  receiver)
    {
        handler.OnMove += receiver.OnMoveInput;
        handler.OnMoveCanceled += receiver.OnMoveCancel;
        handler.OnJump += receiver.OnJumpInput;
        if (receiver is IInteractInput interact) handler.OnInteract += interact.TryInteract;
        if (receiver is IRidingInput riding) handler.OnRiding += riding.TryRiding;
    }
    public void Bind(IMoveInput receiver)
    {
        Unbind();
        currentReceiver = receiver;
        if (receiver == null)
        {
            UnityEngine.Debug.Log("RECEIVER IS NONE");
        }

        handler.OnMove += receiver.OnMoveInput;
        handler.OnMoveCanceled += receiver.OnMoveCancel;
        handler.OnJump += receiver.OnJumpInput;
    }

    public void Unbind()
    {
        if (currentReceiver == null) return;

        handler.OnMove -= currentReceiver.OnMoveInput;
        handler.OnMoveCanceled -= currentReceiver.OnMoveCancel;
        handler.OnJump -= currentReceiver.OnJumpInput;
        currentReceiver = null;
    }
}
