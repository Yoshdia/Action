using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNearObject : MonoBehaviour
{
    

    public GameObject GetNearTarget(System.String searchTag)
    {
        float tmpDistance = 100;
        GameObject target=null;

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag(searchTag))
        {
            float dis = Vector3.Distance(transform.position, obj.transform.position);

            if(Mathf.Abs(dis)<=tmpDistance)
            {
                tmpDistance = dis;
                target = obj;
            }
        }

        return target;
    }

}
