using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QPathFinder
{
    public class MoveToPoint : MonoBehaviour
    {
        public GameObject playerObj;

        public float playerSpeed = 20.0f;
        public float playerFloatOffset;     // This is how high the player floats above the ground. 
        public float raycastOriginOffset;   // This is how high above the player u want to raycast to ground. 
        public int raycastDistanceFromOrigin = 40;   // This is how high above the player u want to raycast to ground. 
        public bool thoroughPathFinding = false;    // uses few extra steps in pathfinding to find accurate result. 

        public bool useGroundSnap = false;          // if snap to ground is not used, player goes only through nodes and doesnt project itself on the ground. 


        public QPathFinder.Logger.Level debugLogLevel;
        public GameObject currentTarget;


        void Awake()
        {
            QPathFinder.Logger.SetLoggingLevel( debugLogLevel );
            if (currentTarget == null)
            {
                Debug.Log("new target");
                FindNewTarget();
            }
        }
        void Update () 
        {
            
        }

        void FindNewTarget()
        {
            OnTag[] players = FindObjectsOfType<OnTag>().Where<OnTag>(p => !p.IsFrozen).ToArray<OnTag>();
            int distance = Int32.MaxValue;
            foreach (OnTag p in players)
            {
                if(Math.Abs(p.transform.position.x - transform.position.x) + Math.Abs(p.transform.position.y - transform.position.y) < distance)
                {
                    currentTarget = p.gameObject;
                    MoveTo(p.transform.position);
                }
            }
        }

        void MoveTo( Vector3 position )
        {
            {
                PathFinder.instance.FindShortestPathOfPoints( playerObj.transform.position, position,  PathFinder.instance.graphData.lineType, 
                    Execution.Asynchronously,
                    SearchMode.Simple, 
                    delegate ( List<Vector3> points ) 
                    { 
                        //PathFollowerUtility.StopFollowing( playerObj.transform );
                        if ( useGroundSnap )
                        {
                           FollowThePathWithGroundSnap ( points );
                        }
                        else 
                            FollowThePathNormally ( points );

                    }
                 );
            }
        }

        void FollowThePathWithGroundSnap ( List<Vector3> nodes )
        {
            PathFollowerUtility.FollowPathWithGroundSnap ( playerObj.transform, 
                                                        nodes, playerSpeed, Vector3.down, playerFloatOffset, LayerMask.NameToLayer( PathFinder.instance.graphData.groundColliderLayerName ),
                                                        raycastOriginOffset, raycastDistanceFromOrigin );
        }

        void FollowThePathNormally ( List<Vector3> nodes )
        {
            PathFollowerUtility.FollowPath ( playerObj.transform, nodes, playerSpeed );
        }
    }
}
