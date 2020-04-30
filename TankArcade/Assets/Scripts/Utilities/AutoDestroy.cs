using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime;


    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
            Destroy(gameObject);
    }
}
