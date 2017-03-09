using UnityEngine;
using System.Collections;

public delegate void OnABS_Working ();

public class Brake
{
	public event OnABS_Working onABS_Working;

	public float brakeForce;
	public bool hasABS;
	public float ABS_minBrakeForce = 0.1f;
	public float ABS_postExtremumSlip = 0.25f;
	public float ABS_preExtremumSlip = 0.15f;

	public CarControl m_Car;
	Wheel wheel;

	public Brake (Wheel wheel, float brakeForce, bool hasABS)
	{
		this.wheel = wheel;
		this.brakeForce = brakeForce;
		this.hasABS = hasABS;
	}

	public void doBrake ()
	{
		wheel.wheelCollider.motorTorque = 0;
       
		if (hasABS) {
			ABS ();
		} else {
			wheel.wheelCollider.brakeTorque = brakeForce + Mathf.Abs (m_Car.car.engine.getEngineWheelsBrakeTorque ());
		}
       
	}

	public void ABS ()
	{
		if (wheel.wheelCollider.brakeTorque <= ABS_minBrakeForce || wheel.speedInKmH >= m_Car.speedInKmH)
			{
			wheel.wheelCollider.brakeTorque = brakeForce + Mathf.Abs (m_Car.car.engine.getEngineWheelsBrakeTorque ());
			//Debug.Log ("ABS 0");
		}
		else if (wheel.desiredSlip * Mathf.Sin (m_Car.car.transmission.currentGear) < ABS_postExtremumSlip*-1) {		
			//when far after ExtremumSlip decrease brake
			wheel.wheelCollider.brakeTorque *=0.3f*(1- Mathf.Abs (wheel.desiredSlip));
			//Debug.Log ("ABS 1");
			if (onABS_Working != null)
				onABS_Working ();
		} else if (wheel.desiredSlip * Mathf.Sin (m_Car.car.transmission.currentGear) > ABS_preExtremumSlip) {
			//when far before ExtremumSlip add full brake again
			wheel.wheelCollider.brakeTorque *=  brakeForce + Mathf.Abs (m_Car.car.engine.getEngineWheelsBrakeTorque ());
			//Debug.Log ("ABS 2");
		} else {
			//when in the ExtremumSlip zone tye increase brake 
			wheel.wheelCollider.brakeTorque *= 1.5f;
			//Debug.Log ("ABS 3");
		}
		if (wheel.wheelCollider.brakeTorque > brakeForce + Mathf.Abs (m_Car.car.engine.getEngineWheelsBrakeTorque ()))
			wheel.wheelCollider.brakeTorque = brakeForce + Mathf.Abs (m_Car.car.engine.getEngineWheelsBrakeTorque ());
		//Debug.Log (wheel.wheelCollider.brakeTorque);
	}


	public void releaseBrake ()
	{
		wheel.wheelCollider.brakeTorque = 0;
       
	}
}
