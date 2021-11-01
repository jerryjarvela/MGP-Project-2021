using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Semi-singleton, COULD use mono-singleton?
    private static InputManager _instance;
    public static InputManager Instance
    {
        get { return _instance; }
    }

    // Action scheme from generated script
    private RunnerInputAction _actionScheme;
    
    // Configs
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;

    #region public properties
    
    public bool Tap
    {
        get { return tap; }
    }
    public Vector2 TouchPosition
    {
        get { return touchPosition; }
    }
    public bool SwipeLeft
    {
        get { return swipeLeft; }
    }
    public bool SwipeRight
    {
        get { return swipeRight; }
    }
    public bool SwipeUp
    {
        get { return swipeUp; }
    }
    public bool SwipeDown
    {
        get { return swipeDown; }
    }

    #endregion
    
    #region private properties

    private bool tap;
    private Vector2 touchPosition;
    private Vector2 startDrag;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;

    #endregion

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }

    private void LateUpdate()
    {
        ResetInputs();
    }

    public void OnEnable()
    {
        _actionScheme.Enable();
    }

    public void OnDisable()
    {
        _actionScheme.Disable();
    }

    private void SetupControl()
    {
        _actionScheme = new RunnerInputAction();
        
        // Register different actions
        _actionScheme.Gameplay.TapClick.performed += ctx => OnTap(ctx);
        _actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        _actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        _actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);

    }

    private void ResetInputs()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    }
    
    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = touchPosition - startDrag;
        float sqrDistance = delta.sqrMagnitude; // normal magnitude?

        // Confirmed swipe
        if (sqrDistance > sqrSwipeDeadzone)
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            if (x > y) // Left or right
            {
                if (delta.x > 0)
                {
                    swipeRight = true;
                }
                else
                {
                    swipeLeft = true;
                }
            }
            else // Up or down
            {
                if (delta.y > 0)
                {
                    swipeUp = true;
                }
                else
                {
                    swipeDown = true;
                }
            }
        }
        
        startDrag = Vector2.zero;
    }

    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        startDrag = touchPosition;
    }

    private void OnPosition(InputAction.CallbackContext ctx)
    {
        touchPosition = ctx.ReadValue<Vector2>();
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        tap = true;
    }
}
