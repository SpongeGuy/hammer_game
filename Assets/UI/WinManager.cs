using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;
    private int nailCount = 0;
    private int totalNails = 0;
    private int totalCollectibles;
    public GameObject WinScreen;
    private bool gameWon = false;

    void Start()
    {
        totalCollectibles = GameObject.FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
        totalNails = GameObject.FindObjectsByType<NailCollect>(FindObjectsSortMode.None).Length;
    }

    // Update is called once per frame
    void Update()
    {
        totalCollectibles = GameObject.FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
        totalNails = GameObject.FindObjectsByType<NailCollect>(FindObjectsSortMode.None).Length;
        if (gameWon)
        {
            WinScreen.SetActive(true);
        }
        else
        {
            WinScreen.SetActive(false);
        }
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (totalNails == 3)
        {
            if (totalCollectibles == 0)
            {
                gameWon = true;
            }
        }
    }
}
