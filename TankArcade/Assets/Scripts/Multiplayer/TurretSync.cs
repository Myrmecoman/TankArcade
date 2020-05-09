using Mirror;
using UnityEngine;


public class TurretSync : NetworkBehaviour
{
    public Transform turret;

    private float updateValue = 0;


    // update only every 0.5s
    void Update()
    {
        updateValue += Time.deltaTime;
        if (isLocalPlayer && updateValue >= 0.5f)
        {
            updateValue %= 0.5f;
        }
    }
}