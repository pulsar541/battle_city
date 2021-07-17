using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;
 
 
    Vector3 spawnPlayer1 = new Vector3(9,-24,0);
    Vector3 spawnPlayer2 = new Vector3(17,-24,0);
  
    Vector3[] spawnEnemy  = { new Vector3(1,0,0),   new Vector3(13,0,0), new Vector3(24,0,0) }; //(1,0,0); 
    //Vector3 spawnEnemy2 = new Vector3(13,0,0);//
    //Vector3 spawnEnemy3 = new Vector3(24,0,0);
    
    public float enemySpawnInterval = 3.0f;
    private int _currentEnemySpawnIndex = 0;
    private float _enemySpawnCounter = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
         GameObject player1 = Instantiate(playerPrefab, spawnPlayer1, Quaternion.identity); 
    }
    // Update is called once per frame
    void Update()
    {
        if( _enemySpawnCounter > enemySpawnInterval) 
        { 
           GameObject enemy = Instantiate(enemyPrefab, spawnEnemy[_currentEnemySpawnIndex], Quaternion.identity); 
           _enemySpawnCounter = 0;
           _currentEnemySpawnIndex ++;
           if(_currentEnemySpawnIndex > 2)
                _currentEnemySpawnIndex = 0;
        }
        _enemySpawnCounter += Time.deltaTime;
    }
}
