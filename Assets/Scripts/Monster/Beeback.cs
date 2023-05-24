using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBack : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, Patrol }

    private State curState;
    [SerializeField] private float detectRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform[] patrolPoints;

    private Transform player;
    private Vector3 returnPosition;
    private int patrolIndex = 0;
    float lastAttackTime = 0;

    private void Start()
    {
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        switch (curState)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Trace:
                TraceUpdate();
                break;
            case State.Return:
                ReturnUpdate();
                break;
            case State.Attack:
                AttackUpdate();
                break;
            case State.Patrol:
                PatrolUpdate();
                break;
        }
    }
    float idleTime = 0;
    private void IdleUpdate()
    {
        if (idleTime > 2)
        {
            idleTime = 0;
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            curState = State.Patrol;
        }
        idleTime += Time.deltaTime;
        if (Vector2.Distance(player.position, transform.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

    private void TraceUpdate()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(player.position, transform.position) > detectRange)
        {
            curState = State.Return;
        }
        else if (Vector2.Distance(player.position, transform.position) < attackRange)
        {
            curState = State.Attack;
        }
    }

    private void ReturnUpdate()
    {
        Vector2 dir = (returnPosition - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, returnPosition) < 0.02f)
        {
            curState = State.Idle;
        }
        else if (Vector2.Distance(player.position, transform.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

    private void AttackUpdate()
    {
        if (lastAttackTime > 1)
        {
            Debug.Log("АјАн");
            lastAttackTime = 0;
        }
        lastAttackTime += Time.deltaTime;
        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            curState = State.Trace;
        }
    }

    private void PatrolUpdate()
    {

        Vector2 dir = (patrolPoints[patrolIndex].position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoints[patrolIndex].position) < 0.02f)
        {
            curState = State.Idle;
        }
        else if (Vector2.Distance(player.position, transform.position) < detectRange)
        {
            curState = State.Trace;
        }
    }
}
