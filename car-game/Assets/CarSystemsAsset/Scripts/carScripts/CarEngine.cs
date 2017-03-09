using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CarEngine
{

    public int maxRPM = 7000;
    public int minRPM = 700;

    public float currentRPM;
    public float maxEngineBrake = 30f;
    
    public CarControl m_Car;

    //float avilableTorquePercent = 1f;
   


    

    float tmpRPM;
    //float speedRang;
    public void updateCurrentRPM()
    {
        //  avilableTorquePercent = 1f;
        tmpRPM = (m_Car.speedInKmH * m_Car.car.transmission.getCurrentGearRatio() *
           m_Car.car.transmission.differentialRatio *(1000/60))
           /
           (m_Car.car.motorWheels[0].wheelCollider.radius* 2*Mathf.PI);
        currentRPM = Mathf.Lerp(currentRPM, tmpRPM, Time.deltaTime +0.1f);
       /* speedRang = m_Car.car.transmission.getCurrentGearSpeedRang();
        if (speedRang == 0)
        {
            currentRPM = minRPM;
            return;
        }
        currentRPM = maxRPM * m_Car.speedInKmH / speedRang;
        */
        if (currentRPM < minRPM)
            currentRPM = minRPM;
        if (currentRPM > maxRPM)
            currentRPM = maxRPM;
    }


    public float getEngineTorque()
    {
        if (m_Car.car.hp > 0)
            return getEngineTorque(currentRPM);
        return m_Car.car.RPM_Curve.Evaluate(currentRPM);
    }

    public float getEngineTorque(float rpm)
    {
        if (rpm > maxRPM)
            return 0;
        return m_Car.car.hp * 5252 / rpm;
    }
    float avilableTorque;
    public float wheelsTorque;
    public void updateWheelsTorque()
    {
        avilableTorque= wheelsTorque = getEngineTorque() *
           m_Car.car.transmission.getCurrentGearRatio() *
           m_Car.car.transmission.differentialRatio;
         
        
    }

    public float getAvilableTorque(float percent)
    {
        float desiredTorque = wheelsTorque * percent;
        if (desiredTorque > avilableTorque)
        {
            desiredTorque = avilableTorque;
        }
        avilableTorque -= desiredTorque;
        return desiredTorque;
    }

    public float getEngineBrakeTorque()
    {
        return Mathf.Lerp(0,maxEngineBrake,currentRPM/maxRPM);
    }

    public float getEngineWheelsBrakeTorque()
    {
        return getEngineBrakeTorque() *
           m_Car.car.transmission.getCurrentGearRatio() *
           m_Car.car.transmission.differentialRatio/ m_Car.car.motorWheelsCount;
    }

    /* public float desiredTorquePercentToTake(float desiredValue)
     {

         avilableTorquePercent -= desiredValue;
         if (avilableTorquePercent < 0)
         {
             desiredValue += avilableTorquePercent;
             avilableTorquePercent = 0;
         }
         return desiredValue;
     }
     */
}