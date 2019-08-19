using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonController : MonoBehaviour
{
    Animator animator;

    enum SkelltonState
    {
        AutoAttacking,
        SkillChaging,
    }
    SkelltonState myState;

    /// <summary>
    /// 移動を停止する距離
    /// </summary>
    [SerializeField]
    float movingDistance = 0.5f;

    /// <summary>
    /// 動的に変化する移動速度
    /// </summary>
    float moveSpeed = 0;

    /// <summary>
    /// 移動速度の最大
    /// </summary>
    [SerializeField]
    float moveSpeedMax = 0.1f;

    public MoveController target;

    float attackInterval;
    [SerializeField]
     float attackIntervalMax = 100;

    [SerializeField]
     float autoAttackDamage = 1;

    /// <summary>
    /// スキルプレハブ
    /// </summary>
    [SerializeField]
    GameObject skillPrehub;

    /// <summary>
    /// スキルチャージ中に発生するエフェクト
    /// </summary>
    [SerializeField]
    GameObject skillEffect;

    GameObject instantSkillEffect;

    /// <summary>
    /// スキル１の予兆が表示される時間
    /// </summary>
    public const float SphereSkillDangerTime = 200;

    /// <summary>
    /// 動的に変化する、予兆発生からモーション再生までのインターバル
    /// </summary>
    float skillDangerTime;

    /// <summary>
    /// スキルプレハブを設置したことを表すフラグ
    /// </summary>
    bool skillPlaced;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        instantSkillEffect = null;
        myState = SkelltonState.SkillChaging;
        attackInterval = attackIntervalMax;
    }

    // Update is called once per frame
    void Update()
    {
        attackInterval--;
    }


    private void FixedUpdate()
    {
        if (myState == SkelltonState.AutoAttacking)
        {
            Move();
        }
        else
        {
            SkillPlace();
        }
    }

    void Move()
    {
        moveSpeed += 0.002f;
        if (moveSpeed > moveSpeedMax)
        {
            moveSpeed = moveSpeedMax;
        }
        Vector3 targetPosition = target.GetPosition();
        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance > movingDistance)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), 0.3f);
            transform.position += transform.forward * moveSpeed;
            animator.SetBool("Walk", true);
        }
        else
        {
            moveSpeed = 0;
            animator.SetBool("Walk", false);
            Attack();
        }
    }

    void Attack()
    {
        if (attackInterval < 0)
        {
            attackInterval = attackIntervalMax;
            animator.SetBool("Attack", true);
            target.AddDamage(autoAttackDamage);
            Debug.Log("attacked!");
        }
    }

    void SkillPlace()
    {
        if (skillPlaced)
        {
            if(skillDangerTime<0)
            {
                animator.SetBool("Skill", true);
                myState = SkelltonState.AutoAttacking;
                Destroy(instantSkillEffect.gameObject);
                instantSkillEffect = null;
            }
            else
            {
                skillDangerTime--;
            }
        }
        else
        {
            skillPlaced = true;
            skillDangerTime = SphereSkillDangerTime;
            GameObject skill = Instantiate(skillPrehub, transform.position, new Quaternion());
            skill.transform.parent = transform.parent;
            Quaternion effectQua = new Quaternion();
            effectQua = Quaternion.Euler(new Vector3(-90, 0, 0));
            instantSkillEffect= Instantiate(skillEffect, transform.position, effectQua);
            instantSkillEffect.transform.parent = transform.parent;
        }
    }


    private void LateUpdate()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Skill", false);
    }
}
