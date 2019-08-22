using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonController : MonoBehaviour
{
    /// <summary>
    /// 移動や攻撃の対象になるオブジェクト
    /// </summary>
    [SerializeField]
    protected MoveController target;

    Animator animator;

    HeadForTargetByMove headForTarget;
    Turn turnComponent;
    AttackAndAnimation attackComponent;
    EnemySkillPlace skillPlaceComponent;
    DeadEnemy deadComponent;


    enum SkeltonState
    {
        AutoAttacking,
        SkillChaging,
    }
    SkeltonState myState;

    enum SkeltonSkill
    {
        SphereExplosion,
        Search,
    }
    SkeltonSkill skill;

    [SerializeField]
    protected float autoAttackDamage = 1;

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
    float moveSpeedMax = 7.0f;

    [SerializeField]
     GameObject sphereSkill;
    [SerializeField]
     GameObject searchSkill;
    

    /// <summary>
    /// スキルチャージ中にSkeltonを中心に発生するエフェクト
    /// </summary>
    [SerializeField]
    GameObject sphereSkillEffect;

    /// <summary>
    /// チャージ終了後のエフェクトを変更、削除するために格納しておく変数
    /// </summary>
    GameObject instantSkillEffect;

    /// <summary>
    /// スキルプレハブを設置したことを表すフラグ
    /// </summary>
    bool skillPlaced;

    [SerializeField]
    GameObject HitEffect;

    float hitPoint;
    [SerializeField]
    float MaxHitPoint = 10;

    void Awake()
    {
        headForTarget = GetComponent<HeadForTargetByMove>();
        turnComponent = GetComponent<Turn>();
        attackComponent = GetComponent<AttackAndAnimation>();
        animator = GetComponent<Animator>();
        skillPlaceComponent = GetComponent<EnemySkillPlace>();
        deadComponent = GetComponent<DeadEnemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hitPoint = MaxHitPoint;
        instantSkillEffect = null;
        myState = SkeltonState.AutoAttacking;
        skill = SkeltonSkill.Search;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (myState == SkeltonState.AutoAttacking)
        {
            Move();
            SkillChange();
            Dead();
        }
        else
        {
            SkillPlace();
        }
    }

    void Move()
    {
        moveSpeed += 0.2f;
        if (moveSpeed > moveSpeedMax)
        {
            moveSpeed = moveSpeedMax;
        }
        Vector3 targetPosition = target.GetPosition();
        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance > movingDistance)
        {
            turnComponent.LookRotate(targetPosition - transform.position);
            headForTarget.HeadForTarget(targetPosition, moveSpeed);
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
        bool attacked = attackComponent.Attack("Attack");
        if (attacked)
        {
            target.AddDamage(autoAttackDamage);
            Debug.Log("attacked!");
        }
    }

    void SkillPlace()
    {
        if (skillPlaced)
        {
            bool attacked = skillPlaceComponent.DangerSkillAttacked("SkillChaging", "Skill");
            if (attacked)
            {
                myState = SkeltonState.AutoAttacking;
                Destroy(instantSkillEffect.gameObject);
                instantSkillEffect = null;
                skillPlaced = false;
            }
        }
        else
        {
            skillPlaced = true;
            Quaternion effectQua = new Quaternion();
            effectQua = Quaternion.Euler(new Vector3(-90, 0, 0));
            instantSkillEffect = Instantiate(sphereSkillEffect, transform.position, effectQua);
            instantSkillEffect.transform.parent = transform;

            GameObject ShotSkill = null;

            Vector3 targetPosition = transform.position;
            switch (skill)
            {
                case (SkeltonSkill.SphereExplosion):
                    ShotSkill = sphereSkill;
                    break;
                case (SkeltonSkill.Search):
                    ShotSkill = searchSkill;
                    targetPosition = target.GetPosition();
                    break;
                default:
                    ShotSkill = sphereSkill;
                    break;
            }

            skillPlaceComponent.SkillPlace(ShotSkill, targetPosition, "SkillChaging");
        }
    }

    float timeLine;

    void SkillChange()
    {
        timeLine++;
        if (timeLine == 500)
        {
            myState = SkeltonState.SkillChaging;
            skill = SkeltonSkill.SphereExplosion;
        }
        else if (timeLine == 1500)
        {
            myState = SkeltonState.SkillChaging;
            skill = SkeltonSkill.Search;
        }
        else if (timeLine > 2000)
        {
            timeLine = 0;
        }
    }

    void Dead()
    {
        deadComponent.Dead(hitPoint, "Down");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            Vector3 effectPos = transform.position;
            effectPos.y += 2.0f;
            Instantiate(HitEffect, effectPos, new Quaternion());
            hitPoint--;
        }
    }

    void LateUpdate()
    {
        attackComponent.AttackMotionEnd("Attack");
        skillPlaceComponent.StopSkillAttackAnimation("Skill");
    }
}
