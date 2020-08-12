using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject PathPrefab;
    [SerializeField] float TimeBetweenSpawns = 0.5f;
    [SerializeField] float SpawnRandomFactor = 0.3f;
    [SerializeField] int NumberOfEnemies = 5;
    [SerializeField] float MoveSpeed = 2f;


    public GameObject GetEnemeyPrefab(){return EnemyPrefab; }    

    public List<Transform> GetWaypoints() 
    {
        List<Transform> waypoints = new List<Transform>();

        foreach (Transform item in PathPrefab.transform)
        {
            waypoints.Add(item);
        }

        return waypoints;
    }

    public float GetTimeBetweenSpawns() { return TimeBetweenSpawns; }
    public float GetSpawnRandomFactor() { return SpawnRandomFactor; }
    public int GetNumberOfEnemies() { return NumberOfEnemies; }
    public float GetMoveSpeed() { return MoveSpeed; }




}
