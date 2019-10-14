using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField]
    private Transform point1, point2;

    private GameObject player;
    private PlayerProt3Movement playerScript;
    private Vector3 lane;
    private Vector3 direction;
    private bool grinding = false;

    private void Start()
    {
        lane = point1.position - point2.position;
    }

    private void FixedUpdate()
    {
        if (grinding)
        {
            Debug.Log(playerScript.grindMagnitude);
            player.GetComponent<Rigidbody>().AddForce(direction.normalized*playerScript.grindSpeed);
        }
    }

    public void StopGrinding()
    {

        playerScript.ChangePlayerState(PlayerProt3Movement.PlayerStates.driving);
        grinding = false;
    }
    private void InitializeGrinding(GameObject playerParent)
    {
        player = playerParent;
        playerScript = player.GetComponent<PlayerProt3Movement>();
        float temp = Vector3.Angle(player.transform.forward, lane);
        direction = temp >= 90 ? -lane : lane;
        playerScript.ChangePlayerState(PlayerProt3Movement.PlayerStates.grinding);
    }

    public void StartGrinding(GameObject playerParent)
    {
        grinding = true;
        InitializeGrinding(playerParent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(point1.position, point2.position);
    }
}
