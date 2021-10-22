using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{
    [SerializeField] GameObject _pathPrefab;
    
    private void Start()
    {
        StartCoroutine(PathDistanceCheck());

    }

    IEnumerator PathDistanceCheck()
    {
        while (true)
        {
            Vector3 playerPosition = transform.position;
            Vector3 pathPosition = SpawnPathObject(playerPosition);

            float distance = Vector3.Distance(pathPosition, playerPosition);

            while (distance < 2)
            {
                playerPosition = transform.position;
                distance = Vector3.Distance(pathPosition, playerPosition);
                Debug.Log("Get further away");
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("You've done it!");
        }
    }

    Vector3 SpawnPathObject(Vector3 spawnPosition)
    {
        GameObject path = Instantiate(_pathPrefab, spawnPosition, Quaternion.identity);
        Vector3 pathPosition = path.transform.position;
        return pathPosition;
    }
}
