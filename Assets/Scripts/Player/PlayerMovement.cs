using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    Rigidbody rb => CharacterManager.Instance.Player.rigidBody;

    Vector2 movement;
    
    [Header("Movement")]
    [SerializeField] float speed = 10f;
    [SerializeField] private float jumpPower = 80f;
    public LayerMask groundLayerMask;


    void Start()
    {
        PlayerInputController inputController = CharacterManager.Instance.Player.inputController;
        inputController.OnMoveEvent += OnMove;
        inputController.OnJumpEvent += OnJump;
    }
    
    void FixedUpdate()
    {
        // rb.velocity = 
        Move();
    }


    void OnMove(Vector2 dir)
    {
        movement = dir;
    }

    void Move()
    {
        Vector3 move = transform.forward * movement.y + transform.right * movement.x;
        move *= speed;
        move.y += rb.velocity.y;

        rb.velocity = move;
    }

    void OnJump()
    {
        if(IsGrounded())
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + -transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + transform.right * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + -transform.right * 0.2f + transform.up * 0.01f, Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        
        return false;
    }

}
