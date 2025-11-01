using UnityEngine;
using TMPro;

public class NailManager : MonoBehaviour
{
    public static NailManager Instance;
    public Transform player;
    public int maxNails = 3;
    public float followSpeed = 5f;
    public float followDistance = 1.5f;
    private int nailCount = 0;
    private int visibleNails = 0;
    [SerializeField] private TextMeshProUGUI nailCountText;
    // On start, ensuring that there is only one instance of NailManager.
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateNailCount();
    }

    // When the player collects a nail, increase nail count.
    // Then, check if there is room for more followers. If not, hide collected nail.
    public void CollectNail(GameObject NailPrefab)
    {
        nailCount++;
        if (visibleNails < maxNails)
        {
            ConvertToFollower(NailPrefab);
        }
        else
        {
            NailPrefab.SetActive(false);
        }

        UpdateNailCount();
    }

    // Makes the collected nail follow the player, utilizing NailFollow.cs.
    private void ConvertToFollower(GameObject NailPrefab)
    {
        NailPrefab.AddComponent<NailFollow>().OnFollow(visibleNails, followSpeed, followDistance);
        visibleNails++;
    }

    // THIS PART OF THE CODE STILL NEEDS CHECKED!!!!!!
    //
    // When a nail is used, check if a new nail can replace it. Destroy visible nail if not.
    public void UseNail()
    {
        if (nailCount <= 0) return;
        nailCount--;

        if (visibleNails > nailCount)
        {
            NailFollow[] currentNails = GameObject.FindObjectsByType<NailFollow>(FindObjectsSortMode.None);
            if (currentNails.Length > 0)
            {
                Destroy(currentNails[currentNails.Length - 1].gameObject);
                visibleNails--;
            }
        }

        UpdateNailCount();
    }

    // UI Management for Nails
    private void UpdateNailCount()
    {
        nailCountText.text = "x" + nailCount;
    }
}
