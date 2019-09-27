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

    public float gravity;
    public float baseJumpHeight;
    public float airJumpHeight;
    public float airJumpDistance;
    public float jumpDistance;
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
    private Vector3 jumpVector;
    public float evasionSpeed;
    private bool evasion;
    private bool beat;
    private bool jumping;
    [SerializeField]
    private bool inputDone;
    private InputPackage inputPackage;
    public InputPackage InputPackage { get => inputPackage; set => inputPackage = value; }

    #region buttonDown Bools
    private bool inputAReady;
    private bool inputBReady;
    private bool inputYReady;
    private bool inputXReady;
    #endregion

    [SerializeField]
    AnimationClip[] animationClips;


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

    [SerializeField]
    private Collider col;

    [SerializeField]
    private GameObject enemy;

    private void Awake()
    {
        input = GetComponent<VirtuellController>();
        beatBox = GameObject.FindWithTag("MusicBox").GetComponent<BeatAnalyse>();
        anim = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        cam = Camera.main;
    }
    void Start()
    {
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.ResetTrigger("jumping");
        anim.ResetTrigger("sliding");
        anim.ResetTrigger("parrying");
        anim.ResetTrigger("attacking");

        isGrounded = controller.isGrounded;

        inputPackage = input.InputPackage;
        if (inputPackage == null)
        {
            return;
        }

        //Did you become grounded?

        if (Input.GetButtonDown("A"))
        {
           //if (inputAReady)
           //{
                if (!animationLocked)
                {
                    HandleJump();
                    inputAReady = false;
                }
            //}
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

            if (isGrounded)
            {
                if(currentStance != Stances.Neutral) { 
                currentStance = Stances.Neutral;
                    }
            }

            verticalVel = -gravity;

            moveVector = new Vector3(0, verticalVel, 0);
            controller.Move(moveVector);
            if (currentStance == Stances.Neutral)
            {
                InputMagnitude();
            }
            //check if we should be in neutral --> grounded, anim neutral --> Stance Neutral
            //--> execute neutral --> Walk Run and so on
        }
        

        // if (isGrounded)
        //     {
        //         verticalVel = 0;
        //     }
        //     else
        //     {
        //         verticalVel -= 2;
        //     }
        //
        //     moveVector = new Vector3(0, verticalVel, 0);
        //     controller.Move(moveVector);
    }


    private void FixedUpdate()
    {

        if (jumping)
        {
            if (animationLocked)
            {
                controller.Move(jumpVector);
            }
            else
            {
                jumping = false;
            }
        }
    }

    private void PlayerMoveAndRotation()
    {
        //InputX = Input.GetAxisRaw("Horizontal");
        //InputZ = Input.GetAxisRaw("Vertical");

        CalculateMovementDirection();

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

    private void CalculateMovementDirection()
    {
        InputX = inputPackage.MoveHorizontal;
        InputZ = inputPackage.MoveVertical;

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputZ + right * InputX;
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
                AirJumpBehaviour();
            }
        }
        else if(currentStance == Stances.Jump)
        {
            AirJumpBehaviour();
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
        anim.SetTrigger("parrying");
    }

    //Attack Behaviour
    private void AttackBehaviour()
    {
        anim.SetTrigger("attacking");
    }

    //Slide Behaviour
    private void SlideBehaviour()
    {
        anim.SetTrigger("sliding");
    }

    //Jump Behaviour
    private void BaseJumpBehaviour()
    {
        anim.SetTrigger("jumping");

        //controller.Move(new Vector3(0, baseJumpHeight, 0))
        CalculateMovementDirection();
        transform.rotation = Quaternion.LookRotation(desiredMoveDirection);
        jumpVector = desiredMoveDirection;
        jumpVector = jumpVector * jumpDistance;
        jumpVector.y = baseJumpHeight;
        Debug.Log("JumpVector: " + jumpVector);
        EnableAnimationLock(0,0.0f);
        jumping = true;
    }

    private void AirJumpBehaviour()
    {


        anim.SetTrigger("jumping");
        //get controller direction in here
        CalculateMovementDirection();
        transform.rotation = Quaternion.LookRotation(desiredMoveDirection);
        jumpVector = desiredMoveDirection;
        jumpVector = jumpVector * airJumpDistance;
        jumpVector.y = airJumpHeight;
        EnableAnimationLock(0,0.1f);
        jumping = true;
    }




    IEnumerator animationLock(float clipLength , float window)
    {
        animationLocked = true;
        yield return new WaitForSeconds(clipLength - window);
        animationLocked = false;
    }


    public void ColliderEnabled()
    {
        col.enabled = true;
    }
    public void ColliderDisabled()
    {
        col.enabled = false;
    }

    public void EnableAnimationLock(int clipIndex ,float window)
    {
        StartCoroutine(animationLock(animationClips[clipIndex].length, window));
    }

    // called from Animation Events!
    public void DisableAnimationLock()
    {
        Debug.Log("unlock Animation");
        animationLocked = false;
    }


    private void OnGUI()
    {
        GUILayout.Toggle(animationLocked,"AnimationLocked: ");
        GUILayout.Toggle(jumping, "jumping");
    }
}