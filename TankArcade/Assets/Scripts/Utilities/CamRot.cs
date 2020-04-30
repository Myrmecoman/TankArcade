using UnityEngine;

public class CamRot : MonoBehaviour
{
	public Transform camPivot;

	private int value;
	private InputManager im;


	void Start()
	{
		im = InputManager.instance;
	}


	void Update()
    {
		value = 0;
		if (im.GetKey(KeybindingActions.camLeft))
			value = -1;
		if (im.GetKey(KeybindingActions.camRight))
			value = 1;
		camPivot.eulerAngles = new Vector3(0, camPivot.eulerAngles.y + value * Time.deltaTime * 150, 0);
	}
}