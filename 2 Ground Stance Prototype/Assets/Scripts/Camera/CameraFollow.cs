using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private InputPackage inputPackage;
    
    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    [SerializeField] private GameObject target;

    [SerializeField] private float offSetY, offSetZ;

    Quaternion rotationX;
    Quaternion rotationY;

    Vector3 offSet;
    Vector3 direction;

    void Update()
    {
        if (inputPackage != null)
        {
            CameraMove();
        }
        direction = transform.forward;
    }
    private void FixedUpdate()
    {
        offSet = new Vector3(0, offSetY, offSetZ);

    }
    public void CameraMove()
    {
        rotationX = Quaternion.AngleAxis(inputPackage.CameraHorizontal, Vector3.up);
        rotationY = Quaternion.AngleAxis(inputPackage.CameraVertical, Vector3.right);
        offSet = rotationY * rotationX * offSet;
        transform.position = target.transform.position + offSet;
        if(transform.localEulerAngles.x >= 70)
        {
            transform.Rotate(70, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if(transform.localEulerAngles.x <= 10)
        {
            transform.Rotate(10, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        transform.LookAt(target.transform);
    }
}
