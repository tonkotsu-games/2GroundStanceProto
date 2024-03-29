﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    private enum StanceState
    {
        AgilityStance,
        AggroStance
    }

    private StanceState currentState;
    private StanceState nextState;

    private InputPackage inputPackage;
    private Rigidbody rigi;

    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    [SerializeField] private float movementSpeed = 0;
    [SerializeField] private float jumpheight = 10;
    [SerializeField] private float rotationSpeed = 0.1f;
    float evasionDistance;
    [SerializeField] float evasionSpeed = 0.1f;
    Vector3 positionAfterEvade;
    Animator playerAnim;

    Vector3 evasionHeading;
    bool evasion = false;

    private Vector3 moveVector;

    private bool cameraButton = false;

    VirtuellController input;

    [SerializeField] GameObject mainCam;

    private void Start()
    {
        playerAnim = gameObject.GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        ChangeState(currentState);
        input = GetComponent<VirtuellController>();
    }

    private void Update()
    {
        inputPackage = input.InputPackage;
        if (inputPackage != null)
        {
            MovementCalculation();
        }

        if (inputPackage != null)
        {
            //This is for changing the Stances
            if (inputPackage.CameraButton)
            {
                if (!cameraButton)
                {
                    if (currentState == StanceState.AgilityStance)
                    {
                        nextState = StanceState.AggroStance;
                    }
                    else
                    {
                        nextState = StanceState.AgilityStance;
                    }
                    cameraButton = true;
                }
                else
                {
                    nextState = StanceState.AgilityStance;
                }
                cameraButton = true;
                mainCam.GetComponent<CameraFollow>().ChangeState();
            }
            else if (cameraButton)
            {
                cameraButton = false;
            }
        }
    }


    private void FixedUpdate()
    {
        if (inputPackage != null)
        {
            Move();

        }

        if (currentState != nextState)
        {
            ChangeState(nextState);
        }
    }

    private void MovementCalculation()
    {
        float move = new Vector2(inputPackage.MoveHorizontal, inputPackage.MoveVertical).magnitude;
        if(move > 1)
        {
            move = 1;
        }
        moveVector = new Vector3(inputPackage.MoveHorizontal, 
                                 0f, 
                                 inputPackage.MoveVertical);

        moveVector =moveVector.normalized * move * movementSpeed;
    }

    private void Move()
    {
        if (inputPackage.MoveHorizontal >= 0.1f ||
            inputPackage.MoveHorizontal <= -0.1f ||
            inputPackage.MoveVertical >= 0.1f ||
            inputPackage.MoveVertical <= -0.1f)
        {
            Vector3 direction = Camera.main.transform.forward;
            direction.y = 0f;
            direction.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10);
            moveVector = Camera.main.transform.TransformDirection(moveVector);
            rigi.velocity = new Vector3(moveVector.x,
                                    0f,
                                    moveVector.z);
        }
        else
        {
            rigi.velocity = new Vector3(0f,
                                        rigi.velocity.y,
                                        0f);
        }
    }
    /// <summary>
    /// Here the state would be changed and the player get all the informations from a seperated class
    /// </summary>
    /// <param name="changeState"></param>
    private void ChangeState(StanceState changeState)
    {
        if(changeState == StanceState.AgilityStance)
        {
            playerAnim.SetInteger("Stance", 0);
            movementSpeed = AgilityStance.movementSpeed;
            evasionDistance = AgilityStance.evasionDistance;
        }
        else
        {
            playerAnim.SetInteger("Stance", 1);
            movementSpeed = AggroStance.movementSpeed;
            evasionDistance = AggroStance.evasionDistance;
        }
        currentState = changeState;
    }
}
