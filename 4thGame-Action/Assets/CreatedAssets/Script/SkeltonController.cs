using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeltonController : MonoBehaviour
{
    Animator animator;

    enum SkelltonState
    {
        AutoAttacking,
        Chaging,
    }
    SkelltonState myState;

    /// <summary>
    /// 移動を停止する距離
    /// </summary>
    [SerializeField]
    float movingDistance=0.5f;

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
    const float attackIntervalMax=100;

    [SerializeField]
    const float damage=1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myState = SkelltonState.AutoAttacking;
        moveSpeed = 0;
        attackInterval = attackIntervalMax;
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed += 0.002f;
        if(moveSpeed>moveSpeedMax)
        {
            moveSpeed = moveSpeedMax;
        }
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

        }
    }

    private void Move()
    {
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

    public void Attack()
    {
        if(attackInterval<0)
        {
            attackInterval = attackIntervalMax;
            animator.SetBool("Attack",true);
            target.AddDamage(damage);
            Debug.Log("attacked!");
        }
    }

    private void LateUpdate()
    {
            animator.SetBool("Attack",false);
    }
}
