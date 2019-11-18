using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    [SerializeField]
    bool _patrolWaiting;

    [SerializeField]
    float _totalWaitTime = 3f;

    [SerializeField]
    float _switchPropability = 0f;

    [SerializeField]
    List<Waypoint> _patrolPoints;


    private NavMeshAgent _navMeshAgent;
    int _currentPatrolIndex=0;
    bool _travelling;
    bool _waiting;
    [SerializeField]
    bool _patrolForward;
    float _waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null)
        {
            Debug.Log("Le navMesh Agent n'est pas attaché au" + gameObject.name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                Debug.Log("Start" + _currentPatrolIndex);
                //_currentPatrolIndex = 0;
                SetDestinationNPC();
            }
            else
            {
                Debug.Log("Nombre de points de passage insuffisant");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_travelling && _navMeshAgent.remainingDistance <=1.0f)
        {
            _travelling = false;

            if(_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestinationNPC();
            }

        }

        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer >= _totalWaitTime)
            {
                _waiting = false;
                ChangePatrolPoint();
                SetDestinationNPC();
            }
        }

    }
    /// <summary>
    /// Selectionne un nouveau point de passage disponible dans la liste 
    /// ajoute une proba d'avance en avant ou en arrière
    /// </summary>
    private void ChangePatrolPoint()
    {
        if(UnityEngine.Random.Range(0f,1f) <= _switchPropability)
        {
            _patrolForward = !_patrolForward;

        }
        
        if (_patrolForward)
        {
            Debug.Log("j'avance" + _currentPatrolIndex);
            _currentPatrolIndex++;

            if(_currentPatrolIndex >= _patrolPoints.Count)
            {
                _currentPatrolIndex = 0;
            }
            //Debug.Log("patrol" + _currentPatrolIndex);

            // modulo
            //_currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;

        }
        else
        {
            _currentPatrolIndex--;
            if( _currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
            /*
            if (--_currentPatrolIndex < 0) { 
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }*/
        }
    }

    private void SetDestinationNPC()
    {
        if (_patrolPoints != null)
        {
            
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            Debug.Log(targetVector);
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
            
        }
    }

    
}
