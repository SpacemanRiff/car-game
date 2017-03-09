using UnityEngine;

[System.Serializable]
public class HandBraking
{

  
    public bool isHandBraked = false;
    public CarControl m_Car;
   // public float handBrakeForwardStifftness = 0.5f;// forward stifftness While Hand Braking
  //  public float handBrakeSideStifftness = 0.2f;// sideward stifftness While Hand Braking



    public void beforStartHandBrakeSimulator()
    {

        for (int i = 0; i < 2; i++)
        {
             m_Car.car.frontWheels[i].wheelCollider.brakeTorque = float.MaxValue;
        }
        
       isHandBraked = true;
    }

    public void doHandBrake( )
    {
      
      //  if (m_Car.car.carDriveType == CarDriveType.FWD)
           wheelsHandBrake(m_Car.car.backWheels);
       // else
          //  wheelsHandBrake(m_Car.car.frontWheels);
        isHandBraked = true;
    }
    public void wheelsHandBrake(Wheel[] wheels)
    {
        for (int i = 0; i < 2; i++)
        {
			wheels[i].wheelCollider.brakeTorque = wheels[i].brake.brakeForce;
        }
    }
    public void releaseHandBrake()
    {
          if(isHandBraked)
        for (int i =0; i<2;i++)
        {
                m_Car.car.backWheels[i].wheelCollider.brakeTorque = 0;
        }
        isHandBraked = false;
    }
}
