using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{ 
    // Update is called once per frame

    
    private float _shootInterval = 0;

    void Update()
    {   
        if (Input.GetButton("Horizontal"))
        {   
            if(Input.GetAxis("Horizontal") < 0) 
                GoLeft();
            else 
                GoRight(); 
        }
         
        if (Input.GetButton("Vertical"))
        {              
            if(Input.GetAxis("Vertical") < 0) 
                GoUp();
            else 
                GoDown(); 
        }
         
        if (Input.GetButton("Fire1")) { 
            if(_shootInterval >= 0.5f  || _shootInterval == 0) 
            {
                 Shoot();
                 if(_shootInterval > 0)
                    _shootInterval = 0;
            }
            _shootInterval += Time.deltaTime;
        }  
        if (Input.GetButtonUp("Fire1")) {
            _shootInterval = 0;
        } 
    }
 
}
