using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    MoveByRigidbody moveComponent;
    int lifeCount;
    [SerializeField]
    int lifeCountMax=200;

    private void Awake()
    {
        moveComponent = GetComponent<MoveByRigidbody>();
    }

    private void Start()
    {
        lifeCount = lifeCountMax;
    }
    private void FixedUpdate()
    {
        moveComponent.Move(transform.forward);
        if(lifeCount--<0)
        {
            Destroy(transform.gameObject);
        }
    }
}
