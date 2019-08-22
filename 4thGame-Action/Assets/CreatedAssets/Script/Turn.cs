using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public void LookRotate(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void LookAt(Transform transform)
    {
        transform.LookAt(transform);
    }
}
