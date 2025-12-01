using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MenuNavigator : MonoBehaviour
{
    public static event Action OnSubmitPressed;
    public static event Action OnUpPressed;
    public static event Action OnDownPressed;
    private MenuNavigation menuControls;
    void Awake()
    {
        menuControls = new MenuNavigation();
        menuControls.Menu.Navigate.performed += OnNavigate;
        menuControls.Menu.Submit.performed += OnSubmit;
    }

    void OnEnable()
    {
        menuControls.Menu.Enable();
    }

    void OnDisable()
    {
        menuControls.Menu.Disable();
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        if (dir.y > 0f) OnUpPressed?.Invoke();
        else if (dir.y < 0f) OnDownPressed?.Invoke();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        OnSubmitPressed?.Invoke();
    }
}
