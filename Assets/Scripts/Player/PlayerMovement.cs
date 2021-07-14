using System.Collections;
using System.Collections.Generic;
using Game.Enemies;
using RPG.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //data
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float maxRunningSpeed = 5.662316f;
        [SerializeField] private ValueTracker tracker;
        
        //controls
        [Header("Controls")]
        [SerializeField] private InputAction movementControls;
        [SerializeField] private InputAction targetBtn;

        //var
        private float turnSmoothVelocity;
        private Transform target;

        //components
        private CharacterController characterController;
        private Animator animator;
        private PlayerController playerController;
    
        // Start is called before the first frame update
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            
            Cursor.lockState = CursorLockMode.Locked;
            movementControls.Enable();
            targetBtn.Enable();

            targetBtn.performed += _ => TargetEnemy();
        }

        // Update is called once per frame
        private void Update()
        {
            Movement();

            if (target)
            {
                transform.LookAt(target);
            }
            else
            {
                LookForTarget();
            }
        }

        private void TargetEnemy()
        {
            if(!tracker.playerTarget) return;

            target = target ? null : tracker.playerTarget.transform;
        }
        
        private void LookForTarget()
        {
            Vector3 originPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            bool didHit = Physics.Raycast(originPos,transform.forward,out var hit, Mathf.Infinity);
            Debug.DrawRay(originPos,transform.forward * 1000, Color.yellow);

            if (didHit && hit.collider.gameObject.layer == 6)
            {
                tracker.TargetEnemy(hit.collider.gameObject);
            }
            
        }

        private void Movement()
        {
            //speed and input
            Vector2 input = movementControls.ReadValue<Vector2>();

            float vertical = -input.y;
            float horizontal = -input.x;
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
            float speed = input.magnitude * maxRunningSpeed;
            animator.SetFloat("movement",speed);

            //moves the player somehow??
            //change later
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; //+ Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,turnSmoothTime);
                transform.rotation = Quaternion.Euler(0,angle,0);

                Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                float previousY = transform.position.y;
                characterController.Move(moveDir.normalized * (speed * Time.deltaTime));
                transform.position = new Vector3(transform.position.x, previousY, transform.position.z);
            }
        }
    }
}