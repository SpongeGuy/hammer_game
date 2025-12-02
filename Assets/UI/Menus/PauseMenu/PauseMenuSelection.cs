using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuSelection : MonoBehaviour
{
     private AudioSource audioSource;
    [SerializeField] private AudioClip menuSelect;
    [SerializeField] private float selectVolume = 0.5f;
    [SerializeField] private AudioClip menuHammer;
    [SerializeField] private float hammerVolume = 0.5f;
    private int currentButton = 1;
    public GameObject resumeSelected;
    public GameObject restartSelected;
    public GameObject returnSelected;
    public GameObject resume;
    public GameObject restart;
    public GameObject returns;
    public GameObject resumeNail;
    public GameObject returnNail;
    public GameObject restartNail;
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
        if (currentButton > 3)
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
            currentButton = 3;
        }
        audioSource.PlayOneShot(menuSelect, selectVolume);
        UpdateSelectedButton();
    }

    // Called to update the visuals for the selected buttons.
    void UpdateSelectedButton()
    {
       if (currentButton == 1)
        {
            returns.SetActive(true);
            returnNail.SetActive(false);
            returnSelected.SetActive(false);
            restart.SetActive(true);
            restartNail.SetActive(false);
            restartSelected.SetActive(false);
            resumeSelected.SetActive(true);
            resume.SetActive(false);
            resumeNail.SetActive(true);
        } else if (currentButton == 2)
        {
            returns.SetActive(true);
            returnNail.SetActive(false);
            returnSelected.SetActive(false);
            restart.SetActive(false);
            restartNail.SetActive(true);
            restartSelected.SetActive(true);
            resumeSelected.SetActive(false);
            resume.SetActive(true);
            resumeNail.SetActive(false);
        } else
        {
            returns.SetActive(false);
            returnNail.SetActive(true);
            returnSelected.SetActive(true);
            restart.SetActive(true);
            restartNail.SetActive(false);
            restartSelected.SetActive(false);
            resumeSelected.SetActive(false);
            resume.SetActive(true);
            resumeNail.SetActive(false);
        }
    }

    // Called when the selected button is pressed.
    void UseButton()
    {
        DontCutAudio(menuHammer, hammerVolume);
        if (currentButton == 1)
        {
            Menus.Instance.ResumeGame();
        } else if (currentButton == 2)
        {
            Menus.Instance.RestartLevel();
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

