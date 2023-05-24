using BeeState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bee : MonoBehaviour
{

    private State curState;
    public float detectRange;
    public float moveSpeed;
    public float attackRange;
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

namespace BeeState
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size, }

    public class IdleState : StateBase
    {
        private Bee bee;
        private float idleTime;

        public IdleState(Bee bee)
        {
            this.bee = bee;
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
                bee.patrolIndex = (bee.patrolIndex + 1) % bee.patrolPoints.Length;
                bee.StateChange(State.Patrol);
            }
            idleTime += Time.deltaTime;
            if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.StateChange(State.Trace);
            }
        }
    }

    public class TraceState : StateBase
    {
        private Bee bee;

        public TraceState(Bee bee)
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
        private Bee bee;

        public ReturnState(Bee bee)
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

        private Bee bee;

        public AttackState(Bee bee)
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
            if (Vector2.Distance(bee.player.position, bee.transform.position) >  bee.attackRange)
            {
                bee.StateChange(State.Trace);
            }
        }
    }

    public class PatrolState : StateBase
    {
        private Bee bee;

        public PatrolState(Bee bee)
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
}
