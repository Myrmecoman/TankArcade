using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class ControllerTest : MonoBehaviour
{
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public Camera cam;
	public Transform turret;
	public Transform camPivot;
	public GameObject shell;
	public Transform shellPos;

	private bool grounded = false;
	private int horizontal;
	private int vertical;
	private int value;
	private Rigidbody rig;

	//stats
	private float health = 100;
	private float reloadTime = 0.5f;


	void Awake()
	{
		rig = GetComponent<Rigidbody>();
	}


	void Update()
	{
		if (reloadTime < 0.5)
			reloadTime += Time.deltaTime;
		if (Input.GetMouseButton(0) && reloadTime >= 0.5)
		{
			Debug.Log("shoot");
			reloadTime = 0;
			Instantiate(shell, shellPos.position, shellPos.rotation, null);
		}
	}


	void FixedUpdate()
	{
		// camera rotation
		value = 0;
		if (Input.GetKey(KeyCode.A))
			value = -1;
		if (Input.GetKey(KeyCode.E))
			value = 1;
		camPivot.eulerAngles = new Vector3(0, camPivot.eulerAngles.y + value * Time.fixedDeltaTime * 150, 0);
		camPivot.position = transform.position;

		// turret look at mouse
		Plane playerPlane = new Plane(Vector3.up, turret.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hitdist = 0.0f;
		if (playerPlane.Raycast(ray, out hitdist))
		{
			Vector3 targetPoint = ray.GetPoint(hitdist);
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - turret.position);
			turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, 180 * Time.fixedDeltaTime);
		}

		// tank movements
		if (grounded)
		{
			horizontal = 0;
			vertical = 0;
			if (Input.GetKey(KeyCode.Z))
				vertical = 1;
			if (Input.GetKey(KeyCode.S))
				vertical = -1;
			if (Input.GetKey(KeyCode.Q))
			{
				if (vertical != 0)
					horizontal = -1 * vertical;
				else
					horizontal = -1;
			}
			if (Input.GetKey(KeyCode.D))
			{
				if (vertical != 0)
					horizontal = 1 * vertical;
				else
					horizontal = 1;
			}
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + horizontal * Time.fixedDeltaTime * 150, 0);
			Vector3 targetVelocity = new Vector3(0, 0, vertical);
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;
			Vector3 velocity = rig.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			rig.AddForce(velocityChange, ForceMode.VelocityChange);
		}

		grounded = false;
	}


	void OnCollisionStay()
	{
		grounded = true;
	}


	public void HitbyShell(float dmg)
	{
		health -= dmg;
		if(health <= 0)
		{
			Debug.Log("tank destroyed");
		}
	}
}