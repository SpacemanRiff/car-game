using UnityEngine;
using System.Collections;

public class EPSSystem : MonoBehaviour
{

    CarControl m_Car;

    void Start()
    {
        m_Car = GetComponent<CarControl>();
    }
   public void doUpdate()
    {
       // if (m_Car.speedInKmH < 30)
         //   return;
        if (m_Car.statuse != CarStause.Forward)
            return;
        if (m_Car.car.steering.m_SteerAngle > 0)
        {
            brakeInnerWheels(m_Car.car.rightWheels);
			addMoreTorque(m_Car.car.rightWheels, m_Car.car.leftWheels);
        }
        else if (m_Car.car.steering.m_SteerAngle < 0)
        {
            brakeInnerWheels(m_Car.car.leftWheels);
			addMoreTorque(m_Car.car.leftWheels, m_Car.car.rightWheels);
        }
    }

    void brakeInnerWheels(Wheel[] wheels)
    {
        for (int i = 0; i < 2; i++)
        {
			if(wheels[i].wheelCollider.brakeTorque ==0)
			wheels[i].wheelCollider.brakeTorque = 1f  * m_Car.getSpeedFactor() ;
        }
    }

    void addMoreTorque(Wheel[] fromWheels, Wheel[] toWheels)
    {
        for (int i = 0; i < 2; i++)
        {
            fromWheels[i].currentengineTorquePercen -= 0.01f * m_Car.getSpeedFactor() ;
            toWheels[i].currentengineTorquePercen += 0.01f * m_Car.getSpeedFactor() ;
        }

    }
    
}
