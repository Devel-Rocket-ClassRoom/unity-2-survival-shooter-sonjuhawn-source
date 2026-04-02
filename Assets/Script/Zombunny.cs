using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class Zombunny : MonoBehaviour
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    public Transform target;
    public HitBox hitBox;

    public LayerMask targetLayer;

    private NavMeshAgent agent;
    private Animator EnemyAnimator;
    private Rigidbody enemyRb;

    public ParticleSystem damagedParticle;

    public float traceDistance = 4f;
    public float attackDistance = 1f;
    public float attackInterval = 0.5f;
    public float lastAttackTime;
    private float moveAmount;
    public int health = 200;

    private Status currentStatus;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var pervStatus = currentStatus;
            currentStatus = value;


            switch (currentStatus)
            {
                case Status.Idle:
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    agent.isStopped = true;
                    break;
            }
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponent<Animator>();
        moveAmount = 0f;
    }

    private void Start()
    {
        currentStatus = Status.Idle;
    }

    private void Update()
    {

        switch (currentStatus)
        {
            case Status.Idle:
                moveAmount = 0f;
                UpdateIdle();
                break;
            case Status.Trace:
                moveAmount = 1f;
                UpdateTrace();
                break;
            case Status.Attack:
                moveAmount = 0f;
                UpdateAttack();
                break;
            case Status.Die:
                moveAmount = 0f;
                UpdateDie();
                break;
        }
    }

    private void UpdateIdle()
    {
        if (health < 0)
        {
            currentStatus = Status.Die;
        }
        if (target != null && Vector3.Distance(target.position, transform.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }
        target = FindTarget(traceDistance);
        EnemyAnimator.SetFloat("Move", moveAmount);

    }
    private void UpdateTrace()
    {
        if (health < 0)
        {
            currentStatus = Status.Die;
        }
        //Debug.Log(Vector3.Distance(target.position, transform.position));
        if (target == null || Vector3.Distance(target.position, transform.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }
        if (target != null)
        {
            var find = hitBox.Colliders.Find(x => x.transform == target);
            if (find != null)
            {
                CurrentStatus = Status.Attack;
                return;
            }

        }
        agent.SetDestination(target.position);
        EnemyAnimator.SetFloat("Move", moveAmount);

    }
    private void UpdateAttack()
    {
        if (health < 0)
        {
            currentStatus = Status.Die;
        }
        if (target == null)
        {
            CurrentStatus = Status.Trace;
            return;
        }
        var find = hitBox.Colliders.Find(x => x.transform == target);
        if (find == null)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (Time.time > lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;
            var damagable = target.GetComponent<IDamageable>();
            if (damagable != null)
            {
                damagable.OnDamage(10f, transform.position, -transform.forward);
            }
        }
        EnemyAnimator.SetFloat("Move", moveAmount);
    }
    private void UpdateDie()
    {
        EnemyAnimator.SetTrigger("Die");
        Destroy(gameObject, 3f);
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        if (colliders.Length == 0)
        {
            return null;
        }
        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
        return target.transform;
    }

    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDir)
    {
        Debug.Log(health);
        health -= (int)damage;

        if (damagedParticle != null)
        {
            if (damagedParticle != null)
            {
                damagedParticle.transform.position = hitPoint;
                damagedParticle.transform.forward = hitDir;
                damagedParticle.Play();
            }
        }

        // 죽음 체크
        if (health <= 0)
        {
            CurrentStatus = Status.Die;
        }
    }
}
