using UnityEngine;


namespace Mirror.Examples.NetworkRoom
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))]

	public class Controller : NetworkBehaviour
	{
		public float speed = 10.0f;
		public float gravity = 9.8f;
		public float maxVelocityChange = 10.0f;
		public Transform turret;

		private bool grounded = false;
		private Rigidbody rig;


		void Awake()
		{
			rig = GetComponent<Rigidbody>();
			rig.freezeRotation = true;
			rig.useGravity = false;
		}


		public override void OnStartLocalPlayer()
		{
			base.OnStartLocalPlayer();

			Camera.main.orthographic = false;
			Camera.main.transform.SetParent(transform);
			Camera.main.transform.localPosition = new Vector3(0, 40, -8);
			Camera.main.transform.localEulerAngles = new Vector3(70, 0, 0);
		}


		void OnDisable()
		{
			if (isLocalPlayer && Camera.main != null)
			{
				Camera.main.orthographic = true;
				Camera.main.transform.SetParent(null);
				Camera.main.transform.localPosition = new Vector3(0, 70, 0);
				Camera.main.transform.localEulerAngles = new Vector3(90, 0, 0);
			}
		}


		void FixedUpdate()
		{
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
				Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				targetVelocity = transform.TransformDirection(targetVelocity);
				targetVelocity *= speed;
				Vector3 velocity = rig.velocity;
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0;
				rig.AddForce(velocityChange, ForceMode.VelocityChange);
			}

			// We apply gravity manually for more tuning control
			rig.AddForce(new Vector3(0, -gravity * rig.mass, 0));
			grounded = false;
		}


		void OnCollisionStay()
		{
			grounded = true;
		}
	}
}