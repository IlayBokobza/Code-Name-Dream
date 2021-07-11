using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {   
        //data
        [SerializeField] private float standTimeAfterStopAgro = 2f;
        [SerializeField] private float walkingSpeed = 3f;
        [SerializeField] private float runningSpeed = 5.6f;
        
        //components
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private EnemyAI ai;
        
        //vars
        private float timeSinceStartedToStareAtTarget;
        private Vector3 originalPos;
        private Transform target;

        private void Start()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            ai = GetComponent<EnemyAI>();
            originalPos = transform.position;
        }

        public void Stop()
        {
            navMeshAgent.speed = 0;
            navMeshAgent.destination = transform.position;
            animator.SetFloat("movement",0);
        }

        public void MoveTo(Vector3 dest,string speedText)
        {
            float speed = GetSpeedFromString(speedText);
            
            if (dest == transform.position)
            {
                Stop();
                return;
            }
            
            navMeshAgent.speed = speed;
            navMeshAgent.destination = dest;
            transform.LookAt(dest);
            animator.SetFloat("movement",speed);
        }

        public void StareAtTarget()
        {
            timeSinceStartedToStareAtTarget += Time.deltaTime;
            Stop();
            transform.LookAt(target);

            if (timeSinceStartedToStareAtTarget >= standTimeAfterStopAgro)
            {
                MoveTo(originalPos, "walk");
                ai.ResetAgro();
            }
        }

        private float GetSpeedFromString(string speedText)
        {
            switch (speedText.ToLower())
            {
                case "run":
                    return runningSpeed;
                case "walk":
                    return walkingSpeed;
                default:
                    Debug.LogWarning($"{name} asked to move, but didn't give valid speed.");
                    return 0;
            }
        }
    }
}