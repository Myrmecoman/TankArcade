using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;


    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }


    private void OnCollisionEnter(Collision collision)
    {
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