using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example skid script. Put this on a WheelCollider.
// Copyright 2017 Nition, BSD licence (see LICENCE file). http://nition.co
[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour {

	// INSPECTOR SETTINGS

	[SerializeField]
	Rigidbody rb;
	private bool trailPlaying = false;
	private bool canStop = false;
	private float duration;
	[SerializeField] private ParticleSystem trail;
	//[SerializeField]
	//Skidmarks skidmarksController;

	// END INSPECTOR SETTINGS

	WheelCollider wheelCollider;
	WheelHit wheelHitInfo;

	const float SKID_FX_SPEED = 0.5f; // Min side slip speed in m/s to start showing a skid
	const float MAX_SKID_INTENSITY = 5.0f; // m/s where skid opacity is at full intensity
	const float WHEEL_SLIP_MULTIPLIER = 10.0f; // For wheelspin. Adjust how much skids show
	int lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
	float lastFixedUpdateTime;

	// #### UNITY INTERNAL METHODS ####

	protected void Awake() {
		wheelCollider = GetComponent<WheelCollider>();
		lastFixedUpdateTime = Time.time;
		duration = trail.main.duration;
	}

	protected void FixedUpdate() {
		lastFixedUpdateTime = Time.time;
	}

	protected void LateUpdate() {
		if (wheelCollider.GetGroundHit(out wheelHitInfo))
		{
			// Check sideways speed

			// Gives velocity with +z being the car's forward axis
			Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
			float skidTotal = Mathf.Abs(localVelocity.x);

			// Check wheel spin as well

			float wheelAngularVelocity = wheelCollider.radius * ((2 * Mathf.PI * wheelCollider.rpm) / 60);
			float carForwardVel = Vector3.Dot(rb.velocity, transform.forward);
			float wheelSpin = Mathf.Abs(carForwardVel - wheelAngularVelocity) * WHEEL_SLIP_MULTIPLIER;

			// NOTE: This extra line should not be needed and you can take it out if you have decent wheel physics
			// The built-in Unity demo car is actually skidding its wheels the ENTIRE time you're accelerating,
			// so this fades out the wheelspin-based skid as speed increases to make it look almost OK
			wheelSpin = Mathf.Max(0.4f, wheelSpin * (10 - Mathf.Abs(carForwardVel)));

			skidTotal += wheelSpin;

			// Skid if we should
			if (skidTotal >= SKID_FX_SPEED) {
				
				float intensity = Mathf.Clamp01(skidTotal / MAX_SKID_INTENSITY);
				// Account for further movement since the last FixedUpdate
				Vector3 skidPoint = wheelHitInfo.point + (rb.velocity * (Time.time - lastFixedUpdateTime));
				lastSkid = Skidmarks.instance.AddSkidMark(skidPoint, wheelHitInfo.normal, intensity, lastSkid);
				if (trailPlaying)
					return;
				playTrail();
				
				/*GameObject newParticle = Instantiate(trail.gameObject, transform.position, transform.rotation);
				ParticleSystem.MinMaxCurve minMaxCurve = newParticle.GetComponent<ParticleSystem>().emission.rateOverTime;
				intensity = intensity < 0.5f ? 0.5f : intensity;
				minMaxCurve.constant = minMaxCurve.constant * intensity;
				var emission = newParticle.GetComponent<ParticleSystem>().emission;
				emission.rateOverTime = minMaxCurve;*/
			}
			else {
				lastSkid = -1;
				stopTrail();
			}
		}
		else {
			lastSkid = -1;
			//trail.Stop();
			stopTrail();
		}
	}

	private void playTrail()
	{
		trail.Play();
		trailPlaying = true;
		StartCoroutine("changeCanStop");
		//Debug.Log("Playing");
	}

	private void stopTrail()
	{
		if (canStop)
		{
			trail.Stop();
			trailPlaying = false;
			//Debug.Log("Stopping");
		}
	}

	IEnumerator changeCanStop()
	{
		canStop = false;
		yield return new WaitForSeconds(duration);
		canStop = true;
	}

	// #### PUBLIC METHODS ####

	// #### PROTECTED/PRIVATE METHODS ####


}
