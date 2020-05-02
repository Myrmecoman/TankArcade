using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform player;
    public Camera cam;


    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(player.position.x, player.position.y, player.position.z),
            5 * Time.deltaTime);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && cam.fieldOfView > 50) // forward
            cam.fieldOfView -= 4;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && cam.fieldOfView < 70) // backwards
            cam.fieldOfView += 4;
    }
}