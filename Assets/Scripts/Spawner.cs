using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public List<GameObject> objects;

    public int maxObjectsNumber = 5;
    public List<Transform> locationsToSpawn;

    public List<GameObject> enemyPrefabs;
    Vector2 timeToNextSpawn =  new Vector2(1.0f, 2.75f);
    Rigidbody2D objectToFollow;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        objects = new List<GameObject>();
        
        objectToFollow = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        GameManager.instance.OnRestart += RestartSpawnerState;
        Invoke("SpawnEnemy", 1.0f);
    }


    void SpawnEnemy()
    {
        if(GameManager.instance.state == GameState.InGame)
        { 
            if(objects.Count < maxObjectsNumber)
            { 
                int locationIndex = Random.Range(0, locationsToSpawn.Count);
                int enemyIndex = Random.Range(0, enemyPrefabs.Count);
                GameObject instance = Instantiate(enemyPrefabs[enemyIndex], locationsToSpawn[locationIndex].position, Quaternion.identity);
                instance.transform.parent = transform;
                objects.Add(instance);

                EnemyController enemy = instance.GetComponent<EnemyController>();
                enemy.playerRb = objectToFollow;

                float time = Random.Range(timeToNextSpawn.x, timeToNextSpawn.y);
                Invoke("SpawnEnemy", time);
                return;
            }
        }
        StopAllCoroutines();
        StartCoroutine(CheckObjectsCount());
    }

    IEnumerator CheckObjectsCount()
    {
        while (true)
        {
            if(objects.Count < maxObjectsNumber)
            {
                Invoke("SpawnEnemy", Random.Range(timeToNextSpawn.x, timeToNextSpawn.y));
                break;
            }
            yield return null;
        }
    }

    public void RemoveObjectFromList(GameObject obj)
    {
        objects.Remove(obj);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .3f);
    }

    void RestartSpawnerState()
    {
        foreach(var obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();

    }
}
