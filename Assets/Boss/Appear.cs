using UnityEngine;

public class Appear : MonoBehaviour
{
    [SerializeField] GameObject myObject;
    private int totalNails = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myObject.SetActive(false);
        totalNails = GameObject.FindObjectsByType<NailCollect>(FindObjectsSortMode.None).Length;
    }

    // Update is called once per frame
    void Update()
    {
        totalNails = GameObject.FindObjectsByType<NailCollect>(FindObjectsSortMode.None).Length;
        if(totalNails <= 3)
        {
            myObject.SetActive(true);
        }
    }
}
