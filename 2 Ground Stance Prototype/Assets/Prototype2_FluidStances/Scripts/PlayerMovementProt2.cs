using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementProt2 : MonoBehaviour
{
    VirtuellController input;
    BeatAnalyse beatBox;
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
    private bool beat;
    private InputPackage inputPackage;
    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    //Stances
    int stance = 0;
    public enum Stances { Neutral, Jump, Slide, Attack, Parry}
    public static Stances currentStance = Stances.Neutral;

    public enum JumpStances { BaseJump, UpJump, AirJuggle }
    public enum SlideStances { Slide }
    public enum AttackStances { Attack }
    public enum ParryStances { Parry }
    //Stances nextStance = Stances.Aggro;
    //public static int stanceChargeLevel = 0;

    private bool cameraButton = false;

    private bool animationLocked = false;

    [SerializeField] GameObject enemy;



    void Start()
    {
        input = GetComponent<VirtuellController>();
        beatBox = GameObject.FindWithTag("MusicBox").GetComponent<BeatAnalyse>();
        anim = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        inputPackage = input.InputPackage;
        //if(!beatBox.IsOnBeat(100))
        //{
        //    beat = false;
        //}

        //#region TriggerResets
        //
        //anim.ResetTrigger("jumping");
        //anim.ResetTrigger("attacking");
        //anim.ResetTrigger("evasion");
        //anim.ResetTrigger("evasionRight");
        //anim.ResetTrigger("evasionLeft");
        //
        //#endregion


        if (!animationLocked)
        {
            

            if (inputPackage.InputA)
            {
                //if (beatBox.IsOnBeat(100) && stanceChargeLevel < 4 && !beat)
                //{
                //    stanceChargeLevel++;
                //    Debug.Log("StanceChargeLevel: " + stanceChargeLevel);
                //    beat = true;
                //}
                Jump();
            }
            else if (inputPackage.InputY)
            {
                Parry();
            }
            //Attack different button
            else if (inputPackage.InputX)
            {
                if (beatBox.IsOnBeat(100) && stanceChargeLevel < 4 && !beat)
                {
                    stanceChargeLevel++;
                    Debug.Log("StanceChargeLevel: " + stanceChargeLevel);
                    beat = true;
                }
                Attack();
                StartCoroutine(animationLock());
            }
            else if (inputPackage.InputB)
            {
                Slide();
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
        }
        else
        {
            transform.LookAt(enemy.transform);
        }
        controller.Move(desiredMoveDirection * Time.deltaTime * speed);
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
            stanceChargeLevel = 0;
            switch (requestedStance)
            {
                case Stances.Agility:
                    blockRotationPlayer = false;
                    speed = speed * 2;
                    anim.SetInteger("Stance", 0);
                    nextStance = Stances.Aggro;
                    evasionSpeed = 15;
                    break;
                case Stances.Aggro:
                    blockRotationPlayer = true;
                    speed = speed * 0.5f;
                    anim.SetInteger("Stance", 1);
                    nextStance = Stances.Agility;
                    evasionSpeed = 1;
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




    IEnumerator animationLock()
    {
        animationLocked = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        animationLocked = false;
        evasion = false;
    }
}
