using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOverSelection : MonoBehaviour
{
     private AudioSource audioSource;
    [SerializeField] private AudioClip menuSelect;
    [SerializeField] private float selectVolume = 0.5f;
    [SerializeField] private AudioClip menuHammer;
    [SerializeField] private float hammerVolume = 0.5f;
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
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

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
        audioSource.PlayOneShot(menuSelect, selectVolume);
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
        audioSource.PlayOneShot(menuSelect, selectVolume);
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
        DontCutAudio(menuHammer, hammerVolume);
        if (currentButton == 1)
        {
            Menus.Instance.StartGame();
        } else
        {
            Menus.Instance.ExitToMainMenu();
        }
    }

    // Prevents the audio from cutting when switching scenes.
    void DontCutAudio(AudioClip clip, float volume)
    {
        GameObject transitionAudio = new GameObject("TransitionAudio");
        DontDestroyOnLoad(transitionAudio);

        AudioSource transitionSource = transitionAudio.AddComponent<AudioSource>();
        transitionSource.volume = volume;
        transitionSource.PlayOneShot(clip);

        Destroy(transitionAudio, clip.length);
    }
}
