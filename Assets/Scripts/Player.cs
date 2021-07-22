using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Player : Tank
{
    // Update is called once per frame

    [SerializeField] public TileBase tileBaseIce;

    [SerializeField] private AudioSource soundSourceFire;
    [SerializeField] private AudioClip fireSound;


    public int selfIndex = -1;

    public Vector3 _spawnPosition = new Vector3();

    public float _shootInterval = 0;

    AudioSource audioSourceEngine = null;
    [SerializeField]  AudioClip engineAudioClip;

    private bool _isRespawnProcess = false;

    void Start()
    {
        gameObject.name = "Player" + GetInstanceID().ToString();
        //_spawnPosition = transform.position;
        Lives = 3;
        LevelController.UpdateBattleInformation();


        foreach (Transform child in transform)
        {
            if (child.name == "AudioSourceEngine") {
                audioSourceEngine = child.GetComponent<AudioSource>();
               // audioSourceEngine.PlayOneShot(engineAudioClip); 
                break;
            }
        }


    }

    public void Init(int index, Vector3 spawnPosition)
    {
        selfIndex = index;
        Global.score[index] = 0;
        _spawnPosition = spawnPosition;
        for (int i = 0; i < (int)Tank.Type.MAX_TYPES; i++)
            Global.destroyedTankTypesCounter[selfIndex, i] = 0;
    }

 
    public override void BattleObjectUpdate()
    { 

        if (LevelController.isGameOver)
            return;

        if(_isRespawnProcess)
            return;

        Vector3Int cellWorldPosition = tilemapCollider.WorldToCell(transform.position);


        bool onIce = false;
        if (_rigidbody.velocity.magnitude > 0.1f && tilemapCollider.HasTile(cellWorldPosition) && tilemapCollider.GetTile(cellWorldPosition) == tileBaseIce)
        {
            switch ((Direction)lastDirection)
            {
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
            onIce = true;
        }

        if (audioSourceEngine != null) 
            audioSourceEngine.pitch = 0.75f + _rigidbody.velocity.magnitude / _rigidbody.transform.localScale.x *0.04f;  

        if (GetComponent<InputsPlayer>() != null)
            GetComponent<InputsPlayer>().InputsUpdate(onIce);

    }


    public override void OnZeroHealth()
    {
        base.OnZeroHealth();
        StartCoroutine(Respawn());  
       // _rigidbody.MovePosition(_spawnPosition);  
       // SetHealth(100);     
    }

    public void UpdateScore(int deltaScore)
    {
        if (selfIndex >= 0 && selfIndex < Global.MaxPlayersCount)
            Global.score[selfIndex] += deltaScore;
    }
    public void UpdateDestroyedTanksInfo(Tank.Type type)
    {
        if ((int)type >= 0 && (int)type < (int)Tank.Type.MAX_TYPES)
            Global.destroyedTankTypesCounter[selfIndex, (int)type]++;
    }

    public override void Shoot(bool isEnemyProjectile)
    {
        base.Shoot();
        soundSourceFire.PlayOneShot(fireSound);
    }

 
    IEnumerator Respawn()
    {    SetHealth(100); 
        _isRespawnProcess = true; 
        transform.position = new Vector3(1000,1000,0);
        yield return new WaitForSeconds(1.5f); 
        transform.position = _spawnPosition; 
        _isRespawnProcess = false;
         yield return null;
    }
 
}
