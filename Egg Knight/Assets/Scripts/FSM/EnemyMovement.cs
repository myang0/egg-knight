using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private bool _usesPathfinding = true;

    private Transform _playerTransform;
    private float nextWaypointDistance = 2f;
    private Path _path;
    private int _currentWaypoint;
    private bool _reachedEndOfPath;
    private Seeker _seeker;
    private Rigidbody2D _rb;
    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _seeker = GetComponent<Seeker>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (_usesPathfinding) {
            InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
        }
    }
    
    void UpdatePath() {
        if (_seeker.IsDone()) _seeker.StartPath(_rb.position, _playerTransform.position, OnPathComplete);
    }
    
    private void OnPathComplete(Path p) {
        if (p.error) return;
        _path = p;
        _currentWaypoint = 0;
    }

    public void MoveToPlayer(float currentSpeed) {
        if (_path == null) return;
        _reachedEndOfPath = _currentWaypoint >= _path.vectorPath.Count;

        if (_currentWaypoint >= _path.vectorPath.Count) return;
        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 movementForce = direction * currentSpeed;
        // Vector2 movementForce = direction * _currentSpeed * Time.deltaTime;
    
        _rb.velocity = movementForce;
        // _rb.AddForce(movementForce);

        float distanceToNextWaypoint = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
        if (distanceToNextWaypoint < nextWaypointDistance) {
            _currentWaypoint++;
        }
    
    }
  
    public void Flee(float currentSpeed) {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 vectorFromPlayer = VectorHelper.GetVectorToPoint(playerPos, transform.position);

        _rb.velocity = vectorFromPlayer * currentSpeed;
    }
    
}
