using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Vector3[] waypoints;
    public bool isLooped;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(waypoints[0], waypoints[1]);
        Gizmos.DrawLine(waypoints[1], waypoints[2]);
        Gizmos.DrawLine(waypoints[2], waypoints[3]);
        if (isLooped)
        {
            Gizmos.DrawLine(waypoints[3], waypoints[0]);
        }

        for(int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i], 1);
        }
    }
}