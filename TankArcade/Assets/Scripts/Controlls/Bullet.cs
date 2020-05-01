using UnityEngine;


public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public GameObject popSound;

    private float time = 30;
    private int bounces = 3;
    private float damage = 20;


    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
            Destroy(gameObject);
        transform.position += transform.forward * Time.deltaTime * speed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<ControllerTest>().HitbyShell(damage);
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Bot")
        {
            collision.transform.GetComponent<BotController>().HitbyShell(damage);
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Shell")
            Destroy(gameObject);
        else
        {
            Instantiate(popSound, transform.position, Quaternion.identity, null);
            ContactPoint contact = collision.contacts[0];
            Vector3 newDir = Vector3.zero;
            var curDir = transform.TransformDirection(Vector3.forward);
            newDir = Vector3.Reflect(curDir, contact.normal);
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);

            bounces -= 1;
            if (bounces == 0)
                Destroy(gameObject);
        }
    }
}