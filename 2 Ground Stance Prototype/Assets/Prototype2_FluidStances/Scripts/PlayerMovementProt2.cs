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
    public enum Stances { Neutral, Jump, Slide, Attack, Parry }
    private Stances currentStance = Stances.Neutral;

    public enum JumpStances { BaseJump, AirJuggle }
    private JumpStances currentJumpStance = JumpStances.BaseJump;

    public enum SlideStances { Slide }
    private SlideStances currentSlideStance = SlideStances.Slide;

    public enum AttackStances { Attack }
    private AttackStances currentAttackStance = AttackStances.Attack;

    public enum ParryStances { GroundParry, AirParry }
    private ParryStances currentParryStance = ParryStances.GroundParry;

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


        //Did you become grounded?

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
                HandleJump();
            }
            else if (inputPackage.InputY)
            {
                HandleParry();
            }
            else if (inputPackage.InputX)
            {
                HandleAttack();
            }
            else if (inputPackage.InputB)
            {
                HandleSlide();
            }
            else
            {
                //check if we should be in neutral --> grounded, anim neutral --> Stance Neutral
                //--> execute neutral --> Walk Run and so on
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

        if (blockRotationPlayer == false)
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
        else if (Speed < allowPlayerRotation)
        {
            anim.SetFloat("InputMagnitude", Speed, 0f, Time.deltaTime);
        }
    }


    //Refers to Behaviour based on previous stance
    private void HandleParry()
    {
        ParryBehaviour();
        currentStance = Stances.Parry;
    }
    private void HandleAttack()
    {
        AttackBehaviour();
        currentStance = Stances.Attack;
    }
    private void HandleSlide()
    {
        SlideBehaviour();
        currentStance = Stances.Slide;
    }

    private void HandleJump()
    {
        if(currentStance == Stances.Neutral)
        {
            BaseJumpBehaviour();
        }
        else if(currentStance == Stances.Parry)
        {
            if (currentParryStance == ParryStances.GroundParry)
            {
                BaseJumpBehaviour();
            }
            else
            {
                //AirJuggle
            }
        }
        else if(currentStance == Stances.Jump)
        {
            //Airjuggle
        }
        else if (currentStance == Stances.Attack)
        {
            BaseJumpBehaviour();
        }
        else if (currentStance == Stances.Slide)
        {
            BaseJumpBehaviour();
        }

        currentStance = Stances.Jump;
    }


    //Parry Behaviour
    private void ParryBehaviour()
    {

        
    }

    //Attack Behaviour
    private void AttackBehaviour()
    {
        anim.SetTrigger("attacking");
    }

    //Slide Behaviour
    private void SlideBehaviour()
    {
       
    }

    //Jump Behaviour
    private void BaseJumpBehaviour()
    {
        controller.Move(new Vector3(0, jumpHeight, 0));
        anim.SetTrigger("jumping");
    }

    private void AirJumpBehaviour()
    {
        //get controller direction in here
        controller.Move(new Vector3(0, jumpHeight, 0));
        anim.SetTrigger("jumping");
    }




    IEnumerator animationLock()
    {
        animationLocked = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        animationLocked = false;
        evasion = false;
    }
}
