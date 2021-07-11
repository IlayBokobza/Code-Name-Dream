using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //data
        [SerializeField] private InputAction movementControls;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float maxRunningSpeed = 5.662316f;
    
        //var
        private float turnSmoothVelocity;
    
        //components
        private CharacterController characterController;
        private Animator animator;
    
        // Start is called before the first frame update
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            
            Cursor.lockState = CursorLockMode.Locked;
            movementControls.Enable();
        }

        // Update is called once per frame
        private void Update()
        {
            Movement();
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