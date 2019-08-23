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

    enum PlayerState
    {
        Moving,
        Strong,
        Died,
    }

    PlayerState playerState;

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

    bool isFireAttacking;

    bool isHealing;

    bool isStrong;

    int comboCount;

    PlayerSkill[] skill;
    [SerializeField]
    GameObject skillEffect;

    void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<InputController>();
        moveComponent = GetComponent<MoveByRigidbody>();
        jumpComponent = GetComponent<JumpByRigidbody>();
        turnComponent = GetComponent<Turn>();
        attackComponent = GetComponent<AttackAndAnimation>();
        skill = GetComponents<PlayerSkill>();
    }


    private void Start()
    {
        notJumping = true;
        isAttacking = false;
        weapon.enabled = false;
        comboCount = 0;
        isFireAttacking = false;
        isHealing = false;
        isStrong = false;
        playerState = PlayerState.Moving;
    }


    void Update()
    {
        inputMovement = input.GetMovement();

        if (!isStrong)
        {
            if (input.HasJumpInput() > 0 && notJumping)
            {
                notJumping = false;
                animator.SetBool("Jump", true);
                jumpComponent.Jump();
            }

            if (input.HasAttackInput() && !isAttacking && !isFireAttacking)
            {
                isAttacking = true;
                attackComponent.AttackAndIntervalReset("Attack1");
                weapon.enabled = true;
                comboCount++;
            }

            if (input.HasFireInput() > 0 && !isFireAttacking && !isAttacking)
            {
                isFireAttacking = true;
                skill[0].Shot("Fire", transform);
                Instantiate(skillEffect, transform.position, transform.rotation);
            }

            if (input.HasHealInput() > 0 && !isHealing)
            {
                isHealing = true;
                skill[1].Shot("Heal", transform);
                Instantiate(skillEffect, transform.position, transform.rotation);
            }

            if (input.HasStrongInput() > 0 && !isHealing && !isFireAttacking)
            {
                isStrong = true;
                animator.SetBool("StrongAttack", true);
            }
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
        else if (other.tag == "PlayerHealing")
        {
            //Collider[] collide=other;
            Debug.Log("Healed");
        }
    }

    private void FixedUpdate()
    {
        moveComponent.Move(inputMovement);
        Turn();
        Jumping();
        Attack();
        isFireAttacking = skill[0].CountInterval();
        isHealing = skill[1].CountInterval();
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
            bool attackEnd = attackComponent.Attack("Attack1");
            if (attackEnd)
            {
                attackComponent.AttackMotionEnd("Attack1");
                weapon.enabled = false;
                isAttacking = false;
                comboCount = 0;
            }
            else
            {
                if (input.HasAttackInput() && comboCount < 4)
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
        skill[0].StopAnimation("Fire");
        skill[1].StopAnimation("Heal");
        animator.SetBool("StrongAttack", false);
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
