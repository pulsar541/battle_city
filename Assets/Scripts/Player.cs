using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float baseSpeed = 15;

    private Rigidbody2D _rigidbody;
    private Vector2 _currentPosition;
    private Vector2 _oldPosition; 

    int spriteChildEnableNum = 0;

    private float _msek = 0;

    [SerializeField] GameObject projectilePrefab;
    private Quaternion _childLocalRotation = new Quaternion();

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
    }

    // Start is called before the first frame update
    void Start()
    {
        _oldPosition = _currentPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = new Vector3();   

        if (Input.GetButton("Horizontal"))
        {   
            movement = new Vector3(Input.GetAxis("Horizontal") * baseSpeed, 0);
            _childLocalRotation =  Quaternion.Euler (0,0, Input.GetAxis("Horizontal") < 0 ? 0 : 180);    
        }
         
        if (Input.GetButton("Vertical"))
        { 
            movement = new Vector3(0, Input.GetAxis("Vertical") * baseSpeed); 
            _childLocalRotation =  Quaternion.Euler (0,0, Input.GetAxis("Vertical") < 0 ? 90 : 270); 
        }

        foreach (Transform child in transform) 
        { 
            child.localRotation =  _childLocalRotation;
        }

        movement *= Time.fixedDeltaTime;  
        _rigidbody.AddForce(movement, ForceMode2D.Impulse); 
        _rigidbody.AddRelativeForce(movement - _rigidbody.velocity);

        if(_rigidbody.velocity.magnitude > 0.1f)
            spriteChildEnableNum = 1 - spriteChildEnableNum;
 
        int i = 0;
        foreach (Transform child in transform) 
        {  
            child.GetComponent<Renderer>().enabled = spriteChildEnableNum == i;
            i++; 
        }



    }

    void Shoot() {
        ///Vector3 direction =  new Vector3(Mathf.Sin(_childLocalRotation.z)  ,Mathf.Cos(_childLocalRotation.z) ,0).normalized ;
        Vector3 projectilePosition = transform.position;// + direction * transform.localScale.x;
 
        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, _childLocalRotation * Quaternion.Euler(0,0,180.0f));
        //projectile.GetComponent<Projectile>().Direction = direction;
    }

    void Update() {  

        if (Input.GetButton("Fire1")) {

            if(_msek >= 0.5f  || _msek == 0) 
            {
                 Shoot();
                 if(_msek > 0)
                    _msek = 0;
            }
            _msek += Time.deltaTime;
        } 

        if (Input.GetButtonUp("Fire1")) {
            _msek = 0;
        }
        
    }
}
