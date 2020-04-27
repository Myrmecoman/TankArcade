using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Controller : MonoBehaviour
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


	void FixedUpdate()
	{
		if (grounded)
		{
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;

			// Apply a force that attempts to reach our target velocity
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