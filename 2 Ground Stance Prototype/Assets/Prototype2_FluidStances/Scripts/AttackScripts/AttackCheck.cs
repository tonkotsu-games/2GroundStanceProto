using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private bool hit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss" && !hit)
        {
            Debug.Log("Hit");
            other.GetComponent<LifeCounter>().LifeChange();
            hit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hit = false;
    }

}
