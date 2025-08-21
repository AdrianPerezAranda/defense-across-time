using DG.Tweening;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;


public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;
    NavMeshAgent agent;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        enemy.onStartPathToBase.AddListener(FollowPathToBase);
        enemy.onStartAttacking.AddListener(StopMovement);
        enemy.onStartDying.AddListener(StopMovement);
    }
    private void OnDisable()
    {
        enemy.onStartPathToBase.RemoveListener(FollowPathToBase);
        enemy.onStartAttacking.RemoveListener(StopMovement);
        enemy.onStartDying.RemoveListener(StopMovement);
    }

    private void Update()
    {
        if(enemy.estado == Enemy.Estados.FollowingPlayer)
        {
            ChasePlayer();
        }

        transform.rotation.SetLookRotation(agent.destination);
    }

    /// <summary>
    /// Funcion que le dice al nav mesh agent que tiene que ir a la base
    /// </summary>
    private void FollowPathToBase()
    {
        agent.SetDestination(GameController.Instance.timeMachine.transform.position);
    }
    private void ChasePlayer()
    {
        agent.SetDestination(enemy.player.transform.position);
    }
    private void StopMovement()
    {
        agent.SetDestination(transform.position);
    }
}
