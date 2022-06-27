using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject harderEnemyPrefab;
    private float spawnRange = 9;
    public int waveNumber = 1;
    public GameObject powerupPrefab;
    public GameObject powerIconPrefab;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(enemyPrefab, waveNumber);
    }
    
    GameObject InitializeRandom()
    {
        // Debug.Log(Random.Range(0, 2));
        return Random.Range(0, 2) == 0 ? enemyPrefab : harderEnemyPrefab;
    }

    private Vector3 GenerateSpawnPosition(){
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;

    }
    void SpawnEnemyWave(GameObject prefab, int enemiesToSpawn){
        for (int i = 0; i < enemiesToSpawn; i++){
            Instantiate(prefab, GenerateSpawnPosition(), prefab.transform.rotation);
        }
        waveNumber++;
    }
    // Update is called once per frame
    public int enemyCount;
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length + FindObjectsOfType<HarderEnemy>().Length;
        Debug.Log(enemyCount);
        if (enemyCount == 0) {
            var newEnemy = InitializeRandom();
            SpawnEnemyWave(newEnemy, waveNumber);
        }   
    }
}
