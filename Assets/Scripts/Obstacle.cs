using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.TakeDamage(damage);
        }
    }
}
