using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillPlace : MonoBehaviour
{
    Animator animator;

    /// <summary>
    /// 動的に変化する、予兆発生からモーション再生までのインターバル
    /// </summary>
    float skillDangerTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void SkillPlace(float skillDangerTimeMax,EnemySkillBasic skill,Transform targetTransform,System.String animationName)
    {
        skillDangerTime = skillDangerTimeMax;
        Transform target = targetTransform;
        skill.SetSkillPrehub(targetTransform);
        animator.SetBool(animationName, true);
    }

    public bool DangerSkillAttacked(System.String endSkillAnimation,System.String skillAttackAnimation)
    {
        if (skillDangerTime < 0)
        {
            animator.SetBool(endSkillAnimation, false);
            animator.SetBool(skillAttackAnimation, true);
            return true;
        }
        else
        {
            skillDangerTime--;
            return false;
        }
    }

    public void StopSkillAttackAnimation(System.String skillAttackAnimation)
    {
        animator.SetBool(skillAttackAnimation, false);
    }

}
