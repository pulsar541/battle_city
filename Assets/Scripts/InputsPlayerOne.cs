using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsPlayerOne : InputsPlayer
{

    Tank tank;
    float _shootInterval;
    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
    }

    // Update is called once per frame
    public override void InputsUpdate()
    {
         if(LevelController.gameMode == 0) 
        {
            if (Input.GetButton("Horizontal"))
            {
                if (Input.GetAxis("Horizontal") < 0)
                    tank.GoLeft();
                else
                     tank.GoRight();
            }

            if (Input.GetButton("Vertical"))
            {
                if (Input.GetAxis("Vertical") < 0)
                     tank.GoDown();
                else
                     tank.GoUp();
            }  

            if (Input.GetButton("Fire1")) { 
                if(_shootInterval >= 0.5f  || _shootInterval == 0) 
                {
                    tank.Shoot();
                    if(_shootInterval > 0)
                        _shootInterval = 0;
                }
                _shootInterval += Time.deltaTime;
            }  
            if (Input.GetButtonUp("Fire1")) {
                _shootInterval = 0;
            } 
        }
        else if(LevelController.gameMode == 1) 
        {
            if (Input.GetKey(KeyCode.A))
                tank.GoLeft();

            if (Input.GetKey(KeyCode.D))
                tank.GoRight();

            if (Input.GetKey(KeyCode.W))
                tank.GoUp();

            if (Input.GetKey(KeyCode.S))
                tank.GoDown(); 

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (_shootInterval >= 0.5f || _shootInterval == 0)
                {
                    tank.Shoot();
                    if (_shootInterval > 0)
                        _shootInterval = 0;
                }
                _shootInterval += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                _shootInterval = 0;
            }      
        }
    }
}
