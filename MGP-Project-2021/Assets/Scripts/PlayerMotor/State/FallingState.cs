using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : BaseState
{
    public override void Destruct()
    {
        motor.anim?.SetTrigger("Fall");
    }

    public override Vector3 ProcessMotion()
    {
        // Apply gravity
        motor.ApplyGravity();
        
        // Create return vector
        Vector3 move = Vector3.zero;

        move.x = motor.SnapToLane();
        move.y = motor.verticalVelocity;
        move.z = motor.baseRunSpeed;

        return move;
    }

    public override void Transition()
    {
        if (motor.isGrounded)
        {
            motor.ChangeState(GetComponent<RunningState>());
        }
    }
}
