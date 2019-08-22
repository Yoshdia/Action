using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    int Interval;
    [SerializeField]
    int IntervalMax = 100;

    [SerializeField]
    GameObject skillObject;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Shot(System.String fireAnimation,Transform transform)
    {
        animator.SetBool(fireAnimation, true);
        GameObject skill = Instantiate(skillObject, transform.position, transform.rotation);
        Interval = IntervalMax;
    }

    public bool CountInterval()
    {
        if (Interval > 0)
        {
            Interval--;
            return true;
        }
        else
        {
            return false;

        }
    }

    public void StopAnimation(System.String fireAnimation)
    {
        animator.SetBool(fireAnimation, false);
    }
}
