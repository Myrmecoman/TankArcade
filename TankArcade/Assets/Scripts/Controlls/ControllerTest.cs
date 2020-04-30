using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]

public class ControllerTest : MonoBehaviour
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

	private bool grounded = false;
	private int horizontal;
	private int vertical;
	private Rigidbody rig;
	private CameraShake shake;
	private AudioSource explode;
	private InputManager im;

	//stats
	private float health = 100;
	private float reloadTime = 0.5f;


	void Awake()
	{
		rig = GetComponent<Rigidbody>();
		shake = GetComponent<CameraShake>();
		explode = GetComponent<AudioSource>();
		im = InputManager.instance;
	}


	void Update()
	{
		if (reloadTime < 0.5)
			reloadTime += Time.deltaTime;
		if (im.GetKey(KeybindingActions.shoot) && reloadTime >= 0.5)
		{
			reloadTime = 0;
			Instantiate(shell, shellPos.position, shellPos.rotation, null);
		}
		camPivot.position = transform.position;
	}


	void FixedUpdate()
	{
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
		health -= dmg;
		healthBar.SetHealth(health);
		if (health <= 0)
		{
			Debug.Log("tank destroyed");
			gameObject.tag = "Untagged";
			Destroy(healthBar.gameObject);
			shake.shakeDuration = 0.2f;
			explode.Play();
			Instantiate(Smoke, turret.position, transform.rotation, turret);

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