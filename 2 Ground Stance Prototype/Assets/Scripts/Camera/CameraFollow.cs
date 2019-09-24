using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private InputPackage inputPackage;
    
    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    [SerializeField] private GameObject playerTarget;
    [SerializeField] private GameObject enemyTarget;
    private GameObject lookAt;

    [Range(0,20)]
    [SerializeField] private float offSetY;
    [Range(-20,0)]
    [SerializeField] private float offSetZ;
    [Range(0,20)]
    [SerializeField] private float highSet;

    Quaternion rotationX;
    Quaternion rotationY;

    Vector3 offSet;
    Vector3 direction;
    Vector3 offSetNew;
    Vector3 offSetOld;


    private void Start()
    {
        offSet = new Vector3(0, offSetY, offSetZ);
        offSetNew = offSet;
        offSetOld = offSet;
        lookAt = playerTarget;
    }

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
        offSetNew = new Vector3(0, offSetY, offSetZ);

        if (offSetOld != offSetNew)
        {
            offSet = offSetNew;
            offSetOld = offSetNew;
        }
    }
    public void CameraMove()
    {
        if (lookAt == playerTarget)
        {
            rotationX = Quaternion.AngleAxis(inputPackage.CameraHorizontal, Vector3.up);
            rotationY = Quaternion.AngleAxis(inputPackage.CameraVertical, Vector3.right);
            offSet = rotationY * rotationX * offSet;
            transform.position = playerTarget.transform.position + offSet;
        }
        else
        {
            transform.position = (playerTarget.transform.position + offSet) - enemyTarget.transform.position;
        }
        transform.LookAt(lookAt.transform);
    }
    public void ChangeState()
    {
        if(lookAt == playerTarget)
        {
            lookAt = enemyTarget;
        }
        else
        {
            lookAt = playerTarget;
        }
    }
}
