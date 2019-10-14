using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public static bool isGrounded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rail>() != null)
        {
            other.GetComponent<Rail>().StartGrinding(this.transform.parent.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
        if(other.gameObject.GetComponent<Rail>() != null)
        {
            other.GetComponent<Rail>().StopGrinding();
        }
    }

    private void OnGUI()
    {
        GUILayout.Toggle(isGrounded, "Is Grounded");
    }
}
