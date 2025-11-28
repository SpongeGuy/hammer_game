using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToBoss : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            SceneManager.LoadScene("BossFight");
        }
    }
}
