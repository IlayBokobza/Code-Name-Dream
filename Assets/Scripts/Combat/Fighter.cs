using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour
    {
        //data
        [Header("Weapon")]
        [SerializeField] public Weapon weapon;
        [SerializeField] private Transform weaponSpawnLocation;

        [Header("Data")] 
        [SerializeField] private ParticleSystem attack2Effect;

        //components
        private Animator animator;
        private float attackCooldown;
    
        //vars
        private bool canAttack = true;
        private float timeSinceLastAttack;
        private int currentAttack = 1;

        private void Start()
        {
            //null backup for weapon
            if (weapon == null)
            {
                weapon = ScriptableObject.CreateInstance<Weapon>();
                weapon.spawnOnLoad = false;
            }
            
            //get components
            animator = gameObject.GetComponent<Animator>();
            animator.SetFloat("attackSpeed",weapon.speed);

            if (weapon.spawnOnLoad)
            {
                weapon.Spawn(weaponSpawnLocation);
            }
        }

        public void Attack()
        {
            if(!canAttack) return;
            timeSinceLastAttack = 0;
            currentAttack = 1;
            canAttack = false;
            animator.SetTrigger("attack");

            //sets attack cooldown
            attackCooldown = animator.GetCurrentAnimatorStateInfo(0).speed / weapon.speed - weapon.cooldownOffset;
            StartCoroutine(CountAttackTime());
        }

        public void Attack2()
        {
            if(!canAttack) return;
            timeSinceLastAttack = 0;
            currentAttack = 2;
            canAttack = false;
            
            animator.SetTrigger("attack2");

            //sets attack cooldown
            attackCooldown = animator.GetCurrentAnimatorStateInfo(0).speed / weapon.speed - weapon.cooldownOffset;
            StartCoroutine(CountAttackTime());
        }

        private void PlayEffect(ParticleSystem effect)
        {
            if (effect != null)
            {
                effect.Play();
            }
        }
        
        private IEnumerator CountAttackTime()
        {
            while (timeSinceLastAttack < attackCooldown)
            {
                timeSinceLastAttack += Time.deltaTime;
                yield return null;
            }
        
            canAttack = true;
        }
        
        //called by animation
        private void Hit()
        {
            Vector3 hitStartPos = transform.position + new Vector3(0, transform.position.y + 1, 0);
            RaycastHit[] hits = Physics.SphereCastAll(hitStartPos,weapon.range, transform.forward,weapon.range);
            List<GameObject> hitOnEnemies = new List<GameObject>();

            //get all hits
            foreach (RaycastHit hit in hits)
            {
                GameObject hitObject = hit.collider.gameObject;
                if(tag == hitObject.tag) continue;
                if (hitObject.tag == "Enemy" || hitObject.tag == "Player")
                {
                    if (!hitObject.GetComponent<Health>().isDead)
                    {
                        hitOnEnemies.Add(hitObject);
                    }
                }
            }
            
            //take damage from hit enemies
            foreach (GameObject enemy in hitOnEnemies)
            {
                AttackHandler(enemy,hitOnEnemies.Count);
            }
        }

        private void AttackHandler(GameObject enemy,int enemiesCount)
        {
            switch (currentAttack)
            {
                case 1:
                    enemy.BroadcastMessage("TookDamage",weapon.damage/enemiesCount);
                    break;
                case 2:
                    PlayEffect(attack2Effect);
                    // enemy.GetComponent<EnemyAI>().FlyBack();
                    break;
            }
        }
    }
}