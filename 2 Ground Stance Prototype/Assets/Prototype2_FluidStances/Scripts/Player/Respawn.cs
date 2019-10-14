using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform respawnZone;

    public void RespawnPlayer()
    {
        transform.position = respawnZone.position;
    }
}
