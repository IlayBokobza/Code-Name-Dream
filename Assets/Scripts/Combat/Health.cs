using UnityEngine;

namespace Game.Combat
{
    public class Health : MonoBehaviour
    {
        //data
        [SerializeField] private float health = 50f;
        
        //components
        private Animator animator;

        //vars
        public bool isDead;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void DecreaseHealth(float healthAmount)
        {
            health -= healthAmount;

            if (health <= 0)
            {
                isDead = true;
            }
        }

        //called when takes damage
        private void TookDamage(int damage)
        {
            if(isDead) return;
            
            DecreaseHealth(damage);

            if (isDead)
            {
                animator.SetTrigger("die");
                BroadcastMessage("Death");
            }
        }
        
    }
}