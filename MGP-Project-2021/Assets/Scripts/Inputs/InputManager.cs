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

    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }

    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }

    private void OnPosition(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }

    private void OnTap(InputAction.CallbackContext ctx)
    {
        
    }
}
