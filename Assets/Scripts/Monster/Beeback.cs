using BeebackState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Beeback : MonoBehaviour
{
    private State curState;
    public float detectRange;
    public float moveSpeed;
    public float attackRange;
    public float RunawayRange;
    public GameObject something;
    public UnityEngine.Transform somethingPoint;
    public UnityEngine.Transform[] patrolPoints;

    public StateBase[] states;
    private State curstate;
    public UnityEngine.Transform player;
    public Vector3 returnPosition;
    public int patrolIndex = 0;


    private void Awake()
    {
        states = new StateBase[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Patrol] = new PatrolState(this);
        states[(int)State.Runaway] = new RunawayState(this);
    }
    private void Start()
    {
        curState = State.Idle;
        states[(int)curState].Enter();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }
    private void Update()
    {
        states[(int)curState].Update();
    }
    public void StateChange(State state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }
}

namespace BeebackState
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Runaway, Size }

    public class IdleState : StateBase
    {
        private Beeback beeback;
        private float idleTime;

        public IdleState(Beeback bee)
        {
            this.beeback = bee;
        }
        public override void Enter()
        {
            Debug.Log("IdleEnter");
        }
        public override void Exit()
        {
            Debug.Log("IdleExit");
        }

        public override void Update()
        {
            if (idleTime > 2)
            {
                idleTime = 0;
                beeback.patrolIndex = (beeback.patrolIndex + 1) % beeback.patrolPoints.Length;
                beeback.StateChange(State.Patrol);
            }
            idleTime += Time.deltaTime;
            if (Vector2.Distance(beeback.player.position, beeback.transform.position) < beeback.detectRange)
            {
                beeback.StateChange(State.Trace);
            }
        }
    }

    public class TraceState : StateBase
    {
        private Beeback bee;

        public TraceState(Beeback bee)
        {
            this.bee = bee;
        }
        public override void Enter()
        {
            Debug.Log("TraceEnter");
        }
        public override void Exit()
        {
            Debug.Log("TraceExit");
        }
        public override void Update()
        {
            Vector2 dir = (bee.player.position - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.detectRange)
            {
                bee.StateChange(State.Return);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.attackRange)
            {
                bee.StateChange(State.Attack);
            }
        }
    }

    public class ReturnState : StateBase
    {
        private Beeback bee;

        public ReturnState(Beeback bee)
        {
            this.bee = bee;
        }
        public override void Enter()
        {
            Debug.Log("ReturnEnter");
        }
        public override void Exit()
        {
            Debug.Log("ReturnExit");
        }
        public override void Update()
        {
            Vector2 dir = (bee.returnPosition - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.transform.position, bee.returnPosition) < 0.02f)
            {
                bee.StateChange(State.Idle);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.StateChange(State.Trace);
            }
        }
    }

    public class AttackState : StateBase
    {
        float lastAttackTime = 0;

        private Beeback bee;

        public AttackState(Beeback bee)
        {
            this.bee = bee;
        }
        public override void Enter()
        {
            Debug.Log("AttackEnter");
        }
        public override void Exit()
        {
            Debug.Log("AttackExit");
        }
        public override void Update()
        {
            if (lastAttackTime > 1)
            {
                Debug.Log("АјАн");
                lastAttackTime = 0;
            }
            lastAttackTime += Time.deltaTime;
            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.attackRange)
            {
                bee.StateChange(State.Trace);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.RunawayRange)
            {
                bee.StateChange(State.Runaway);
            }
        }

        private void Instantiate(object bulletPrefab, object position, object rotation)
        {
            throw new NotImplementedException();
        }
    }

    public class PatrolState : StateBase
    {
        private Beeback bee;

        public PatrolState(Beeback bee)
        {
            this.bee = bee;
        }
        public override void Enter()
        {
            Debug.Log("PatrolEnter");
            bee.patrolIndex = (bee.patrolIndex + 1) % bee.patrolPoints.Length;
        }
        public override void Exit()
        {
            Debug.Log("PatrolExit");
        }
        public override void Update()
        {
            Vector2 dir = (bee.patrolPoints[bee.patrolIndex].position - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.transform.position, bee.patrolPoints[bee.patrolIndex].position) < 0.02f)
            {
                bee.StateChange(State.Idle);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.StateChange(State.Trace);
            }
        }
    }
    public class RunawayState : StateBase
    {
        private Beeback bee;

        public RunawayState(Beeback bee)
        {
            this.bee = bee;
        }
        public override void Enter()
        {
            Debug.Log("RunawayEnter");
        }
        public override void Exit()
        {
            Debug.Log("RunawayExit");
        }
        public override void Update()
        {
            Vector2 dir = -(bee.patrolPoints[bee.patrolIndex].position - bee.transform.position);
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.attackRange)
            {
                bee.StateChange(State.Attack);
            }
        }
    }
}
