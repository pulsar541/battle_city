using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Enemy : Tank
{ 
    public float shootInterval = 2.0f; 
    public float turnInterval = 5.0f;
    private float _shootIntervalCounter = 0; 
    private float _turnIntervalCounter = 0;
   

    int direction = 0;


    Vector3Int saveSellWorldPosition = new Vector3Int();
    
    enum Direction {
        DIR_LEFT = 0,
        DIR_RIGHT = 1,   
        DIR_UP = 2,
        DIR_DOWN = 3,  
    }

    void Update()
    {     
        if(_turnIntervalCounter >= turnInterval) 
        { 
             direction = Random.Range(0, 4); 
            _turnIntervalCounter = 0;
        }
        _turnIntervalCounter += Time.deltaTime; 
 
        Vector3Int cellWorldPosition = tilemapCollider.WorldToCell(transform.position);

        if(_rigidbody.velocity.magnitude < 0.2f)
        {
           direction = Random.Range(0, 4); 
           saveSellWorldPosition = cellWorldPosition;
        }
 
        switch((Direction)direction) {
            case Direction.DIR_LEFT: 
                GoLeft(); 
                break;
            case Direction.DIR_RIGHT: 
                GoRight(); 
                break;
            case Direction.DIR_UP: 
                GoUp(); 
                break;
            case Direction.DIR_DOWN: 
                GoDown(); 
                break;                                                
        } 

        if(_shootIntervalCounter >= shootInterval) 
        {
            Shoot(); 
            _shootIntervalCounter = 0;
        }
        _shootIntervalCounter += Time.deltaTime;
         
    } 
 
}
