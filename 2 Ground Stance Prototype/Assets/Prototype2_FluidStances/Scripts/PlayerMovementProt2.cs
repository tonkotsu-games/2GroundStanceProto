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

    int stance = 0;
    public enum Stances { Agility,Aggro}
    public static Stances currentStance = Stances.Agility;
    Stances nextStance = Stances.Aggro;
    public static int stanceChargeLevel = 0;

    private bool cameraButton = false;

    private bool animationLocked = false;

    [SerializeField]
    private Collider col;

    [SerializeField]
    private GameObject enemy;

    void Start()
    {
        input = GetComponent<VirtuellController>();
        beatBox = GameObject.FindWithTag("MusicBox").GetComponent<BeatAnalyse>();
        anim = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        cam = Camera.main;
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        inputPackage = input.InputPackage;
        if(!beatBox.IsOnBeat(100))
        {
            beat = false;
        }

        #region TriggerResets

        anim.ResetTrigger("jumping");
        anim.ResetTrigger("attacking");
        anim.ResetTrigger("evasion");
        anim.ResetTrigger("evasionRight");
        anim.ResetTrigger("evasionLeft");

        #endregion
        if (evasion)
        {

            
            if (!blockRotationPlayer)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
            }
            controller.Move(desiredMoveDirection * Time.deltaTime * evasionSpeed);
            StartCoroutine(animationLock());
            
        }
        if (inputPackage != null)
        {
            if (!animationLocked)
            {
                if (inputPackage.CameraButton)
                {
                    ChangePlayerStance(nextStance);
                }

                if (inputPackage.InputA)
                {
                    if (beatBox.IsOnBeat(100) && stanceChargeLevel < 4 && !beat)
                    {
                        stanceChargeLevel++;
                        Debug.Log("StanceChargeLevel: " + stanceChargeLevel);
                        beat = true;
                    }
                    Jump();
                }

                if (inputPackage.TriggerRight != 0)
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

                if (inputPackage.InputB)
                {
                    if (beatBox.IsOnBeat(100) && stanceChargeLevel < 4 && !beat)
                    {
                        stanceChargeLevel++;
                        Debug.Log("StanceChargeLevel: " + stanceChargeLevel);
                        beat = true;
                    }
                    var forward = cam.transform.forward;
                    var right = cam.transform.right;

                    forward.y = 0f;
                    right.y = 0f;

                    forward.Normalize();
                    right.Normalize();
                    desiredMoveDirection = forward * InputZ + right * InputX;
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

    void Evade()
    {
        if (currentStance == Stances.Aggro)
        {
            if(inputPackage.MoveHorizontal <= -0.5f)
            {
                anim.SetTrigger("evasionLeft");
            }
            else if(inputPackage.MoveHorizontal >= 0.5f)
            {
                anim.SetTrigger("evasionRight");
            }
            else
            {
                anim.SetTrigger("evasionLeft");
            }
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


    public void ColliderEnabled()
    {
        col.enabled = true;
    }
    public void ColliderDisabled()
    {
        col.enabled = false;
    }

}
