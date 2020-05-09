using UnityEngine;


public class NetBillBoard : MonoBehaviour
{
    public Transform cam;
    public Transform healthTrans;


    void Start()
    {
        // if not localplayer, find the localplayer's camera and use it for the billboards
        if (!gameObject.GetComponent<MultiTank>().isLocalPlayer)
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
        healthTrans.LookAt(healthTrans.position + cam.forward);
    }
}