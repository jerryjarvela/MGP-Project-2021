using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane;

    public float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

    public CharacterController controller;
    public Animator anim;
    private BaseState _state;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        _state = GetComponent<RunningState>();
        _state.Construct();
    }

    private void Update()
    {
        UpdateMotor();
    }

    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
    
    public void ChangeState(BaseState s)
    {
        _state.Destruct();
        _state = s;
        _state.Construct();
    }

    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < -terminalVelocity)
        {
            verticalVelocity = -terminalVelocity;
        }
    }
    
    public float SnapToLane()
    {
        float returnValue = 0.0f;

        if (transform.position.x != (currentLane * distanceInBetweenLanes))
        {
            float deltaToDesiredPosition = (currentLane * distanceInBetweenLanes) - transform.position.x;
            returnValue = (deltaToDesiredPosition > 0) ? 1 : -1;
            returnValue *= baseSidewaySpeed;

            // Calculate exact distance to travel
            float actualDistance = returnValue * Time.deltaTime;

            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
            {
                returnValue = deltaToDesiredPosition * (1 / Time.deltaTime);
            }
        }
        else
        {
            returnValue = 0;
        }
        
        return returnValue;
    }
    
    private void UpdateMotor()
    {
        // Check if we're grounded
        isGrounded = controller.isGrounded;
        
        // How should we be moving? based on state
        moveVector = _state.ProcessMotion();
        
        // Are we trying to change state?
        _state.Transition();
        
        // Move the player
        controller.Move(moveVector * Time.deltaTime);
        
        // Feed animator values
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));
    }
}
