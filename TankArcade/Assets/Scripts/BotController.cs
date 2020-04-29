using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class BotController : MonoBehaviour
{
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public Transform turret;
	public GameObject shell;
	public Transform shellPos;
	public Material MatDestroyed;
	
	private bool grounded = false;/*
	private int horizontal;
	private int vertical;*/
	private Rigidbody rig;
	private Transform playerPos;

	//stats
	private float health = 100;
	private float rng;


	void Awake()
	{
		rig = GetComponent<Rigidbody>();
		float rng = Random.Range(1, 5);
	}


	void Start()
	{
		playerPos = GameObject.Find("turret").transform;
	}


	void Update()
	{
		if (rng >= 0)
			rng -= Time.deltaTime;
		else
		{
			Instantiate(shell, shellPos.position, shellPos.rotation, null);
			rng = Random.Range(1, 5);
		}
	}


	void FixedUpdate()
	{
		Quaternion targetRotation = Quaternion.LookRotation(playerPos.position - turret.position);
		turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, 180 * Time.fixedDeltaTime);

		/*
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
		*/
		grounded = false;
	}


	void OnCollisionStay()
	{
		grounded = true;
	}


	public void HitbyShell(float dmg)
	{
		health -= dmg;
		if (health <= 0)
		{
			Debug.Log("tank destroyed");
			gameObject.tag = "Untagged";

			// setting all mats to destroyed
			// hull
			Material[] mats = GetComponent<Renderer>().materials;
			mats[0] = MatDestroyed;
			mats[1] = MatDestroyed;
			mats[2] = MatDestroyed;
			GetComponent<Renderer>().materials = mats;
			// turret
			Material[] matsTurret = transform.GetChild(0).GetComponent<Renderer>().materials;
			matsTurret[0] = MatDestroyed;
			matsTurret[1] = MatDestroyed;
			transform.GetChild(0).GetComponent<Renderer>().materials = matsTurret;
			// gun
			Material[] matsGun = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().materials;
			matsGun[0] = MatDestroyed;
			transform.GetChild(0).GetChild(0).GetComponent<Renderer>().materials = matsGun;

			Destroy(this);
		}
	}
}