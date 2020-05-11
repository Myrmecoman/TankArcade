using UnityEngine;
using Mirror;


public class MultiBullet : NetworkBehaviour
{
    public GameObject popSound;
    public float speed = 10;
    public float time = 30;
    public int bounces = 3;

    private Rigidbody rb;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }


    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
            Destroy(gameObject);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Shell")
            Destroy(gameObject);
        else
        {
            Instantiate(popSound, transform.position, Quaternion.identity, null);
            ContactPoint contact = collision.contacts[0];
            Vector3 newDir = Vector3.zero;
            var curDir = transform.TransformDirection(Vector3.forward);
            newDir = Vector3.Reflect(curDir, contact.normal);
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
            rb.velocity = transform.forward * speed;

            if (bounces == 0)
                Destroy(gameObject);
            else
                bounces -= 1;
        }
    }
}