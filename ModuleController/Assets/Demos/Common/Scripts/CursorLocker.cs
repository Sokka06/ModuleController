using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLocker : MonoBehaviour
{
    public bool focusOnEnable = true; // whether or not to focus and lock cursor immediately on enable
    
    static bool Focused {
        get => Cursor.lockState == CursorLockMode.Locked;
        set {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = value == false;
        }
    }
    
    void OnEnable() {
        if( focusOnEnable ) Focused = true;
    }

    void OnDisable() => Focused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Focused)
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;
            
            // Leave cursor lock
            if( keyboard.escapeKey.wasPressedThisFrame )
                Focused = false;
        }
        else
        {
            var mouse = Mouse.current;
            if (mouse == null)
                return;
            
            if( mouse.leftButton.wasPressedThisFrame )
                Focused = true;
        }
    }
}
