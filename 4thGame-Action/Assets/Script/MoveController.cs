using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    Animator animator;
    Rigidbody rigidbody;

    InputController input;
    //カメラに基づいた移動方向 inputから受け取る
    Vector3 inputMovement;

    [SerializeField]
    float moveSpeed = 1;

    void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<InputController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    void Update()
    {
        inputMovement = input.GetMovement();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 movement = inputMovement * moveSpeed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }

    private void Turn()
    {
        if (input.HasMoveInput())
        {
            transform.rotation = Quaternion.LookRotation(inputMovement);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", true);
        }
    }
}
