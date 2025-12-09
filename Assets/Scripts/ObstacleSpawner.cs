using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject[] bonusPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistanceAhead = 30f;
    [SerializeField] private float laneOffset = 2f;
    [SerializeField] private float despawnDistanceBehind = 20f;
    [SerializeField, Range(0f, 1f)] private float bonusChance = 0.2f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        int lane = Random.Range(0, 3);
        float laneX = (lane - 1) * laneOffset;

        Vector3 pos = new Vector3(
            laneX,
            0.5f,
            player.transform.position.z + spawnDistanceAhead
        );

        bool spawnBonus = bonusPrefabs.Length > 0 && Random.value < bonusChance;

        GameObject obj;

        if (spawnBonus)
        {
            obj = Instantiate(
                bonusPrefabs[Random.Range(0, bonusPrefabs.Length)],
                pos + Vector3.up * 0.5f,
                Quaternion.identity
            );
        }
        else
        {
            obj = Instantiate(
                obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)],
                pos,
                Quaternion.identity
            );
        }

        obj.AddComponent<AutoDespawn>().Init(player.transform, despawnDistanceBehind);
    }
}
