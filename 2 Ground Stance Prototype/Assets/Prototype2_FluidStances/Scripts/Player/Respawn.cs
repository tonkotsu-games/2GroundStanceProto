using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public void RespawnPlayer()
    {
        transform.position = new Vector3(0, 0, 7.5f);
    }
}
