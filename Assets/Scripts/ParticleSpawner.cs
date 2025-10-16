using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _object;

    public void Spawn()
    {
        Instantiate(_object, transform.position, Quaternion.identity);
    }
}
