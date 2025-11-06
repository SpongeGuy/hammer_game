using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [TextArea] public string[] dialogueLines;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public GameObject interactPrompt;

    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        {
            if (!playerInRange) return;

            if (!dialogueActive)
                StartDialogue();
            else
                NextLine();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactPrompt.SetActive(false);
            EndDialogue();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLineIndex];
        interactPrompt.SetActive(false);
    }

    void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        interactPrompt.SetActive(playerInRange);
    }
}
