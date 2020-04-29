using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;
    public bool isBot = false;


    void Start()
    {
        if (isBot)
            cam = GameObject.Find("MainCam").GetComponent<Transform>();
    }


    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}