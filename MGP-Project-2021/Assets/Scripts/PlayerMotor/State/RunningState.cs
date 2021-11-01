using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState
{
    public override Vector3 ProcessMotion()
    {
        Vector3 move = Vector3.zero;

        move.x = 0;
        move.y = -1.0f;
        move.z = motor.baseRunSpeed;
        
        return move;
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
        {
            // Change lane, go left
            motor.ChangeLane(-1);
        }
        
        if (InputManager.Instance.SwipeRight)
        {
            // Change lane, go right
            motor.ChangeLane(1);
        }
        
        if (InputManager.Instance.SwipeUp && motor.isGrounded)
        {
            // Change to jumping state
            // TODO
            //motor.ChangeState(GetComponent<JUmpingState>());
        }
    }
}
