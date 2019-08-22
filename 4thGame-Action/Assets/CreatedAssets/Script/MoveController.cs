using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    Animator animator;
    InputController input;
    MoveByRigidbody moveComponent;
    Turn turnComponent;
    AttackAndAnimation attackComponent;
    [SerializeField]
    BoxCollider weapon;


    /// <summary>
    ///カメラに基づいた移動方向 inputから受け取る
    /// </summary>
    Vector3 inputMovement;

    /// <summary>
    /// ジャンプしていないかどうか
    /// </summary>
    bool notJumping = true;
    JumpByRigidbody jumpComponent;

    /// <summary>
    /// 攻撃中か
    /// </summary>
    bool isAttacking;

    int comboCount;

    void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<InputController>();
        moveComponent = GetComponent<MoveByRigidbody>();
        jumpComponent = GetComponent<JumpByRigidbody>();
        turnComponent = GetComponent<Turn>();
        attackComponent = GetComponent<AttackAndAnimation>();
    }


    private void Start()
    {
        notJumping = true;
        isAttacking = false;
        weapon.enabled = false;
        comboCount = 0;
    }


    void Update()
    {
        inputMovement = input.GetMovement();

        if (input.HasJumpInput() > 0 && notJumping)
        {
            notJumping = false;
            animator.SetBool("Jump", true);
            jumpComponent.Jump();
        }

        if (input.HasAttackInput() && !isAttacking)
        {
            isAttacking = true;
            attackComponent.AttackAndIntervalReset("Attack1");
            weapon.enabled = true;
            comboCount++;
        }
    }

    //接触中にtagが変わるオブジェクトがあるためOnTriggerStay
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AttackingZone")
        {
            Debug.Log("Outi!");
            //AttackingZone側でも発生してから1f後に削除されるような仕様になっているが、複数ヒットしてしまうことがあるためこちらからもDestroy関数を呼ぶ
            Destroy(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        moveComponent.Move(inputMovement);
        Turn();
        Jumping();
        Attack();
    }

    private void Turn()
    {
        if (input.HasMoveInput() && inputMovement != Vector3.zero)
        {
            turnComponent.LookRotate(inputMovement);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    private void Jumping()
    {
        if (notJumping)
            return;
        if (!jumpComponent.ArrivedGround())
        {
            return;
        }
        animator.SetBool("Jump", false);

        notJumping = jumpComponent.CanJumping();
    }

    private void Attack()
    {
        if (isAttacking)
        {
            bool attackEnd=attackComponent.Attack("Attack1");
            if(attackEnd)
            {
                attackComponent.AttackMotionEnd("Attack1");
                weapon.enabled = false;
                isAttacking = false;
                comboCount = 0;
            }
            else
            {
                if(input.HasAttackInput()&&comboCount<4)
                {
                    attackComponent.AttackAndIntervalReset("Attack1");
                    weapon.enabled = true;
                    comboCount++;
                }
            }
        }
    }

    private void LateUpdate()
    {
        //attackComponent.AttackMotionEnd("Attack");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void AddDamage(float damage)
    {

    }

    public void FootR()
    {
    }

    public void FootL()
    {
    }

    public void Hit()
    {
    }
}
