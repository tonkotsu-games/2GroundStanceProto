using System.Collections;
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
    Animator playerAnim;

    private Vector3 moveVector;

    private bool cameraButton = false;

    private void Start()
    {
        playerAnim = gameObject.GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        ChangeState(currentState);
    }

    private void Update()
    {
        playerAnim.ResetTrigger("jumping");
        playerAnim.ResetTrigger("attacking");
        if (inputPackage != null)
        {
            MovementCalculation();
            playerAnim.SetFloat("runningVer", inputPackage.MoveVertical);
            playerAnim.SetFloat("runningHor", inputPackage.MoveHorizontal);

            
        }

        if (inputPackage.InputA)
        {
            Jump();

        }
        if (inputPackage.InputB) {
            Attack();
        }
        //This is for changing the Stances
        if (inputPackage.CameraButton)
        {
            if (!cameraButton)
            {
                nextState = StanceState.AggroStance;
            }
            else
            {
                nextState = StanceState.AgilityStance;
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

        if(currentState != nextState)
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
                                    rigi.velocity.y,
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
        }
        else
        {
            playerAnim.SetInteger("Stance", 1);
            movementSpeed = AggroStance.movementSpeed;
        }
        currentState = changeState;
    }

    private void Jump()
    {
        switch (currentState)
        {
            case StanceState.AgilityStance:
                playerAnim.SetTrigger("jumping");
                rigi.AddForce(new Vector3(0, jumpheight, 0));
                break;
            case StanceState.AggroStance:
                break;
        }
    }
    private void Attack()
    {
        switch (currentState)
        {
            case StanceState.AgilityStance:
                break;
            case StanceState.AggroStance:
                break;
        }

        playerAnim.SetTrigger("attacking");
    }

}
