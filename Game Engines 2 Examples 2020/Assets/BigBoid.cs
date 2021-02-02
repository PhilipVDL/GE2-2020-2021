using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoid : MonoBehaviour
{
    
    public Vector3 velocity;
    public float speed;
    public Vector3 acceleration;
    public Vector3 force;
    public float maxSpeed = 5;
    public float maxForce = 10;

    public float mass = 1;

    public bool seekEnabled = true;
    public Transform seekTargetTransform;
    public Vector3 seekTarget;

    public bool arriveEnabled = false;
    public Transform arriveTargetTransform;
    public Vector3 arriveTarget;
    public float slowingDistance = 10;

    public Path path;
    public bool isFollowPath;
    public int currentWaypoint;
    public bool isFlee;
    public float fleeDistance;
    public Transform fleeTargetTransform;
    public Vector3 fleeTarget;


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + acceleration);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force * 10);

        if (arriveEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(arriveTargetTransform.position, slowingDistance);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        Vector3 desired = toTarget.normalized * maxSpeed;

        return (desired - velocity);
    } 

    public Vector3 Arrive(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        float dist = toTarget.magnitude;
        float ramped = (dist / slowingDistance) * maxSpeed;
        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = (toTarget / dist) * clamped;

        return desired - velocity;
    }

    //flee
    public Vector3 Flee(Vector3 target)
    {
        Vector3 fromTarget = transform.position - target;
        Vector3 desired = fromTarget.normalized * maxSpeed;

        return (desired - velocity);
    }

    public Vector3 CalculateForce()
    {
        Vector3 f = Vector3.zero;
        if (seekEnabled)
        {
            if (seekTargetTransform != null && !isFollowPath)
            {
                seekTarget = seekTargetTransform.position;
            }
            f += Seek(seekTarget);
        }

        if (arriveEnabled)
        {
            if (arriveTargetTransform != null && !isFollowPath)
            {
                arriveTarget = arriveTargetTransform.position;                
            }
            f += Arrive(arriveTarget);
        }

        //flee
        if (isFlee)
        {
            if(fleeTargetTransform != null)
            {
                fleeTarget = fleeTargetTransform.position;
            }

            if(Vector3.Distance(transform.position, fleeTarget) <= fleeDistance)
            {
                f += Flee(fleeTarget);
            }
        }

        return f;
    }

    void Update()
    {
        FollowPath();
        force = CalculateForce();
        acceleration = force / mass;
        velocity = velocity + acceleration * Time.deltaTime;
        transform.position = transform.position + velocity * Time.deltaTime;
        speed = velocity.magnitude;
        if (speed > 0)
        {
            transform.forward = velocity;
        }        
    }

    void FollowPath()
    {
        if (isFollowPath)
        {
            if(currentWaypoint < path.waypoints.Length)
            {
                seekTarget = path.waypoints[currentWaypoint];
                arriveTarget = path.waypoints[currentWaypoint];
            }

            if(Vector3.Distance(transform.position,arriveTarget) <= 1f)
            {
                currentWaypoint++;

                if (path.isLooped && currentWaypoint >= path.waypoints.Length)
                {
                    currentWaypoint = 0;
                }
            }
        }
    }
}