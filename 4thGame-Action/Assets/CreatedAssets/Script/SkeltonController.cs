using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonController : EnemyBasic
{
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

    private EnemySkillBasic sphereSkill;
    private EnemySkillBasic searchSkill;

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
    /// 動的に変化する、予兆発生からモーション再生までのインターバル
    /// </summary>
    float skillDangerTime;

    /// <summary>
    /// スキルプレハブを設置したことを表すフラグ
    /// </summary>
    bool skillPlaced;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        instantSkillEffect = null;
        myState = SkeltonState.SkillChaging;
        skill = SkeltonSkill.Search;
        sphereSkill = GetComponent<SphereSkill>();
        searchSkill = GetComponent<SearchDanger>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (myState == SkeltonState.AutoAttacking)
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
        Vector3 targetPosition = target.GetTransform().position;
        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance > movingDistance)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), 0.3f);
            Vector3 targetForword = transform.forward;
            targetForword.y = 0;
            transform.position += targetForword * moveSpeed;
            animator.SetBool("Walk", true);
        }
        else
        {
            moveSpeed = 0;
            animator.SetBool("Walk", false);
            Attack();
        }
    }

    void SkillPlace()
    {
        if (skillPlaced)
        {
            if (skillDangerTime < 0)
            {
                animator.SetBool("Skill", true);
                animator.SetBool("SkillChaging", false);
                myState = SkeltonState.AutoAttacking;
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
            animator.SetBool("SkillChaging", true);
            skillPlaced = true;
            skillDangerTime = SphereSkill.dangerTime;
            Quaternion effectQua = new Quaternion();
            effectQua = Quaternion.Euler(new Vector3(-90, 0, 0));
            instantSkillEffect = Instantiate(sphereSkillEffect, transform.position, effectQua);
            instantSkillEffect.transform.parent = transform;

            EnemySkillBasic ShotSkill = null;
            Transform trans = transform;
            switch (skill)
            {
                case (SkeltonSkill.SphereExplosion):
                    ShotSkill = sphereSkill;
                    break;
                case (SkeltonSkill.Search):
                    ShotSkill = searchSkill;
                    trans = target.GetTransform();
                    break;
                default:
                    ShotSkill = sphereSkill;
                    break;
            }

            ShotSkill.SetSkillPrehub(trans);
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        animator.SetBool("Skill", false);
    }
}
