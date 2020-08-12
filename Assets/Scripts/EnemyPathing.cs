using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig WaveConfig;
    List<Transform> waypoints;    
    int waypointIndex = 0;
    void Start()
    {
        waypoints = WaveConfig.GetWaypoints();        
        transform.position = waypoints[waypointIndex].transform.position;
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.WaveConfig = waveConfig;
    }

    // Update is called once per frame
    void Update()
    {
        if (waypointIndex <= waypoints.Count-1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = WaveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
                waypointIndex++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
