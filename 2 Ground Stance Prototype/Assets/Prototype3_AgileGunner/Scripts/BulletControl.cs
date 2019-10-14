using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{

    Vector3 direction;
    public GameObject target;
    public float bulletSpeed;
    public bool shoot = false;
    [SerializeField]
    float lifetime = 5;
    // Start is called before the first frame update
    void Start()
    {      
        direction =target.transform.position- transform.position;
        transform.LookAt(target.transform.position);
        StartCoroutine(LifeTime(lifetime));
        shoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, bulletSpeed*Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    IEnumerator LifeTime(float life)
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }
}
