using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    
    private Animator _animator;
    private NavMeshAgent _agent;
    
    private bool isOnWaypoint () => _agent.remainingDistance > _agent.stoppingDistance;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }
    [ContextMenu("MoveToNextWaypoint")]
    public void MoveToNextWaypoint(Vector3 nextPos)
    {
        _agent.SetDestination(nextPos);
        _animator.SetBool("Run", true);
        StartCoroutine(NavMeshStatus());
    }

    [ContextMenu("Idle")]
    private void Idle()
    {
        _animator.SetBool("Run", false);
    }

    private IEnumerator NavMeshStatus()
    {
        while (_agent.pathPending)
        {
            yield return new WaitForFixedUpdate();
        }
        while (isOnWaypoint())
        {
            yield return new WaitForFixedUpdate();
        }
        Idle();
        GameManager.instance.ReachedWaypoint();
    }


}