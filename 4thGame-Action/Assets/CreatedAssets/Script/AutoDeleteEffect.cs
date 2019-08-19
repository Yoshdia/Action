using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeleteEffect : MonoBehaviour
{
    void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.main.duration+3);
        
    }
}
