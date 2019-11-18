using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Test.TestGChamak.Script
{

    public class NPCConnectedPatrol : MonoBehaviour
    {
        [SerializeField]
        bool _patrolWaiting;

        [SerializeField]
        float _totalWaitTime = 3f;

        [SerializeField]
        float _switchPropability = 0.2f;


        private NavMeshAgent _navMeshAgent;
        ConnectedWaypoint _currentWaypoint;
        ConnectedWaypoint _previousWaypoint;

        int _currentPatrolIndex;
        bool _travelling;
        bool _waiting;
        bool _patrolForward;
        float _waitTimer;
        int _waypointsVisited;

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
                if (_currentWaypoint == null)
                {
                    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("WP");

                    if (allWaypoints.Length > 0)
                    {
                        while (_currentWaypoint == null)
                        {
                            int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                            
                            ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                            if (startingWaypoint != null)
                            {
                                
                                _currentWaypoint = startingWaypoint;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Pas de waypoint sur la scene");
                    }
                }
            }

            SetDestinationNPC();
        }

        // Update is called once per frame
        void Update()
        {
            if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
            {
                _travelling = false;
                _waypointsVisited++;

                if (_patrolWaiting)
                {
                    _waiting = true;
                    _waitTimer = 0f;
                }
                else
                {

                    SetDestinationNPC();
                }

            }

            if (_waiting)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer >= _totalWaitTime)
                {
                    _waiting = false;

                    SetDestinationNPC();
                }
            }

        }

        private void SetDestinationNPC()
        {
            if (_waypointsVisited > 0)
            {
                //Debug.Log("ici");
                ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
                //Debug.LogError(nextWaypoint.transform.position);
                _previousWaypoint = _currentWaypoint;
                _currentWaypoint = nextWaypoint;
            }

            Vector3 targetVector = _currentWaypoint.transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
        }

    }

}
