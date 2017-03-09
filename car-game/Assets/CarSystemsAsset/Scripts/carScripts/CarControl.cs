
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CarStause
{
    Forward,
    Stopped,
    Backward,
    Braking,
    Neutral
}
public delegate void CarStatusChangeListner();
/*public delegate void OnBackwardListner();
public delegate void OnForwardListner();
public delegate void OnBrakingListner();
public delegate void OnStoppedListner();
public delegate void OnNeutralListner();*/
public delegate void OnFrontLightsTrigger(bool isOn);
//public delegate void DriftActivitionListner(bool isOn);



public class CarControl : MonoBehaviour
{
    public event CarStatusChangeListner backwardListner;
    public event CarStatusChangeListner forwardListner;
    public event CarStatusChangeListner brakingListner;
    public event CarStatusChangeListner stoppedListner;
    public event CarStatusChangeListner neutralListner;

    // public event CarStatusChangeListner toggleTCSListner;

    public event OnFrontLightsTrigger frontLightsTrigger;

    public Car car;

    public Rigidbody m_Rigidbody;
   

    public float BrakeInput;//{ get; private set; }
    public float CurrentSteerAngle;// { get { return m_SteerAngle; } }

    public Transform startPoint;

    public CarStause statuse = CarStause.Stopped;

    public float AccelInput;// { get; private set; }
    public float old_speed=0;

    // void Awake()
    void OnEnable()
    {

        car = GetComponent<Car>();

        m_Rigidbody = GetComponent<Rigidbody>();
        car.engine.m_Car = GetComponent<CarControl>();
        
        car.handBrake.m_Car = GetComponent<CarControl>();
        enableLights();
        //car.accessories.OnEnabled();
    }

    void enableLights()
    {
        FrontLight[] frontLights = GetComponentsInChildren<FrontLight>();
        BrakeLight[] brakeLights = GetComponentsInChildren<BrakeLight>();
        RevLights[] revLights = GetComponentsInChildren<RevLights>();
        loopLights(frontLights);
        loopLights(brakeLights);
        loopLights(revLights);
    }
    void loopLights(CarLight[] lights)
    {
        foreach (CarLight carLight in lights)
            carLight.enabled = true;
    }

    void Start()
    {
        
       // optimizeCM();
        foreach (Wheel wheel in car.wheels)
        {
            wheel.m_Car = GetComponent<CarControl>();
            wheel.brake.m_Car= GetComponent<CarControl>();
        }
        car.transmission.onStart(GetComponent<CarControl>());
        car.accessories.enableAtStart();

    }
    void optimizeCM()
    {
        Transform WheelColiders = car.transform.FindChild("WheelColiders");
        Vector3 pos = WheelColiders.localPosition;
        pos.y += car.centerOfMass.y;
        pos.z += car.centerOfMass.z;
        m_Rigidbody.centerOfMass = pos;

    }
    public void frontLightsOn()
    {
        frontLightsTrigger(true);
    }
    public void frontLightsOff()
    {
        frontLightsTrigger(false);
    }
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            car.wheels[i].applyLocalPositionToVisuals();
        }
    }


    void updateWheels()
    {
        for (int i = 0; i < 4; i++)
        {
            car.wheels[i].update();
        }
    }
    public float speedInKmH=0;

    public void updateSpeedInKmH()
    {
       // speedInKmH = getAvregWheelsSpeed();
        speedInKmH =Mathf.Abs( getCarSpeedVector3().z) * 3.6f;
		if (speedInKmH < 0.0001f)
			speedInKmH = 0;
    }


    public Vector3 getCarSpeedVector3()
    {
        return transform.InverseTransformDirection(m_Rigidbody.velocity);
    }

  

   
    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
       
        AccelInput = accel;
        BrakeInput = footbrake;

        //if (statuse != CarStause.Braking )
           // releaseBrake();
        updateSpeedInKmH();
       
            updateWheels();
        car.transmission.updateCurrentGear();
        car.engine.updateCurrentRPM();
       car.steering.calcSteerAngle(steering);
        car.engine.updateWheelsTorque();
        //getAvregWheelsRPM();
        ApplyDrive();

        CapSpeed();

        
        if (handbrake > 0)
            car.handBrake.doHandBrake();
        //else if(statuse!=CarStause.Braking)
         //   car.handBrake.releaseHandBrake();

    }
    float engineBrake;
    public void dragToStop()
    {
       // m_Rigidbody.drag = car.drag + 0.1f;
        setCarStatus(CarStause.Neutral);
        engineBrake = Mathf.Abs(car.engine.getEngineWheelsBrakeTorque());
        for (int i = 0; i < car.motorWheelsCount; i++)
        {
            car.motorWheels[i].wheelCollider.brakeTorque=engineBrake;
            car.wheels[i].wheelCollider.motorTorque=0;
        }
    }

   

    private void CapSpeed()
    {
        if (speedInKmH> car.MaxSpeed)
            m_Rigidbody.velocity = (int)(car.MaxSpeed / 3.6f) * m_Rigidbody.velocity.normalized;

        if (car.transmission.currentGear == -1)
        {
            float reverseSpeed = (int)car.transmission.getCurrentGearSpeedRang(); //speedRange[engine.transmission.currentGear];
            if (speedInKmH > reverseSpeed)
                m_Rigidbody.velocity = (int)(reverseSpeed / 3.6f) * m_Rigidbody.velocity.normalized;
        }
    }


    public float getSpeedFactor()
    {
        return speedInKmH / car.MaxSpeed;
    }

    private void ApplyDrive()
    {
        m_Rigidbody.drag = car.drag;
        if (AccelInput > 0 && car.transmission.currentGear > -1 && statuse!=CarStause.Braking)
            goForward();
        else if (AccelInput < 0 && car.transmission.currentGear < 1 && statuse != CarStause.Braking)
            goRevears();
        else if (BrakeInput != 0)
            doBrake();

        else
        {
            dragToStop();
        }
        if (speedInKmH < 0.1f && (statuse == CarStause.Neutral || statuse == CarStause.Braking))
            setCarStatus(CarStause.Stopped);

    }

    public void setCarStatus(CarStause c_status)
    {
        statuse = c_status;
        switch (statuse)
        {
            case CarStause.Backward:
                backwardListner();
                break;
            case CarStause.Forward:
                forwardListner();
                break;
            case CarStause.Braking:
                brakingListner();
                break;
            case CarStause.Neutral:
                neutralListner();
                break;
            case CarStause.Stopped:
                stoppedListner();
                break;
        }
    }
    public void goRevears()
    {
        setCarStatus(CarStause.Backward);
        releaseBrake();
        giveBackTorque();
        directWheelsTorque();
    }

    public void goForward()
    {
        setCarStatus(CarStause.Forward);
        releaseBrake();
        sendTorque();
    }


    public void sendTorque()
    {
        if (car.accessories.tcsSystem != null && car.accessories.tcsSystem.m_TractionControl > 0)
            car.accessories.tcsSystem.wheelsTractionSystem();
        else
            directWheelsTorque();
    }

    public void directWheelsTorque()
    {
        for (int i = 0; i < car.motorWheelsCount; i++)
        {
            car.motorWheels[i].applyMotorTorque();
        }
    }

    public void doBrake()
    {
        for (int i = 0; i < 4; i++)
        {
            car.wheels[i].brake.doBrake();
        }
        setCarStatus(CarStause.Braking);
    }

    public void releaseBrake()
    {
        for (int i = 0; i < 4; i++)
        {
            car.wheels[i].brake.releaseBrake();
        }
    }

    public void giveBackTorque()
    {
        for (int i = 0; i < 4; i++)
        {
            car.wheels[i].currentengineTorquePercen = car.wheels[i].engineTorquePercent;
        }
    }
    public void nitro(bool active)
    {
        car.accessories.nitro.isActive = active;
    }
    public void toggleDriftSystem()
    {
        if (car.accessories.tcsSystem != null)
        {
            if (car.accessories.tcsSystem.m_TractionControl > 0)
                car.accessories.tcsSystem.deactivateTCS();
            else
                car.accessories.tcsSystem.activateTCS();
        }
      
    }
    /// <summary>
    /// not used methodes
    /// </summary>
    /// 

    float speedsSum;
    public float getAvregWheelsSpeed()
    {
        speedsSum = 0;
       
        for (int i = 0; i < car.motorWheelsCount; i++)
        {
            speedsSum += car.motorWheels[i].speedInKmH;

        }
      
        return speedsSum / car.motorWheelsCount;
    }
    public float rpmSum = 0;
 
    public float getAvregWheelsRPM()
     {

        for (int i = 0; i < car.motorWheelsCount; i++)
        {
            rpmSum += Mathf.Abs(car.motorWheels[i].wheelCollider.rpm);
            // count++;
         }
         return rpmSum / car.motorWheelsCount;
     }


}


