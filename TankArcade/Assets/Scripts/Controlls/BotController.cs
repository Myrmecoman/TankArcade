using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]

public class BotController : MonoBehaviour
{
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public Transform turret;
	public GameObject shell;
	public Transform shellPos;
	public Material MatDestroyed;
	public HealthBar healthBar;
	public GameObject Smoke;

	private Transform playerPos;
	private ControllerTest player;
	private Rigidbody playerRig;
	private AudioSource explode;
	private NavMeshAgent agent;

	//stats
	private float health = 100;
	private float rng;


	void Start()
	{
		playerPos = GameObject.Find("turret").transform;
		player = GameObject.Find("chassis").GetComponent<ControllerTest>();
		playerRig = player.GetComponent<Rigidbody>();
		explode = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		float rng = Random.Range(1, 5);
	}


	void Update()
	{
		if (player.destroyed)
		{
			agent.updatePosition = false;
			agent.updateRotation = false;
			return;
		}

		agent.destination = playerPos.position;
			
		if (rng >= 0)
			rng -= Time.deltaTime;
		else
		{
			RaycastHit hit;
			if (Physics.Raycast(shellPos.position, shellPos.TransformDirection(Vector3.forward), out hit, 2000))
			{
				RaycastHit reflected;
				Vector3 refl = Vector3.Reflect(hit.point - shellPos.position, hit.normal);
				if (Physics.Raycast(hit.point, hit.transform.TransformDirection(Vector3.left) * hit.distance, out reflected, 2000))
				{
					//Debug.DrawRay(shellPos.position, shellPos.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
					//Debug.DrawRay(hit.point, hit.transform.TransformDirection(Vector3.left) * hit.distance, Color.red);

					if (!(reflected.collider.tag == "Bot") && !(hit.collider.tag == "Bot"))
					{
						Instantiate(shell, shellPos.position, shellPos.rotation, null);
						rng = Random.Range(1, 5);
					}
				}
			}
		}
	}


	void FixedUpdate()
	{
		Quaternion targetRotation = Quaternion.LookRotation((playerPos.position + playerRig.velocity * 0.7f) - turret.position);
		turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, 180 * Time.fixedDeltaTime);
	}


	public void HitbyShell(float dmg)
	{
		health -= dmg;
		healthBar.SetHealth(health);
		if (health <= 0)
		{
			agent.updatePosition = false;
			agent.updateRotation = false;
			Debug.Log("tank destroyed");
			gameObject.tag = "Untagged";
			Destroy(healthBar.gameObject);
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