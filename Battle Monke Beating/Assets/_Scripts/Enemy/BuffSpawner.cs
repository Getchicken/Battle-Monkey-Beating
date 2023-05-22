using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public GameObject[] BuffObjects;

    public void SpawnBuff()
    {
        if (BuffObjects.Length == 0)
        {
            Debug.LogWarning("ObjectPrefabs array is empty. Cannot spawn objects.");
            return;
        }

        int randomIndex = Random.Range(0, BuffObjects.Length);

        Vector3 spawnPosition = transform.position + Vector3.up;
        Quaternion spawnRotation = Quaternion.identity;

        GameObject spawnedObject = Instantiate(BuffObjects[randomIndex], spawnPosition, spawnRotation);
        // Optionally modify the spawned object's properties or perform additional actions
    }
}

