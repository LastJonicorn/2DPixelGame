using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        Vector3 spawnPos;

        if (GameManager.instance.hasCheckpoint)
        {
            spawnPos = GameManager.instance.respawnPosition;
        }
        else
        {
            spawnPos = GameObject.FindWithTag("PlayerSpawn").transform.position;
        }

        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        CinemachineCamera cam = FindAnyObjectByType<CinemachineCamera>();
        if (cam != null)
        {
            cam.Follow = player.transform;
            cam.LookAt = player.transform;
        }
    }
}