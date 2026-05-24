using System.Collections;
using UnityEngine;

public class CeilingDropSpawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject[] prefabs;

    public float spawnWidth = 10f;

    public float spawnInterval = 1f;

    public bool startActive = true;

    private Coroutine spawnRoutine;

    [Header("Rotation")]
    public float zRotation = 0f;

    void Start()
    {
        if (startActive)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null)
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnObject();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnObject()
    {
        if (prefabs.Length == 0) return;

        GameObject prefab =
            prefabs[Random.Range(0, prefabs.Length)];

        float randomX =
            Random.Range(-spawnWidth * 0.5f, spawnWidth * 0.5f);

        Vector3 spawnPos =
            transform.position + new Vector3(randomX, 0f, 0f);

        Instantiate(
            prefab,
            spawnPos,
            Quaternion.Euler(0f, 0f, zRotation)
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 left =
            transform.position + Vector3.left * spawnWidth * 0.5f;

        Vector3 right =
            transform.position + Vector3.right * spawnWidth * 0.5f;

        Gizmos.DrawLine(left, right);
    }
}