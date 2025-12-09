using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    private Transform player;
    private float distanceBehind = 20f;

    public void Init(Transform target, float dist)
    {
        player = target;
        distanceBehind = dist;
    }

    private void Update()
    {
        if (player == null) return;

        if (transform.position.z < player.position.z - distanceBehind)
            Destroy(gameObject);
    }
}
