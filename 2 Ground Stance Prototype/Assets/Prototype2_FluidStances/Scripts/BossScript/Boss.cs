﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] [Tooltip("The speed the boss adjusts towards the players position")][Range(0,2)]
    private float adjustSpeed = 0.5f;
    [SerializeField] [Tooltip("The Maximum Range the player has to be in for the boss to initialize an attack")][Range(0,5)]
    private float aggroRange = 1;
    [SerializeField] [Tooltip("The layer assigned to the player")]
    private int layerMaskPlayer = 9;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawner;
    private GameObject player;
    private Animator bossAnim;
    private Vector3 desiredLookDirection;
    private bool attacking;
    private bool facingPlayer = false;
    public float minAngle = 0.6f;
    [SerializeField]
    private List<Collider> cols;

    // Start is called before the first frame update
    void Start()
    {
        layerMaskPlayer = 1 << layerMaskPlayer;
        bossAnim = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            Debug.LogWarning("Player not tagged!!!");
        }
        foreach (Collider collider in cols)
        {
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        RotateTowardsPlayer();
    }

    
    private void FixedUpdate()
    {
        if(!attacking)
        {
            CheckIfPlayerInRange();
        }
    }

    private void CheckIfPlayerInRange()
    {
        if (facingPlayer)
        {
            if (Physics.CheckSphere(transform.position - new Vector3(0, 1.5f, 0), aggroRange, layerMaskPlayer))
            {
                bossAnim.SetTrigger("attacking");
                StartCoroutine(AnimationLockTimer(0));
            }

            else
            {
                bossAnim.SetTrigger("RangedWindup");
                Debug.Log("ranged attack");
                StartCoroutine(AnimationLockTimer(3));
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        desiredLookDirection = player.transform.position - transform.position;
        desiredLookDirection.y = 0f;
        desiredLookDirection.Normalize();
        facingPlayer = Vector3.Angle(transform.forward, desiredLookDirection) < minAngle ? true : false;
        if (!facingPlayer)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredLookDirection), adjustSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position - new Vector3(0,1.5f,0), aggroRange);
    }

    IEnumerator AnimationLockTimer(float bonusDelay)
    {
        attacking = true;
        yield return new WaitForSeconds(bossAnim.GetCurrentAnimatorStateInfo(0).length + 1);
        yield return new WaitForSeconds(bonusDelay);
        attacking = false;
    }

    public void ColliderEnabled()
    {
        foreach (Collider collider in cols)
        {
            collider.enabled = true;
        }
    }
    public void ColliderDisabled()
    {
        foreach (Collider collider in cols)
        {
            collider.enabled = false;
        }
    }

    public void SpawnProjectile()
    {
        var p = Instantiate(projectilePrefab, projectileSpawner.position, Quaternion.identity);
        ProjectileController pScript = p.GetComponent<ProjectileController>();
        pScript.direction = player.transform.position - transform.position;
        pScript.shoot = true;

    }

}
