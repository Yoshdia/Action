using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDangerZone : DangerZoneBasic
{
    [SerializeField]
   GameObject fallEffect;
    [SerializeField]
    GameObject explosionEffect;

    bool effectSet;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        effectSet = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();   
    }

    protected override void DeleatingZone()
    {
        if (waitingInterval < 0)
        {
            alfa -= alfaSpeed;
            if (alfa < 0)
            {
                deletedDangerZone = true;
                if(!effectSet)
                {
                    effectSet = true;
                    Vector3 fall = transform.position;
                    Quaternion fallQ = new Quaternion();
                    Instantiate(explosionEffect,fall,fallQ);
                    fall.y += 20;
                    fallQ = Quaternion.Euler(90, 0, 0);
                    Instantiate(fallEffect,fall,fallQ);
                }
                return;
            }
            GetComponent<Renderer>().material.color = new Color(255, 0, 0, alfa);
        }
        else
        {
            waitingInterval--;
        }
    }
}
