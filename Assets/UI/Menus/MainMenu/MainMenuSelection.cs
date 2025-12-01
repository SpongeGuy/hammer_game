using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuSelection : MonoBehaviour
{
    private int currentButton = 1;
    public GameObject beginSelected;
    public GameObject quitSelected;
    public GameObject begin;
    public GameObject quit;
    public GameObject quitNail;
    public GameObject beginNail;
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
            begin.SetActive(false);
            quitNail.SetActive(false);
            quitSelected.SetActive(false);
            beginSelected.SetActive(true);
            quit.SetActive(true);
            beginNail.SetActive(true);
        } else
        {
            begin.SetActive(true);
            quitNail.SetActive(true);
            quitSelected.SetActive(true);
            beginSelected.SetActive(false);
            quit.SetActive(false);
            beginNail.SetActive(false);
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
            Menus.Instance.QuitGame();
        }
    }
}
