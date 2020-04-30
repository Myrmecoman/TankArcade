using UnityEngine;

public class CamRot : MonoBehaviour
{
	public Transform camPivot;

	private int value;


    void Update()
    {
		value = 0;
		if (Input.GetKey(KeyCode.A))
			value = -1;
		if (Input.GetKey(KeyCode.E))
			value = 1;
		camPivot.eulerAngles = new Vector3(0, camPivot.eulerAngles.y + value * Time.deltaTime * 150, 0);
	}
}