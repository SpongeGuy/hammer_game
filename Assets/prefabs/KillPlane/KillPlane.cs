using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlane : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            Menus.Instance.GameOver();
        }
    }
}
