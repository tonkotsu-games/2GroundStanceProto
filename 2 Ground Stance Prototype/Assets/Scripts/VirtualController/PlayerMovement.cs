using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float InputX;
    public float InputZ;

    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;

    public float jumpHeight;
    public float desiredRotationSpeed;
    public Animator anim;
    public float speed;
    private float Speed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController controller;
    public bool isGrounded;
    public float verticalVel;
    private Vector3 moveVector;
    private Vector3 evasionHeading;
    private Vector3 positionAfterEvade;
    public float evasionSpeed;
    private bool evasion;

    private InputPackage inputPackage;
    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    int stance = 0;
    public enum Stances { Agility,Aggro}
    Stances currentStance = Stances.Agility;
    Stances nextStance = Stances.Aggro;

    private bool cameraButton = false;

    private bool animationLocked = false;


    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        #region TriggerResets

        anim.ResetTrigger("jumping");
        anim.ResetTrigger("attacking");
        anim.ResetTrigger("evasion");

        #endregion
        if (evasion)
        {
            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();
            desiredMoveDirection = forward * InputZ + right * InputX;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * evasionSpeed);
            StartCoroutine(animationLock());
        }
            
        
        if (!animationLocked)
        {
            if (inputPackage.CameraButton && !cameraButton)
            {

                ChangePlayerStance(nextStance);
                cameraButton = true;
            }

            if (inputPackage.InputA)
            {
                Jump();
            }

            if (inputPackage.TriggerRight != 0)
            {
                Attack();
                StartCoroutine(animationLock());
            }

            if (inputPackage.InputB)
            {
                Evade();
                StartCoroutine(animationLock());
            }

            InputMagnitude();

            isGrounded = controller.isGrounded;

            if (isGrounded)
            {
                verticalVel = 0;
            }
            else
            {
                verticalVel -= 2;
            }

            moveVector = new Vector3(0, verticalVel, 0);
            controller.Move(moveVector);
        }
    }

    private void FixedUpdate()
    {
        cameraButton = false;
    }

    void PlayerMoveAndRotation()
    {

        //InputX = Input.GetAxisRaw("Horizontal");
        //InputZ = Input.GetAxisRaw("Vertical");

        InputX = inputPackage.MoveHorizontal;
        InputZ = inputPackage.MoveVertical;

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if(blockRotationPlayer == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * speed);
        }
    }

    void InputMagnitude()
    {
       // InputX = Input.GetAxisRaw("Horizontal");
       // InputZ = Input.GetAxisRaw("Vertical");
        InputX = inputPackage.MoveHorizontal;
        InputZ = inputPackage.MoveVertical;

        anim.SetFloat("InputX", InputX, 0f, Time.deltaTime * 2f);
        anim.SetFloat("InputZ", InputZ, 0f, Time.deltaTime * 2f);

        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if (Speed > allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", Speed, 0f, Time.deltaTime);
            PlayerMoveAndRotation();
        }
        else if ( Speed < allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", Speed, 0f, Time.deltaTime);
        }
    }

    public void ChangePlayerStance(Stances requestedStance)
    {
        if (requestedStance != currentStance)
        {

            currentStance = requestedStance;

            switch (requestedStance)
            {
                case Stances.Agility:
                    speed = speed * 2;
                    anim.SetInteger("Stance", 0);
                    nextStance = Stances.Aggro;
                    break;
                case Stances.Aggro:
                    speed = speed * 0.5f;
                    anim.SetInteger("Stance", 1);
                    nextStance = Stances.Agility;
                    break;
            }
        }
    }

    private void Jump()
    {
        switch (currentStance)
        {
            case Stances.Agility:

                controller.Move(new Vector3(0, jumpHeight, 0));
                anim.SetTrigger("jumping");
               
                break;
            case Stances.Aggro:
                break;
        }
    }

    void Attack()
    {
        anim.SetTrigger("attacking");
    }

    void Evade()
    {
        if (currentStance == Stances.Aggro)
        {
        }
        else
        {
            anim.SetTrigger("evasion");
        }
        evasion = true;
    }

    IEnumerator animationLock()
    {
        animationLocked = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        animationLocked = false;
        evasion = false;
    }
}
