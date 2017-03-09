using UnityEngine;
using System.Collections;

using System.Collections.Generic;

[System.Serializable]
public class Transmission 
{

    // public List<float> gearRatios = new List<float>();
    //  public List<int> speedRange = new List<int>();
    public float differentialRatio = 1f;

    public int currentGear = 0;
    float blockTimer = 0;
    public CarControl m_Car;
    float wheelRadius = 0;

    public float shiftSpeed = 0.05f;

    public string getCurrentGearLabel()
    {
        if (currentGear == -1)
            return "R";
        if (currentGear == 0)
            return "N";
        return currentGear + string.Empty;
    }

    public void onStart(CarControl m_Car)
    {
        this.m_Car = m_Car;
      //  if (wheelRadius == 0)
           
        m_Car.brakingListner += neutralListner;
        m_Car.forwardListner += setGearToFrist;
        m_Car.backwardListner += setGearToRevers;
        m_Car.neutralListner += neutralListner;
        m_Car.stoppedListner += setGearToN;
        wheelRadius = m_Car.car.motorWheels[0].wheelCollider.radius;
    }
    public void startTimer()
    {
        blockTimer += Time.deltaTime;
        if (blockTimer > 0.25f)
            blockTimer = 0;
    }

    public void setCurrentGear(int i)
    {
        if (i == currentGear)
            return;
         startTimer();
         if(blockTimer == 0)
            m_Car.StartCoroutine(startShifting(i));
        //currentGear = i;
    }
    public IEnumerator startShifting(int i)
    {
        currentGear = 0;
        yield return new WaitForSeconds(shiftSpeed);
        currentGear = i;
       // startTimer();
    }
    public void neutralListner()
    {
      //  setCurrentGear(0);
    }
    public void setGearToRevers()
    {
        if (currentGear == 0)
            setCurrentGear(-1);
    }
    public void setGearToN()
    {
        setCurrentGear(0);
    }


    public void setGearToFrist()
    {
        if (currentGear == 0)
            setCurrentGear(1);
    }

    public int getCurrentGearSpeedRang()
    {
        if (currentGear == 0)
            return 0;

        return gearTopSpeed(getCurrentGearRatio());
    }
    float mSpeed;
    float PropShaftspeed;
    float WheelSpeed;
    public int gearTopSpeed(float gearRatio)
    {
        PropShaftspeed = m_Car.car.engine.maxRPM / gearRatio;
        WheelSpeed = PropShaftspeed / differentialRatio;
        mSpeed = WheelSpeed * Mathf.PI*2 * wheelRadius * 60 / 1000;

        return (int)Mathf.Abs(mSpeed);
    }
 
    public void updateCurrentGear()
    {
         if (currentGear < 1)
             return;
             
        int i = 1;
         if (m_Car.getCarSpeedVector3().z < 0)
         {
             currentGear = 1;
             return;
         }
        while (gearTopSpeed(getGearRatio(i)) <= m_Car.speedInKmH &&
            i < m_Car.car.GearRation_Curve.keys.Length - 3)
        {
            i++;
        }
        if (currentGear > i
            && m_Car.speedInKmH > gearTopSpeed(getGearRatio(i)) - 10)
            return;
        //currentGear = i;
        setCurrentGear(i);
    }
  
  

    public float getCurrentGearRatio()
    {
        return getGearRatio(currentGear);
    }

    public float getGearRatio(int gearIndex)
    {
        return m_Car.car.GearRation_Curve.Evaluate(gearIndex);
    }

}
