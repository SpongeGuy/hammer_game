using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOverSelection : MonoBehaviour
{
    private int currentButton = 1;
    public GameObject retrySelected;
    public GameObject mainSelected;
    public GameObject retry;
    public GameObject main;
    public GameObject retryNail;
    public GameObject mainNail;
    // Menu navigation is enabled.
    void OnEnable()
    {
        MenuNavigator.OnUpPressed += MoveUp;
        MenuNavigator.OnDownPressed += MoveDown;
        MenuNavigator.OnSubmitPressed += UseButton;
    }
    // Menu navigation is disabled.
    void OnDisable()
    {
        MenuNavigator.OnUpPressed -= MoveUp;
        MenuNavigator.OnDownPressed -= MoveDown;
        MenuNavigator.OnSubmitPressed -= UseButton;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSelectedButton();
    }

    // When moving up on main menu:
    void MoveDown()
    {
        currentButton++;
        if (currentButton > 2)
        {
            currentButton = 1;
        }
        UpdateSelectedButton();
    }

    // When moving down on main menu:
    void MoveUp()
    {
        currentButton--;
        if (currentButton < 1)
        {
            currentButton = 2;
        }
        UpdateSelectedButton();
    }

    // Called to update the visuals for the selected buttons.
    void UpdateSelectedButton()
    {
       if (currentButton == 1)
        {
            retry.SetActive(false);
            mainNail.SetActive(false);
            mainSelected.SetActive(false);
            retrySelected.SetActive(true);
            main.SetActive(true);
            retryNail.SetActive(true);
        } else
        {
            retry.SetActive(true);
            mainNail.SetActive(true);
            mainSelected.SetActive(true);
            retrySelected.SetActive(false);
            main.SetActive(false);
            retryNail.SetActive(false);
        }
    }

    // Called when the selected button is pressed.
    void UseButton()
    {

        if (currentButton == 1)
        {
            Menus.Instance.StartGame();
        } else
        {
            Menus.Instance.ExitToMainMenu();
        }
    }
}
