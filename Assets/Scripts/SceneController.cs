using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
 
    Vector3 spawnPlayer1 = new Vector3(9,-24,1);
    Vector3 spawnPlayer2 = new Vector3(17,-24,1);
  
    // Start is called before the first frame update
    void Start()
    {
         GameObject player1 = Instantiate(playerPrefab, spawnPlayer1, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
