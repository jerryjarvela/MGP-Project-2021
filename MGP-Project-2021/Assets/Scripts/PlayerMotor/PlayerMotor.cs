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
    private BaseState _state;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
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
    }
}
