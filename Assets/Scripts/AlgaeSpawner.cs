using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgaeSpawner : MonoBehaviour
{
    public GameObject algaePrefab;

    public Vector2 areaSize = new Vector2(8f, 4f);
    
    public Vector2 spawnDelayRange = new Vector2(3f, 8f);   
    public Vector2 scaleRange = new Vector2(0.5f, 1.5f);    

    public int maxAlgae = 6;

    private readonly List<GameObject> activeAlgae = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnDelayRange.x, spawnDelayRange.y));

            activeAlgae.RemoveAll(a => a == null);
            if (activeAlgae.Count < maxAlgae)
            {
                SpawnOne();
            }
        }
    }

    private void SpawnOne()
    {
        Vector3 offset = new Vector3(Random.Range(-areaSize.x / 2f, areaSize.x / 2f), Random.Range(-areaSize.y / 2f, areaSize.y / 2f), 0f);

        GameObject algae = Instantiate(algaePrefab, transform.position + offset, Quaternion.identity, transform);

        float s = Random.Range(scaleRange.x, scaleRange.y);
        algae.transform.localScale = new Vector3(s, s, 1f);

        activeAlgae.Add(algae);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, areaSize.y, 0f));
    }
}