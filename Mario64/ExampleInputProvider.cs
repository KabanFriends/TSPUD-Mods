using UnityEngine;
using LibSM64;

public class ExampleInputProvider : SM64InputProvider
{
    public GameObject cameraObject = null;

    public override Vector3 GetCameraLookDirection()
    {
        return cameraObject.transform.forward;
    }

    public override Vector2 GetJoystickAxes()
    {
        return new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") );
    }

    public override bool GetButtonHeld( Button button )
    {
        switch( button )
        {
            case SM64InputProvider.Button.Jump:  return Singleton<GameMaster>.Instance.stanleyActions.JumpAction.IsPressed;
            case SM64InputProvider.Button.Kick:  return Singleton<GameMaster>.Instance.stanleyActions.UseAction.IsPressed;
            case SM64InputProvider.Button.Stomp: return Singleton<GameMaster>.Instance.stanleyActions.Crouch.IsPressed;
        }
        return false;
    }
}