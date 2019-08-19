using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    protected Animator animator;

    /// <summary>
    /// 移動や攻撃の対象になるオブジェクト
    /// </summary>
    [SerializeField]
    protected MoveController target;

    /// <summary>
    /// オートアタックの間隔、ダメージ
    /// </summary>
    protected　float attackInterval;
    [SerializeField]
    protected　float attackIntervalMax = 200;
    [SerializeField]
    protected　float autoAttackDamage = 1;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        attackInterval = attackIntervalMax;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        attackInterval--;
    }

    /// <summary>
    /// オートアタック実行関数
    /// </summary>
    protected void Attack()
    {
        //インターバルがまだカウント中の場合すぐに返す
        if (attackInterval > 0)
        {
            return;
        }
            animator.SetBool("Attack", true);
            target.AddDamage(autoAttackDamage);
            attackInterval = attackIntervalMax;
            Debug.Log("attacked!");
    }

    protected virtual void LateUpdate()
    {
        animator.SetBool("Attack", false);
    }
}
