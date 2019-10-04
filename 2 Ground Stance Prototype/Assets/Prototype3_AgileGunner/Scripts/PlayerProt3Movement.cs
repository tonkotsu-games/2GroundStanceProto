using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProt3Movement : MonoBehaviour
{
    private BeatAnalyse beatBox;
    private Animator anim;
    private CharacterController controller;
    private Camera cam;
    private Rigidbody rb;

    private float InputX;
    private float InputZ;
    private Vector3 desiredMoveDirection;
    private GameObject boss;

    [SerializeField]
    private float rotationSpeed = 100f;

    [SerializeField]
    private float LandingAccelerationRatio = 0.5f;

    [SerializeField]
    private float baseSpeed = 5f;

    [SerializeField]
    private float speedPerPush = 5f;

    [SerializeField]
    private float maxSpeed = 500f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private float height;

    [SerializeField]
    private float AdditionalGravity = 0.5f; 

    private bool reverse = false;

    private bool aerial;

    AudioSource source;

    [SerializeField]
    AudioClip shotClip;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform bulletSpawnRight;
    [SerializeField]
    Transform bulletSpawnLeft;


    [HideInInspector] public Quaternion PhysicsRotation;
    [HideInInspector] public Quaternion VelocityRotation;
    [HideInInspector] public Quaternion InputRotation;
    [HideInInspector] public Quaternion ComputedRotation;

    public enum PlayerStates { driving,shooting}
    PlayerStates currentPlayerState = PlayerStates.driving;

    // Start is called before the first frame update
    void Start()
    {
        beatBox = GameObject.FindWithTag("MusicBox").GetComponent<BeatAnalyse>();
        boss = GameObject.FindWithTag("Boss");
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        source = gameObject.GetComponent<AudioSource>();
       //controller = gameObject.GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    { 
        #region ResettingTriggers
        anim.ResetTrigger("skateRight");
        anim.ResetTrigger("skateLeft");
        anim.ResetTrigger("shootRight");
        anim.ResetTrigger("shootLeft");
        anim.ResetTrigger("stateSwap");
        #endregion

        float InputX = Input.GetAxis("Horizontal");
        float InputZ = Input.GetAxis("Vertical");

        CheckPhysics();
        Vector2 direction = new Vector2 (InputX,InputZ);
        SkaterMove(direction);
        if (Input.GetButtonDown("CameraButton"))
        {
         
            if(currentPlayerState == PlayerStates.driving)
            {
                anim.SetInteger("state", 1);
                ChangePlayerState(PlayerStates.shooting);
            }
            else if(currentPlayerState == PlayerStates.shooting)
            {
                anim.SetInteger("state", 0);
                ChangePlayerState(PlayerStates.driving);
            }
        }

        if (currentPlayerState == PlayerStates.driving)
        {
            if (Input.GetAxisRaw("TriggerRight") != 0)
            {
                Skate("Right", direction);
            }
            else if (Input.GetAxisRaw("TriggerLeft") != 0)
            {
                Skate("Left", direction);
            }
            
            if (Input.GetButtonDown("A"))
            {
                if (!aerial)
                {
                    rb.AddForce(Vector3.up * jumpForce);
                }
            }
        }

        else if(currentPlayerState == PlayerStates.shooting)
        {
            if (Input.GetAxisRaw("TriggerRight") != 0)
            {
                Shoot("Right");
            }
            else if (Input.GetAxisRaw("TriggerLeft") != 0)
            {
                Shoot("Left");
            }
        }
    }


    private void Skate(string direction,Vector2 moveDirection)
    {
        if (!aerial)
        {
            anim.SetTrigger("skate" + direction);

            Vector3 adapted_direction = CamToPlayer(moveDirection);
            Vector3 planar_direction = transform.forward;
            planar_direction.y = 0;
            InputRotation = Quaternion.FromToRotation(planar_direction, adapted_direction);

            if (rb.velocity.magnitude < maxSpeed)
            {
                Vector3 Direction = InputRotation * transform.forward * speedPerPush;
                rb.AddForce(Direction);
            }
        }

    }

    private void Shoot(string side)
    {
        anim.SetTrigger("shoot" + side);
    }

    void CheckPhysics()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.05f * height))
        {
            if (aerial)
            {
                VelocityOnLanding();
            }
            aerial = false;
        }
        else
        {
            aerial = true;
            rb.velocity += Vector3.down * AdditionalGravity;
        }

    }

    void VelocityOnLanding()
    {
        float magn_vel = rb.velocity.magnitude;
        Vector3 new_vel = rb.velocity;
        new_vel.y = 0;
        new_vel = new_vel.normalized * magn_vel;

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.velocity += LandingAccelerationRatio * new_vel;
        }

    }

    void SkaterMove(Vector2 inputs)
    {

        PhysicsRotation = aerial ? Quaternion.identity : GetPhysicsRotation(); // Rotation according to ground normal 
        VelocityRotation = GetVelocityRot();
        InputRotation = Quaternion.identity;
        ComputedRotation = Quaternion.identity;


        if (inputs.magnitude > 0.1f)
        {
            Vector3 adapted_direction = CamToPlayer(inputs);
            Vector3 planar_direction = transform.forward;
            planar_direction.y = 0;
            InputRotation = Quaternion.FromToRotation(planar_direction, adapted_direction);

            if (!aerial)
            {
                if (rb.velocity.magnitude < maxSpeed)
                {
                    Vector3 Direction = InputRotation * transform.forward * baseSpeed;
                    rb.AddForce(Direction);
                }
            }
        }
          
        

        ComputedRotation = PhysicsRotation * VelocityRotation * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, ComputedRotation, rotationSpeed * Time.deltaTime);
    }

    Vector3 CamToPlayer(Vector2 d)
    {
        Vector3 cam_to_player = transform.position - cam.transform.position;
        cam_to_player.y = 0;

        Vector3 cam_to_player_right = Quaternion.AngleAxis(90, Vector3.up) * cam_to_player;

        Vector3 direction = cam_to_player * d.y + cam_to_player_right * d.x;
        return direction.normalized;
    }

    Quaternion GetPhysicsRotation()
    {
        Vector3 target_vec = Vector3.up;
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.05f * height))
        {
            target_vec = hit.normal;
        }

        return Quaternion.FromToRotation(transform.up, target_vec);
    }

    Quaternion GetVelocityRot()
    {
        Vector3 vel = rb.velocity;
        if (vel.magnitude > 0.2f)
        {
            vel.y = 0;
            Vector3 dir = transform.forward;
            dir.y = 0;
            Quaternion vel_rot = Quaternion.FromToRotation(dir.normalized * 0.05f, vel.normalized);
            return vel_rot;
        }
        else
            return Quaternion.identity;
    }

    public void ChangePlayerState(PlayerStates requestedState)
    {
        if(requestedState!= currentPlayerState)
        {
            Debug.Log("Changing from State: " + currentPlayerState + " to State:  " + requestedState);

            currentPlayerState = requestedState;

            if(requestedState == PlayerStates.driving)
            {

            }
            else if(requestedState == PlayerStates.shooting)
            {

                rb.velocity = Vector3.zero;
                transform.LookAt(boss.transform);
            }
        }
    }
    
    private void SpawnBullet(string side)
    { //
      // if (side == "Right")
      // {
      //     Instantiate(bulletPrefab, bulletSpawnRight.position ,Quaternion.identity);
      // }
      //
      // else if (side == "Left")
      // {
      //     Instantiate(bulletPrefab, bulletSpawnLeft.position,Quaternion.identity);
      // }

        source.clip = shotClip;
        if (!source.isPlaying)
        {
            source.Play();
        }

        Debug.Log("Bullet Spawned");
    }

    private void OnGUI()
    {
        GUILayout.Box(currentPlayerState.ToString());
        GUILayout.Toggle(aerial,"Aerial: ");
        GUILayout.Box("Current Velocity: " + rb.velocity.magnitude);
    }
}
