using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public Vector3 direction;
    public float speed = 1;
    public bool shoot = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            transform.Translate(direction*speed*Time.deltaTime);
        }
    }

    
}
