using UnityEngine;
using System.Collections;

abstract public class DriftSystem : MonoBehaviour
{

    public Car car;
    public CarControl m_car;
    public bool isOn;
    public TCSSystem tcs;
    protected float tcsValue = -1;

    public void OnEnabled()
    {
        car = GetComponent<Car>();
        m_car = GetComponent<CarControl>();
      //  m_car.driftListner += driftHandler;
      //  tcs = car.accessories.tcsSystem;
       // if (tcs != null)
        //    tcsValue = tcs.m_TractionControl;
    }
    public void driftHandler()
    {
        isOn = !isOn;
       
        if (isOn)
        {
           // deactivateTCS();
           // activateDriftSystem();
        }
        else
        {
           // activateTCS();
            deactivateDriftSystem();
        }
    }

   

    public abstract void activateDriftSystem();

    public abstract void deactivateDriftSystem();

    public void setWheelsSlip(Wheel[] wheels, float forwardStiffness, float sideStiffness)
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            setSlip(wheels[i], forwardStiffness, sideStiffness);
        }
    }

    public WheelFrictionCurve wheelFrictionCurve;
    public void setSlip(Wheel wheel, float forwardStiffness, float sideStiffness)
    {

        wheelFrictionCurve = wheel.wheelCollider.sidewaysFriction;

        wheelFrictionCurve.stiffness = sideStiffness;
        wheel.wheelCollider.sidewaysFriction = wheelFrictionCurve;

        wheelFrictionCurve = wheel.wheelCollider.forwardFriction;
        wheelFrictionCurve.stiffness = forwardStiffness;
        wheel.wheelCollider.forwardFriction = wheelFrictionCurve;

    }
}
