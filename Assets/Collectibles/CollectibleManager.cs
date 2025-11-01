using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;
    public int collectedCount = 0;
    public TextMeshProUGUI collectibleCountText;
    public int totalCollectibles;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    void Start ()
    {
        totalCollectibles = GameObject.FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;
        UpdateCollectibleCount();
    }

    public void CollectNut()
    {
        collectedCount++;
        UpdateCollectibleCount();
    }

    void UpdateCollectibleCount()
    {
        collectibleCountText.text = collectedCount + " / " + totalCollectibles;
    }
}
