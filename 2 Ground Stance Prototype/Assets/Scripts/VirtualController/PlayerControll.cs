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
    private StanceState nextSate;

    private InputPackage inputPackage;
    private Rigidbody rigi;

    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    [SerializeField] private float movementSpeed = 0;

    private Vector3 moveVector;

    private bool cameraButton = false;

    private void Start()
    {        
        rigi = GetComponent<Rigidbody>();
        ChangeState(currentState);
    }

    private void Update()
    {       
        if (inputPackage != null)
        {
            MovementCalculation();
        }

        //This is for changing the Stances
        if (inputPackage.CameraButton)
        {
            if (!cameraButton)
            {
                cameraButton = true;
                if (currentState == StanceState.AgilityStance)
                {
                    nextSate = StanceState.AggroStance;
                }
                else
                {
                    nextSate = StanceState.AgilityStance;
                }
            }
        }
        else if(cameraButton)
        {
            cameraButton = false;
        }
    }

    private void FixedUpdate()
    {
        if (inputPackage != null)
        {
            Move();
        }

        if(currentState != nextSate)
        {
            ChangeState(nextSate);
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

        moveVector = moveVector.normalized * move * movementSpeed;
    }

    private void Move()
    {
        if (inputPackage.MoveHorizontal >= 0.1f ||
            inputPackage.MoveHorizontal <= -0.1f ||
            inputPackage.MoveVertical >= 0.1f ||
            inputPackage.MoveVertical <= -0.1f)
        {
            rigi.velocity = new Vector3(moveVector.x,
                                    0f,
                                    moveVector.z);
        }
        else
        {
            rigi.velocity = new Vector3(0f,
                                        0f,
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
            movementSpeed = AgilityStance.movementSpeed;
        }
        else
        {
            movementSpeed = AggroStance.movementSpeed;
        }
        currentState = changeState;
    }
}
