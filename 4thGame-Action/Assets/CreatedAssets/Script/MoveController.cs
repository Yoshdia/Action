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

    enum PlayerCombo
    {
        No,
        Once,
        Twice,
        threeTimes,
    }
    PlayerCombo combo;

    /// <summary>
    ///カメラに基づいた移動方向 inputから受け取る
    /// </summary>
    Vector3 inputMovement;

    /// <summary>
    /// ジャンプしていないかどうか
    /// </summary>
    bool notJumping = true;
    JumpByRigidbody jumpComponent;

    bool isAttacking;
    bool canAttack;

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
        canAttack = true;
        notJumping = true;
        isAttacking = false;
        weapon.enabled = false;
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


        if (input.HasAttackInput() && isAttacking && combo != PlayerCombo.No && attackInterval <= comboInterval)
        {
            attackInterval = attackIntervalMax;
            weapon.enabled = true;

            switch (combo)
            {
                case (PlayerCombo.Once):
                    animator.SetBool("Attack2", true);
                    animator.SetBool("Attack1", false);
                    combo = PlayerCombo.Twice;
                    break;
                case (PlayerCombo.Twice):
                    combo = PlayerCombo.threeTimes;
                    animator.SetBool("Attack3", true);
                    animator.SetBool("Attack2", false);
                    break;
                case (PlayerCombo.threeTimes):
                    animator.SetBool("Attack4", true);
                    animator.SetBool("Attack3", false);
                    combo = PlayerCombo.No;
                    break;

            }

        }
        else if (input.HasAttackInput() && !isAttacking && canAttack && combo == PlayerCombo.No)
        {
            canAttack = false;
            isAttacking = true;
            attackInterval = attackIntervalMax;
            animator.SetBool("Attack1", true);
            //animator.SetBool("Attack2", true);
            //animator.SetBool("Attack3", true);
            //animator.SetBool("Attack4", true);

            weapon.enabled = true;
            combo = PlayerCombo.Once;
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

    /// <summary>
    /// オートアタックの間隔、ダメージ
    /// </summary>
    protected float attackInterval;
    [SerializeField]
    protected float attackIntervalMax = 100;

    //float comboInterval = 90;
    float comboInterval = 30;

    private void Attack()
    {
        if (isAttacking)
        {
            attackInterval--;
        }
        if (attackInterval > 0)
        {
            return;
        }
        //attackComponent.Attack("Attack");

        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        animator.SetBool("Attack4", false);
        combo = PlayerCombo.No;
        canAttack = true;
        weapon.enabled = false;
        isAttacking = false;
    }

    private void LateUpdate()
    {
        //attackComponent.AttackMotionEnd("Attack");


    }

    public Transform GetTransform()
    {
        return transform;
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
