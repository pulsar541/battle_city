using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Tilemaps;

public class Enemy : Tank
{ 
    public float shootInterval = 2.0f; 
    public float turnInterval = 5.0f;
    private float _shootIntervalCounter = 0; 
    private float _turnIntervalCounter = 0; 
    int direction = 0;   

    void Update()
    {     
        if(_turnIntervalCounter >= turnInterval) 
        { 
             direction = Random.Range(0, 4); 
            _turnIntervalCounter = 0;
        }
        _turnIntervalCounter += Time.deltaTime; 
   
        if(_rigidbody.velocity.magnitude < 0.2f)
        {
           direction = Random.Range(0, 4);  
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
            Shoot(true); 
            _shootIntervalCounter = 0;
        }
        _shootIntervalCounter += Time.deltaTime;
         
    } 
 
}
