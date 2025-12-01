using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public static Menus Instance;
    // THIS SCRIPT HAS THE MAIN MENU, PAUSE MENU, AND GAME OVER MENU.
    private static bool mainMenuLoaded = false;
    private bool gamePaused = false;
    public GameObject pauseMenu;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Checks if the main menu has been loaded. If it has not, set that it has been loaded and load the main menu.
        if (!mainMenuLoaded)
        {
            mainMenuLoaded = true;
            SceneManager.LoadScene("MainMenu");
        }

        // Hide pause menu when game loads
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Start button on controller, opens pause menu.
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            // Ensures the game is able to be paused, then pauses game.
            if (SceneManager.GetActiveScene().name != "GameOverScreen" && SceneManager.GetActiveScene().name != "MainMenu" && !pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    // For Pause Menu.
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // For Pause Menu.
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // For Game Over Screen and Pause Menu.
    public void ExitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    // Load Game Over Screen
    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
    }

    // For Game Over Screen and Main Menu.
    public void StartGame()
    {
        SceneManager.LoadScene("prototype");
    }

    //For Main Menu.
    public void QuitGame()
    {
        Application.Quit();
    }
}
