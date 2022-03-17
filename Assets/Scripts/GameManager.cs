using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Serializable]
    private struct Waypoint
    {
        public Transform waypoint;
        public EnemyGroup enemyGroup;
    }
    public static GameManager instance { get; private set; }
    [HideInInspector] public bool isGameStarted;
    public bool CanMove() => waypoints[_currentWaypointIndex].enemyGroup == null || !waypoints[_currentWaypointIndex].enemyGroup.IsEnemiesActive();
    [SerializeField] private List<Waypoint> waypoints;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private CinemachineVirtualCamera cameraToPlayer;
    [SerializeField] private CinemachineVirtualCamera cameraToEnemies;
    private int _currentWaypointIndex;
    

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        playerMove.transform.position = waypoints[0].waypoint.position;
        StartCoroutine(StartHandle());
    }  
    public void ReachedWaypoint()
    {
        if (_currentWaypointIndex == waypoints.Count - 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (CanMove())
            MovePlayerToNextWaypoint();
        else
        {
            cameraToEnemies.LookAt = waypoints[_currentWaypointIndex].enemyGroup.transform;
            cameraToPlayer.enabled = false;
            cameraToEnemies.enabled = true;
        }

    }
    public void EnemyKilled()
    {
        if (CanMove())
        {
            MovePlayerToNextWaypoint();
        }
    }

    private void MovePlayerToNextWaypoint()
    {
        _currentWaypointIndex++;
        playerMove.MoveToNextWaypoint(waypoints[_currentWaypointIndex].waypoint.position);
        cameraToPlayer.enabled = true;
        cameraToEnemies.enabled = false;
    }

    private IEnumerator StartHandle()
    {
        
        while (!Input.GetMouseButtonDown(0) && Input.touchCount == 0)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForFixedUpdate();
        isGameStarted = true;
        MovePlayerToNextWaypoint();
    }
}
