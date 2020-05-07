using UnityEngine;
using Mirror;


public class NetBillBoard : NetworkBehaviour
{
    public Transform cam;


    void Start()
    {
        // if not localplayer, find the localplayer's camera and use it for the billboards
        if (!gameObject.GetComponentInParent<MultiTank>().isLocalPlayer)
        {
            GameObject[] cams = GameObject.FindGameObjectsWithTag("MainCamera");
            for(int i = 0; i < cams.Length; i++)
            {
                if(cams[i].gameObject.GetComponentInParent<MultiTank>().isLocalPlayer)
                {
                    cam = cams[i].transform;
                    break;
                }
            }
        }
    }


    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}