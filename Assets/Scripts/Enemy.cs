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

    protected Vector2Int _currentGridPosition;
    protected Vector2Int _lastTurnPositionAfterEnemyCollide;

    void Start()
    {
        _lastTurnPositionAfterEnemyCollide = _currentGridPosition = Vector2Int.FloorToInt(transform.position);
    }
    void Update()
    { 
         _turnIntervalCounter += Time.deltaTime;

        if (_turnIntervalCounter >= turnInterval)
        {
            direction = Random.Range(0, 4);
            _turnIntervalCounter = 0;
        }
        else {
            bool wasCollideEnemy = false;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 1.0f);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID() && hitCollider.gameObject.name.IndexOf("Enemy") >= 0)
                {
                    wasCollideEnemy = true;
                }
            }

            if (wasCollideEnemy && _currentGridPosition != _lastTurnPositionAfterEnemyCollide)
            {
                direction = Random.Range(0, 4);
                _lastTurnPositionAfterEnemyCollide = Vector2Int.FloorToInt(transform.position);
            }
            else if (_rigidbody.velocity.magnitude < 0.1f)
            {
                direction = Random.Range(0, 4);
            }
        }

        _currentGridPosition = Vector2Int.FloorToInt(transform.position);

        switch ((Direction)direction)
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

        if (_shootIntervalCounter >= shootInterval)
        {
            Shoot(true);
            _shootIntervalCounter = 0;
        }
        _shootIntervalCounter += Time.deltaTime;

    }

}
