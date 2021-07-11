using System;
using System.Collections;
using Game.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(Fighter))]
    public class PlayerCombat : MonoBehaviour
    {
        //data
        [SerializeField] private InputAction attackButton;
        [SerializeField] private InputAction attack2Button;

        //components
        private Fighter fighter;

        private void Start()
        {
            //get components
            fighter = GetComponent<Fighter>();

            //enable controls and assign methods.
            attackButton.Enable();
            attackButton.performed += _ => fighter.Attack();
            
            //enable controls and assign methods.
            attack2Button.Enable();
            attack2Button.performed += _ => fighter.Attack2();
        }

        public void Stop()
        {
            attackButton.Disable();
            attack2Button.Disable();
        }
    }
}