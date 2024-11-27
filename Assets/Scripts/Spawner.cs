using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemy;
    public float respawnTime = 3f;
    public int enemySpawnCount = 2;
    public GameController gameController;

    private bool lastEnemySpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
        if (lastEnemySpawned && FindAnyObjectByType<EnemyScript>()==null)
        {
            StartCoroutine(gameController.LevelComplete());
        } 
    }
    IEnumerator EnemySpawner()
    {
      for(int i = 0; i < enemySpawnCount; i++)
        {

            yield return new WaitForSeconds(respawnTime);
            SpawnEnemy();
            
        }
        lastEnemySpawned=true;
    }
    void SpawnEnemy()
    {
        int randomValue = Random.Range(0, enemy.Length);
        int randomXPos =  Random.Range(-1,3);
        Instantiate(enemy[randomValue] , new Vector2(randomXPos, transform.position.y), Quaternion.identity);
    }
}
