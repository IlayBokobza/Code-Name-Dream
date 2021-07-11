using System;
using System.Collections;
using Game.Combat;
using RPG.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyAI : MonoBehaviour
    {
        //data
        [SerializeField] private Transform target;
        [SerializeField] private ValueTracker tracker;

        [Header("Behaviour")]
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float attackRange = 3f;
        [SerializeField] private float chearRange = 10f;
        [SerializeField] private float callForBackupRange = 50f;
        [SerializeField] private float stopAgroTime = 5f;

        public GameObject placeholderPrefab;
        private GameObject placeHolder;
        
        //components
        private NavMeshAgent navMeshAgent;
        private Fighter fighter;
        private EnemyMovement mover;
        
        //state
        private bool isDead;
        private bool isAttacking;
        private bool isChearing;
        private bool onTarget;
        
        //vars
        private LayerMask layerMask;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<EnemyMovement>();
            layerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));

            placeHolder = Instantiate(placeholderPrefab, transform.position, Quaternion.identity);
        }

        private void Update()
        {
            if(isDead) return;
            float distanceFromTarget = Vector3.Distance(transform.position, target.position);

            if (distanceFromTarget <= chaseRange && !onTarget)
            {
                onTarget = true;
                CallForBackup();
            }

            if (onTarget)
            {
                if (tracker.CanAttack() || isAttacking)
                {
                    if (tracker.CanAttack())
                    {
                        tracker.AddEnemy(gameObject);
                        isAttacking = true;
                        isChearing = false;
                        Destroy(placeHolder);
                    }

                    if (distanceFromTarget <= attackRange)
                    {
                        Attack();
                    }
                    else
                    {
                        ChaseTarget();
                    }
                }
                else if (!tracker.CanAttack())
                {
                    if (Math.Abs(distanceFromTarget - chearRange) > 1)
                    {
                        Vector3 newPos = transform.position + transform.forward * (distanceFromTarget - chearRange);
                        placeHolder.transform.position = newPos;

                        string speed;
                        //gets speed
                        if ((distanceFromTarget - chearRange) > 0)
                        {
                            speed = "run";
                        }
                        else
                        {
                            speed = "walk";
                        }
                        
                        mover.MoveTo(newPos,speed);
                        transform.LookAt(target);
                    }
                    else
                    {
                        mover.Stop();
                    }
                }
                else if (target.GetComponent<Health>().isDead)
                {
                    TargetDeath();
                }
                else
                {
                    ChaseTarget();
                }
            }
        }

        private void CallForBackup()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, callForBackupRange, transform.forward, 10000,layerMask);

            foreach (RaycastHit hit in hits)
            {
                hit.collider.GetComponent<EnemyAI>().CalledForBackup();
            }
        }

        private void CalledForBackup()
        {
            if(onTarget) return;
            onTarget = true;
            CalledForBackup();
        }

        private void Attack()
        {
            mover.Stop();
            fighter.Attack();   
        }

        private void Chear()
        {
            mover.Stop();
        }

        private void TargetDeath()
        {
            
        }

        private void ChaseTarget()
        {
            mover.MoveTo(target.position,"run");
        }

        private void Death()
        {
            ResetAgro();
            tracker.RemoveEnemy(gameObject);
            GetComponent<Collider>().enabled = false;
            isDead = true;
        }

        public void ResetAgro()
        {
            isAttacking = false;
            isChearing = false;
            onTarget = false;
        }


        private void OnDrawGizmosSelected()
        {
            //chase range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,chaseRange);
        
            //attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,attackRange);
            
            //call for backup range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,callForBackupRange);
        }
    }
}