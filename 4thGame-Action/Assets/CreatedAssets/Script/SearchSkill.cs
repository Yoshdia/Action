using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchSkill : DangerZoneBasic
{
    [SerializeField]
    GameObject bombEffect;
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
                if (!effectSet)
                {
                    effectSet = true;

                    Quaternion qua = new Quaternion();
                    qua = Quaternion.Euler(90, 0, 0);
                    Instantiate(bombEffect,transform.position,qua);
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
