using UnityEngine;
using System.Collections;

public class CustomerController : MonoBehaviour
{
    [Header("Patrol Route")]//waypoints the customer will travel between
    [SerializeField] private Transform[] patrolPoints;

    [Header("Movement Settings")]//MOvement settings for the patrol
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float waypointDistance = 0.25f;

    [Header("Wait Settings")]//optional pause at each waypoint..make sure that you don't exceed the number of waypoints
    [SerializeField] private float[] waitTimes;

    //     destination in the patrol route
    [SerializeField] private int currentWaypoint;

    //tracks if the customer is currently waiting
    [SerializeField] private bool isWaiting;

    private void Update()
    {
        if (isWaiting) return;//if i'm waiting, then don't patrol!

        patrol();
    }

    private void patrol()
    //move toward the current waypoint
    {
        if (patrolPoints.Length == 0) return;
    Transform targetWaypoint = patrolPoints[currentWaypoint];

        Vector3 direction = targetWaypoint.position - transform.position;
       direction.y = 0f;

        if (direction.magnitude > waypointDistance)//if i'm further away from waypoint then min distance, then move to it
        {
            rotateToward(direction);
           transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            StartCoroutine(waitAtWaypoint());
        }
    }

    private void rotateToward(Vector3 direction)//smoothly face the next waypoint
    {
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private IEnumerator waitAtWaypoint()//pause before moving to the next waypoint
    {
        isWaiting = true;
        float waitTime = 0f;

        if (waitTimes.Length > currentWaypoint)
        {
            waitTime = waitTimes[currentWaypoint];
        }

        //stop and admire the cereal aisle for a moment....
        yield return new WaitForSeconds(waitTime);

        currentWaypoint++;

        //loop back to the beginning when we reach the end!!
        if (currentWaypoint >= patrolPoints.Length)
        {
            currentWaypoint = 0;
        }

        isWaiting = false;
    }

    private void OnDrawGizmosSelected()//show the patrol route in the editor when gizmo is selected
    {
        if (patrolPoints == null || patrolPoints.Length < 2) return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null) continue;

            Gizmos.DrawSphere(patrolPoints[i].position, 0.25f);

            int nextIndex = i + 1;

            if (nextIndex >= patrolPoints.Length)
            {
                nextIndex = 0;
            }

            if (patrolPoints[nextIndex] == null) continue;

            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[nextIndex].position);
        }
    }
}