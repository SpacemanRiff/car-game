using UnityEngine;
public class TCSSystem : MonoBehaviour
{

    [Range(0f, 1)]
    [SerializeField]
    public float m_TractionControl = 1; // 0 is no traction control, 1 is full interference

    private Car car;


    void Start()
    {
        car = GetComponent<Car>();

    }

    public void wheelCurrentTorquePercen(Wheel wheel)
    {
        wheel.currentengineTorquePercen= wheel.engineTorquePercent+wheel.desiredSlip;
        if (wheel.currentengineTorquePercen > wheel.engineTorquePercent)
            wheel.currentengineTorquePercen = wheel.engineTorquePercent;

        if (wheel.currentengineTorquePercen != wheel.engineTorquePercent)
            wheel.setTCS_ON();
    }

    Wheel wheel;
    bool is_forward_slip;
    public void wheelsTractionSystem()
    {
        for (int i = 0; i < car.motorWheelsCount; i++)
        {
            wheel = car.motorWheels[i];
            wheelCurrentTorquePercen(wheel);
            if (wheel.currentengineTorquePercen <=0)
            {
                wheel.currentengineTorquePercen =  0.01f;
                if(wheel.desiredSlip<-0.9f)
                wheel.wheelCollider.brakeTorque = 1f*wheel.desiredSlip;
            }
            
        }
       if (car.accessories.epsSystem != null && car.accessories.epsSystem.isActiveAndEnabled)
            car.accessories.epsSystem.doUpdate();
        for (int i = 0; i < car.motorWheelsCount; i++)
        {
            car.motorWheels[i].applyMotorTorque();
			//Debug.Log (car.motorWheels[i].desiredSlip);
        }
        }
    float tcsValue = 0;
    public void deactivateTCS()
    {
        tcsValue = m_TractionControl;
        m_TractionControl = 0;
        car.GetComponent<CarControl>().giveBackTorque();
        car.accessories.disableEPS();
    }

    public void activateTCS()
    {
        m_TractionControl = tcsValue;
        car.accessories.enableEPS();
    }

}
