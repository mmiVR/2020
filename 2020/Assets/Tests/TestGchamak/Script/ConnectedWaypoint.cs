using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Test.TestGChamak.Script { 
public class ConnectedWaypoint : Waypoint
{
        [SerializeField]
        protected float _connectivityRadius = 50f;

        [SerializeField]
        List<ConnectedWaypoint> _connections;

        
        

        // Start is called before the first frame update
        void Start()
    {
            // récupere tous les waypoint de la scene
           
            GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("WP");
            _connections = new List<ConnectedWaypoint>();

           


            // check si il y a un waypint conencte
            for (int i=0; i<allWaypoints.Length;i++)
            {
                ConnectedWaypoint nexWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();
                
                // si on trouve un waypoint
                if( nexWaypoint != null && nexWaypoint !=this)
                {
                    
                    if(Vector3.Distance(this.transform.position,nexWaypoint.transform.position) <= _connectivityRadius)
                    {
                        _connections.Add(nexWaypoint);
                        
                    }
                }
            }

           
    
    }

        public override void OnDrawGizmos()
        {
           
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _connectivityRadius);
            
        }
        

        public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
        {
            ConnectedWaypoint nextWaypoint;

            if (_connections.Count == 0)
            {
                //pas de waypoint
                Debug.LogError("Pas assez de points de passage");
                return null;
            }

            else if(_connections.Count == 1 && _connections.Contains(previousWaypoint))
            {
                // seulement 1 seul point de passage 
                return previousWaypoint;
            }

            else
            {
                // trouve un point de passage au hasard
               
                
                int nextIndex = 0;
                do
                {
                    nextIndex = UnityEngine.Random.Range(0, _connections.Count);
                    nextWaypoint = _connections[nextIndex];
                
                } while (nextWaypoint == previousWaypoint);
                return nextWaypoint;
            }

        }

    }
}
