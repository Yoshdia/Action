using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemy : MonoBehaviour
{
    Animator animator;

    float destroyCount = 300;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        destroyCount = 300;
    }

    public void Dead(float hitPoint,System.String deadAnimation)
    {
        if (hitPoint > 0)
        {
            return;
        }
        animator.SetBool(deadAnimation, true);
        destroyCount--;
        if (destroyCount < 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
