using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndAnimation : MonoBehaviour
{
     Animator animator;

    /// <summary>
    /// オートアタックの間隔、ダメージ
    /// </summary>
    protected float attackInterval;
    [SerializeField]
    protected float attackIntervalMax = 200;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackInterval = attackIntervalMax;
    }

    private void FixedUpdate()
    {
        attackInterval--;
    }

    /// <summary>
    /// 通常攻撃実行関数 呼び方はAdd先でそれぞれ決定させる
    /// </summary>
    /// <param name="animationName">攻撃時に再生するアニメーションの名前</param>
    /// <returns></returns>
    public bool Attack(System.String animationName)
    {
        if (attackInterval > 0)
        {
            return false;
        }
        animator.SetBool(animationName, true);
        attackInterval = attackIntervalMax;
        return true;
    }

    /// <summary>
    /// attackIntervalのカウントを無視し攻撃させattackIntervalもリセットさせる
    /// </summary>
    /// <param name="animationName">攻撃時に再生するアニメーションの名前</param>
    public void AttackAndIntervalReset(System.String animationName)
    {
        animator.SetBool(animationName, true);
        attackInterval = attackIntervalMax;
    }

    /// <summary>
    /// 停止させるアニメーションの名前 Attackにて停止させたもののと同じものを呼ぶように
    /// </summary>
    /// <param name="animationName">停止させるアニメーションの名前</param>
    public void AttackMotionEnd(System.String animationName)
    {
        animator.SetBool(animationName, false);
    }
}
