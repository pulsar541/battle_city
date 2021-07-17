using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tank : MonoBehaviour
{
    public float baseSpeed = 6;
 
    protected Rigidbody2D _rigidbody;
    private Vector2 _currentPosition;
    private Vector2 _oldPosition; 
 
    protected Tilemap tilemapCollider;

    int spriteChildEnableNum = 0;
 
    [SerializeField] GameObject projectilePrefab;
    private Quaternion _childLocalRotation = new Quaternion();
    private Vector2 _movement = new Vector3();   
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
        tilemapCollider = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _oldPosition = _currentPosition = transform.position;
    }


    public void GoLeft() {
        _movement = new Vector3(-baseSpeed, 0);
        _childLocalRotation =  Quaternion.Euler (0, 0, 0);           
    }
    public void GoRight() {
        _movement = new Vector3(baseSpeed, 0);
        _childLocalRotation =  Quaternion.Euler (0, 0, 180);           
    }
    public void GoUp() {
        _movement = new Vector3(0, -baseSpeed); 
        _childLocalRotation =  Quaternion.Euler (0, 0, 90);            
    }
    public void GoDown() {
        _movement = new Vector3(0, baseSpeed); 
        _childLocalRotation =  Quaternion.Euler (0, 0, 270);         
    }   

    // Update is called once per frame
    public void FixedUpdate()
    {  
        foreach (Transform child in transform) 
        { 
            child.localRotation =  _childLocalRotation;
        }

        _movement *= Time.fixedDeltaTime;  
        _rigidbody.AddForce(_movement, ForceMode2D.Impulse); 
        _rigidbody.AddRelativeForce(_movement - _rigidbody.velocity);

        if(_rigidbody.velocity.magnitude > 0.1f)
            spriteChildEnableNum = 1 - spriteChildEnableNum;
 
        int i = 0;
        foreach (Transform child in transform) 
        {  
            child.GetComponent<Renderer>().enabled = spriteChildEnableNum == i;
            i++; 
        } 
    }

    public void Shoot() { 
        Vector3 projectilePosition = transform.position; 
        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, _childLocalRotation * Quaternion.Euler(0,0,180.0f)); 
    }

}
