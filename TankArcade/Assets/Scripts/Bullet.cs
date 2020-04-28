using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;

    private float time = 0;
    private int bounce = 0;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }


    void Update()
    {
        time += Time.deltaTime;
        if (time > 30)
            Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("hit player");
            collision.transform.GetComponent<ControllerTest>().HitbyShell(20);
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Shell")
            Destroy(gameObject);
        else
        {
            Vector3 orthogonalVector = collision.contacts[0].point - transform.position;
            float collisionAngle = 180 - Vector3.Angle(orthogonalVector, rb.velocity);
            Debug.Log("collision angle : " + collisionAngle);
            bounce += 1;
            if (bounce == 4)
                Destroy(gameObject);
        }
    }
}