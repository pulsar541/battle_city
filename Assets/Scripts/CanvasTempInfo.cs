using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTempInfo : MonoBehaviour
{ 
    float _timer = 1.0f; 
    void Start()
    {  
       // this.enabled = false;
    }

    void Update() 
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;  
        }
        else {
            Destroy(this.gameObject); 
        } 
    }

    public void Init(Vector3 position, int deltaScore) 
    { 
        transform.position = position; 
    }
 
}
