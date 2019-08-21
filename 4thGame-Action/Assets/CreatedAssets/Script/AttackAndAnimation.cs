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
    /// オートアタック実行関数
    /// </summary>
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

    public void AttackMotionEnd(System.String animationName)
    {
        animator.SetBool(animationName, false);
    }
}
