using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuSelection : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip menuSelect;
    [SerializeField] private float selectVolume = 0.5f;
    [SerializeField] private AudioClip menuHammer;
    [SerializeField] private float hammerVolume = 0.5f;
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
        DontCutAudio(menuHammer, hammerVolume);
        if (currentButton == 1)
        {
            Menus.Instance.StartGame();
        } else
        {
            Menus.Instance.QuitGame();
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
