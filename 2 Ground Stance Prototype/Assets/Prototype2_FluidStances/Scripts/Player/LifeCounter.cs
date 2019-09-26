using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    [SerializeField]
    private int hitPoints;

    private Respawn respawn;

    private void Start()
    {
        respawn = GetComponent<Respawn>();
    }

    public void LifeChange()
    {
        hitPoints -= 1;
        if(hitPoints == 0)
        {
            hitPoints = 3;
            if(respawn != null)
            {
                respawn.RespawnPlayer();
            }
        }
    }

    public void FallToDeath()
    {
        hitPoints = 3;
        respawn.RespawnPlayer();
    }
}
