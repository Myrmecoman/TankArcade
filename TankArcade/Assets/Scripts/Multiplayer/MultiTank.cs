using UnityEngine;
using Mirror;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NetworkTransform))]

public class MultiTank : NetworkBehaviour
{
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public Camera cam;
	public Transform turret;
	public Transform camPivot;
	public GameObject shell;
	public Transform shellPos;
	public Material MatDestroyed;
	public HealthBar healthBar;
	public GameObject Smoke;
	[HideInInspector]
	public bool destroyed = false;

	private bool grounded = false;
	private int horizontal;
	private int vertical;
	private Rigidbody rig;
	private CameraShake shake;
	private AudioSource explode;
	private int RotValue;
	private float PivotValue;
	private InputManager im;

	//stats
	private float health = 100;
	private float reloadTime = 0.5f;


	void Awake()
	{
		rig = GetComponent<Rigidbody>();
		shake = GetComponent<CameraShake>();
		explode = GetComponent<AudioSource>();
	}


	void Start()
	{
		if (!isLocalPlayer)
		{
			cam.gameObject.SetActive(false);
			return;
		}

		PivotValue = turret.eulerAngles.y;
		im = InputManager.instance;
	}


	void Update()
	{
		if (!isLocalPlayer)
			return;

		RotValue = 0;
		if (im.GetKey(KeybindingActions.camLeft))
			RotValue = -1;
		if (im.GetKey(KeybindingActions.camRight))
			RotValue = 1;
		PivotValue = PivotValue + RotValue * Time.deltaTime * 200;
		camPivot.eulerAngles = new Vector3(0, PivotValue, 0);

		if (turret.localEulerAngles.y >= 235 && turret.localEulerAngles.y <= 315)
			shellPos.localPosition = new Vector3(0, 0.5f, 1.7f + Mathf.Abs(turret.localEulerAngles.y - 275) * 0.02f);
		else if (turret.localEulerAngles.y >= 45 && turret.localEulerAngles.y <= 125)
			shellPos.localPosition = new Vector3(0, 0.5f, 1.7f + Mathf.Abs(turret.localEulerAngles.y - 85) * 0.02f);
		else
			shellPos.localPosition = new Vector3(0, 0.5f, 2.5f);

		if (reloadTime < 0.5)
			reloadTime += Time.deltaTime;
		if (im.GetKey(KeybindingActions.shoot) && reloadTime >= 0.5)
		{
			reloadTime = 0;
			Instantiate(shell, shellPos.position, shellPos.rotation, null);
		}
	}


	void FixedUpdate()
	{
		if (!isLocalPlayer)
			return;

		// turret look at mouse
		Plane playerPlane = new Plane(Vector3.up, turret.position);
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
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
			if (im.GetKey(KeybindingActions.forward))
				vertical = 1;
			if (im.GetKey(KeybindingActions.backward))
				vertical = -1;
			if (im.GetKey(KeybindingActions.left))
			{
				if (vertical != 0)
					horizontal = -1 * vertical;
				else
					horizontal = -1;
			}
			if (im.GetKey(KeybindingActions.right))
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
		if (!isLocalPlayer)
			return;

		health -= dmg;
		healthBar.SetHealth(health);
		if (health <= 0)
		{
			Debug.Log("player tank destroyed");
			destroyed = true;
			gameObject.tag = "Untagged";
			Destroy(healthBar.gameObject);
			shake.shakeDuration = 0.2f;
			explode.Play();
			Instantiate(Smoke, transform.position, Quaternion.identity, transform);

			// setting all mats to destroyed
			// hull
			Material[] mats = GetComponent<Renderer>().materials;
			mats[0] = MatDestroyed;
			mats[1] = MatDestroyed;
			mats[2] = MatDestroyed;
			GetComponent<Renderer>().materials = mats;
			// turret
			Material[] matsTurret = transform.GetChild(1).GetComponent<Renderer>().materials;
			matsTurret[0] = MatDestroyed;
			matsTurret[1] = MatDestroyed;
			transform.GetChild(1).GetComponent<Renderer>().materials = matsTurret;
			// gun
			Material[] matsGun = transform.GetChild(1).GetChild(0).GetComponent<Renderer>().materials;
			matsGun[0] = MatDestroyed;
			transform.GetChild(1).GetChild(0).GetComponent<Renderer>().materials = matsGun;

			Destroy(this);
		}
	}
}