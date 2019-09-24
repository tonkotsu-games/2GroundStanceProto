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


    private void Start()
    {
        offSet =new Vector3(target.transform.position.x, target.transform.position.y + offSetY, target.transform.position.z + offSetZ) - target.transform.position;
    }

    void Update()
    {
        if (inputPackage != null)
        {
            CameraMove();
        }
        direction = transform.forward;
    }

    public void CameraMove()
    {
        rotationX = Quaternion.AngleAxis(inputPackage.CameraHorizontal, Vector3.up);
        rotationY = Quaternion.AngleAxis(inputPackage.CameraVertical, Vector3.right);
        offSet = rotationY * rotationX * offSet;
        transform.position = target.transform.position + offSet;
        if(transform.eulerAngles.x >= 90)
        {
            transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if(transform.eulerAngles.x <= 0)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        transform.LookAt(target.transform);
    }
}
