using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public Vector3 direction;
    public float speed = 1;
    public bool shoot = false;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            transform.Translate(direction*speed*Time.deltaTime);
        }

        if (Vector3.Distance(startPos, transform.position) > 20)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<LifeCounter>() != null)
        {
            other.gameObject.GetComponent<LifeCounter>().LifeChange();
        }
    }


}
