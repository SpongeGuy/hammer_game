using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    [SerializeField] private UnityEvent _hit;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private float volume = 0.7f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position, volume);
            
            _hit?.Invoke();
        }
    }
}

