using UnityEngine;

public class SpikeBallBehavior : MonoBehaviour
{
    [Header("Lifetime settings")]
    public float lifetime = 25f;

    private void Start() {
        Expire();
    }

    public void Expire() {
        if (lifetime > 0f) {
            Destroy(gameObject, lifetime);

        }
    }
}
