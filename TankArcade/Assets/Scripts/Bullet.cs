using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;

    private float time = 0;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        time += Time.deltaTime;
        if (time > 50)
            Destroy(gameObject);
        transform.position += transform.forward * Time.deltaTime * speed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collison");
        if (collision.transform.tag == "Player")
        {
            Debug.Log("hit player");
            collision.transform.GetComponent<ControllerTest>().HitbyShell(20);
            Destroy(gameObject);
        }
        else
        {
            
        }
    }
}