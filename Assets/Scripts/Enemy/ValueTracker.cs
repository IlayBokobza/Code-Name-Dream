using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
    public class ValueTracker : MonoBehaviour
    {
        [SerializeField] private int maxEnemies = 3;
        [SerializeField] private List<GameObject> angryEnemies;

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
    }
}