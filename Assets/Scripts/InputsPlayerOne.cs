using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsPlayerOne : InputsPlayer
{

    Player playerTank;
    float _shootInterval;
    // Start is called before the first frame update
    void Start()
    {
        playerTank = GetComponent<Player>();
    }

    // Update is called once per frame
    public override void InputsUpdate(bool onIce = false)
    { 
         if(LevelController.gameMode == 0) 
        {
            if(!onIce) 
            {
                if (Input.GetButton("Horizontal"))
                {
                    if (Input.GetAxis("Horizontal") < 0)
                        playerTank.GoLeft();
                    else
                        playerTank.GoRight();
                }

                if (Input.GetButton("Vertical"))
                {
                    if (Input.GetAxis("Vertical") < 0)
                        playerTank.GoDown();
                    else
                        playerTank.GoUp();
                }  
            }

            if (Input.GetButton("Fire1")) { 
                if(_shootInterval >= 0.5f  || _shootInterval == 0) 
                {
                    playerTank.Shoot(false);
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
            if(!onIce) 
            {
                if (Input.GetKey(KeyCode.A))
                    playerTank.GoLeft();

                if (Input.GetKey(KeyCode.D))
                    playerTank.GoRight();

                if (Input.GetKey(KeyCode.W))
                    playerTank.GoUp();

                if (Input.GetKey(KeyCode.S))
                    playerTank.GoDown(); 
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (_shootInterval >= 0.5f || _shootInterval == 0)
                {
                    playerTank.Shoot(false);
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
