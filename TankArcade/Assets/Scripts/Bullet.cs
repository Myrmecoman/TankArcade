using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            Debug.Log("hit player");
    }
}