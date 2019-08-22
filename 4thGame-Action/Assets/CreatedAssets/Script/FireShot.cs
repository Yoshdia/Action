using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShot : MonoBehaviour
{
    int fireInterval;
    [SerializeField]
    int fireIntervalMax = 100;

    [SerializeField]
    GameObject fireObject;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Fire(System.String fireAnimation, Vector3 direction)
    {
        animator.SetBool(fireAnimation, true);
        fireObject = Instantiate(fireObject, transform.position, transform.rotation);
        fireInterval = fireIntervalMax;
    }

    public bool CountInterval()
    {
        if (fireInterval > 0)
        {
            fireInterval--;
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
