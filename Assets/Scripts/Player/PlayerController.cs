using System;
using Game.Player;
using UnityEngine;

namespace RPG.Player
{
    public class PlayerController : MonoBehaviour
    {
        //components
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;
        
        //vars
        private Transform target;
        
        private void Start()
        {
            playerCombat = GetComponent<PlayerCombat>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        public void SetTarget(Transform localTarget)
        {
            target = localTarget;
            Debug.Log(target);
            target.GetComponent<Outline>().enabled = true;
        }
    
        //called by health when player dies
        public void Death()
        {
            playerCombat.Stop();
            playerMovement.enabled = false;
            Debug.Log("Player died");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(3, 252, 211);
            Gizmos.DrawWireSphere(transform.position,10);
        }
    }
}