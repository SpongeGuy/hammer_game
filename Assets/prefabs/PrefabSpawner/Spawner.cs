using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab to spawn")]
    public GameObject prefab;

    [Header("Where to spawn")]
    public Transform spawnPoint; // if left blank, it will use this object's position

    [Header("Spawn rate")]
    public bool spawnContinuously = true;
    public float spawnInterval = 1f;


    private float timer;

    private void Start() {
        timer = 0f;
    }

    private void Update() {
        if (!spawnContinuously || prefab == null) return;
        timer += Time.deltaTime;

        if (timer >= spawnInterval) {
            Spawn();
            timer = 0f;
        }
    }

    public void Spawn() {
        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rotation = spawnPoint !=  null ? spawnPoint.rotation : transform.rotation;

        Instantiate(prefab, position, rotation);
    }

    // public methods
    public void StartSpawning() => spawnContinuously = true;
    public void StopSpawning() => spawnContinuously = false;
    public void ToggleSpawning() => spawnContinuously = !spawnContinuously;
}
