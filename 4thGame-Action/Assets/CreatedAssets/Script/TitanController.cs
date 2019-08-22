using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanController : MonoBehaviour
{
    /// <summary>
    ///攻撃の対象になるオブジェクト
    /// </summary>
    [SerializeField]
    protected MoveController target;

    Animator animator;
    AttackAndAnimation attackComponent;
    EnemySkillPlace skillPlaceComponent;
    DeadEnemy deadComponent;

    enum TitanState
    {
        AutoAttacking,
        SkillChaging,
    }
    TitanState myState;

    enum TitanSkill
    {
        CenterSphere,
        Punch,
        AllRange,
        Line,
    }
    TitanSkill skill;


    [SerializeField]
    float autoAttackDamage = 1;

    [SerializeField]
    GameObject sphereSkill;
    [SerializeField]
    GameObject punchSkill;
    [SerializeField]
    GameObject allRange;
    [SerializeField]
    GameObject line;

    /// <summary>
    /// スキルチャージ中に発生するエフェクト
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
        myState = TitanState.AutoAttacking;
        skill = TitanSkill.CenterSphere;
    }

    private void FixedUpdate()
    {
        if (myState == TitanState.AutoAttacking)
        {
            Attack();
            SkillChange();
            Dead();
        }
        else
        {
            SkillPlace();
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
    float timeLine;

    void SkillChange()
    {
        timeLine++;
        if (timeLine == 500)
        {
            myState = TitanState.SkillChaging;
            skill = TitanSkill.CenterSphere;
        }
        //else if (timeLine == 1500)
        //{
        //    myState = SkeltonState.SkillChaging;
        //    skill = SkeltonSkill.Search;
        //}
        else if (timeLine > 2000)
        {
            timeLine = 0;
        }
    }

    void SkillPlace()
    {
        if (skillPlaced)
        {
            bool attacked = skillPlaceComponent.DangerSkillAttacked("SkillChaging", "Skill");
            if (attacked)
            {
                myState = TitanState.AutoAttacking;
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
                    animator.SetBool("PunchSkill", false);
            switch (skill)
            {
                case (TitanSkill.CenterSphere):
                    ShotSkill = sphereSkill;
                    targetPosition = new Vector3(3.5f, 0, -12.0f);
                    break;
                case (TitanSkill.Punch):
                    ShotSkill = punchSkill;
                    targetPosition = new Vector3(0, 0, 15);
                    animator.SetBool("PunchSkill",true);
                    break;
                case (TitanSkill.AllRange):
                    ShotSkill = allRange;
                    targetPosition = new Vector3(0, 0, 0);
                    break;
                case (TitanSkill.Line):
                    ShotSkill = line;
                    targetPosition = new Vector3(0, 0, 0);
                    break;
                    //default:
                    //    ShotSkill = sphereSkill;
                    //    break;
            }

            skillPlaceComponent.SkillPlace(ShotSkill, targetPosition, "SkillChaging");
        }
    }

    void Dead()
    {
        deadComponent.Dead(hitPoint, "KnockDown");
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
