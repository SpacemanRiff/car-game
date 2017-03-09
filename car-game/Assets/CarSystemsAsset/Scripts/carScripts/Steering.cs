using UnityEngine;
using System.Collections;

public class Steering : MonoBehaviour {

    public float maxSpeedSteeringAngle = 3; // steering Angle to apply when the car at its maximum speed
    public float lowSpeedFSteerAngle = 15; //steering Angle to apply when the car at its minimum speed
    public float m_SteerAngle;
    Car car;
    CarControl carControl;
    Vector3 wheelsRotation;

    void Start () {
        car = GetComponent<Car>();
        carControl = GetComponent<CarControl>();

    }
	
    public void calcSteerAngle(float inputValue)
    {
        m_SteerAngle = Mathf.Lerp(lowSpeedFSteerAngle, maxSpeedSteeringAngle,
            carControl.getSpeedFactor()) * inputValue;

        for (int i = 0; i < 2; i++)
        {
            car.frontWheels[i].wheelCollider.steerAngle = m_SteerAngle;
        }

      //  if ( car.accessories.hasCountersteering )
        //    WheelsCountersteering(inputValue);
    }

    public void WheelsCountersteering(float inputValue)
    {
        for(int i = 0; i < 4; i++)
        {
            wheelsRotation = car.wheels[i].wheelCollider.transform.localRotation.eulerAngles;
            wheelsRotation.z = Mathf.Lerp(maxSpeedSteeringAngle, lowSpeedFSteerAngle,
            carControl.getSpeedFactor()) * inputValue;
            car.wheels[i].wheelCollider.transform.localRotation = Quaternion.Euler(wheelsRotation.x, wheelsRotation.y, wheelsRotation.z);
        }
    }
}
