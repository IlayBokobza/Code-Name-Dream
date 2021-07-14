using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
    public class ValueTracker : MonoBehaviour
    {
        //enemies tracker
        [SerializeField] private int maxEnemies = 3;
        [SerializeField] private List<GameObject> angryEnemies;
        
        //target tracker
        [SerializeField] public Outline playerTarget;

        public void AddEnemy(GameObject enemy)
        {
            if (CanAttack() && angryEnemies.FindIndex(item => item == enemy) == -1)
            {
                angryEnemies.Add(enemy);
            }
        }

        public void RemoveEnemy(GameObject enemy)
        {
            angryEnemies.Remove(enemy);
        }

        public bool CanAttack()
        {
            return angryEnemies.Count < maxEnemies-1;
        }
        
        //Target tracker
        public void TargetEnemy(GameObject target)
        {
            Outline targetOutline = target.GetComponent<Outline>();
            if(!targetOutline) return;

            if (playerTarget)
            {
                playerTarget.enabled = false;
            }
            
            targetOutline.enabled = true;
            playerTarget = targetOutline;
        }
    }
}