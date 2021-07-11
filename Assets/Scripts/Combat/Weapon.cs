using UnityEngine;

namespace Game.Combat
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Project/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        //public data
        [SerializeField] public float speed = 1f;
        [SerializeField] public float range = 3f;
        [SerializeField] public float damage = 3;
        [SerializeField] public float cooldownOffset = 0f;
        [SerializeField] public bool spawnOnLoad = true;
        
        //data
        [SerializeField] private GameObject weaponPrefab;

        public void Spawn(Transform location)
        {
            Instantiate(weaponPrefab, location);
        }
    }
}