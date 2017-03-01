using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour {

	public float idealRPM = 500f;
	public float maxRPM = 1000f;

	public Transform centerOfGravity;

	public WheelCollider wheelFR;
	public WheelCollider wheelFL;
	public WheelCollider wheelRR;
	public WheelCollider wheelRL;

    public GameObject booster;

	public float turnRadius = 6f;
	public float torque = 25f;
	public float brakeTorque = 100f;
    public float boostPower = 1000f;

	public float AntiRoll = 20000.0f;

	public enum DriveMode { Front, Rear, All };
	public DriveMode driveMode = DriveMode.Rear;

	public Text speedText;

    private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfGravity.localPosition;

    }

	public float Speed() {
		return ((wheelRR.radius + wheelRL.radius)/2) * Mathf.PI * ((wheelRR.rpm + wheelRL.rpm)/2) * 60f / 1000f;
	}

	public float Rpm() {
		return wheelRL.rpm;
	}

	void FixedUpdate () {

		if(speedText!=null)
			speedText.text = "Speed: " + Speed()/*.ToString("f0")*/ + " km/h";

		float scaledTorque = Input.GetAxis("Drive") * torque;

		//if(wheelRL.rpm < idealRPM)
			scaledTorque = Mathf.Lerp(scaledTorque/10f, scaledTorque, 0/*wheelRL.rpm / idealRPM*/ );
		//else 
			//scaledTorque = Mathf.Lerp(scaledTorque, 0,  (wheelRL.rpm-idealRPM) / (maxRPM-idealRPM) );

		DoRollBar(wheelFR, wheelFL);
		DoRollBar(wheelRR, wheelRL);

		wheelFR.steerAngle = Input.GetAxis("Horizontal") * turnRadius;
		wheelFL.steerAngle = Input.GetAxis("Horizontal") * turnRadius;

		wheelFR.motorTorque = driveMode==DriveMode.Rear  ? 0 : scaledTorque;
		wheelFL.motorTorque = driveMode==DriveMode.Rear  ? 0 : scaledTorque;
		wheelRR.motorTorque = driveMode==DriveMode.Front ? 0 : scaledTorque;
		wheelRL.motorTorque = driveMode==DriveMode.Front ? 0 : scaledTorque;

		if(Input.GetButton("Brake")) {
			wheelFR.brakeTorque = brakeTorque;
			wheelFL.brakeTorque = brakeTorque;
			wheelRR.brakeTorque = brakeTorque;
			wheelRL.brakeTorque = brakeTorque;
            Debug.Log("brake");
        }
        if (Input.GetButton("Boost"))
        {
            rb.AddForce(transform.forward*boostPower);
            Debug.Log("boost");
        }
        else {
			wheelFR.brakeTorque = 0;
			wheelFL.brakeTorque = 0;
			wheelRR.brakeTorque = 0;
			wheelRL.brakeTorque = 0;
		}
	}


	void DoRollBar(WheelCollider WheelL, WheelCollider WheelR) {
		WheelHit hit;
		float travelL = 1.0f;
		float travelR = 1.0f;
		
		bool groundedL = WheelL.GetGroundHit(out hit);
		if (groundedL)
			travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;
		
		bool groundedR = WheelR.GetGroundHit(out hit);
		if (groundedR)
			travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;
		
		float antiRollForce = (travelL - travelR) * AntiRoll;
		
		if (groundedL)
			GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * -antiRollForce,
			                             WheelL.transform.position); 
		if (groundedR)
			GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * antiRollForce,
			                             WheelR.transform.position); 
	}

}
