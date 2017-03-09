using UnityEngine;
using System.Collections;

using System.Collections.Generic;

/*
public enum CarDriveType
{
    FWD,
    RWD,
    AWD
}
*/
[System.Serializable]
public class Car : MonoBehaviour
{
    [SerializeField]
   // private CarDriveType m_CarDriveType = CarDriveType.AWD;
   // public CarDriveType carDriveType { get { return m_CarDriveType; } }
    // public List<Wheel> wheels;
    public Wheel[] wheels = new Wheel[4];
    public float mass = 1700; // Mass of the car
    public float drag = 0.01f; // drag to apply to the car
    public CarEngine engine; // Cars Engine
    [SerializeField]
    public Transmission transmission = new Transmission();

    public float frontBrakeTorque = 2500; // Brake torque to apply for each front wheel when Braking
    public float backBrakeTorque = 2000; // Brake torque to apply for each back wheel when Braking

  
    public HandBraking handBrake = new HandBraking(); // Class to handle Hand Braking

    public float forwardStifftness = 1f;// Cars wheels forward stifftness
    public float sideStifftness = 1f;// Cars wheels sideward stifftness

    public Vector3 centerOfMass; //vector to optimize the ceneter of mass of the car so it didnt flip over easly


    public float MaxSpeed = 310;//car's Top speed//{ get { return m_Topspeed; } }

   

    public float hp = 0;//Car's horsepower
    public AnimationCurve RPM_Curve;//engine rmp->torque curve, if hp>0 this will be ignored
    public AnimationCurve GearRation_Curve;// cars transmission's gear's ratio

    public Accessories accessories = new Accessories();

    //private Wheel motorWheel;
    private Transform wheelColiders_trans;
    public Wheel[] motorWheels; // motor wheels will be saved in this array for faster access
    public int motorWheelsCount = 0;
    public Wheel[] frontWheels = new Wheel[2];
    public Wheel[] backWheels = new Wheel[2];
    public Wheel[] leftWheels = new Wheel[2];
    public Wheel[] rightWheels = new Wheel[2];

    public Steering steering;

    void Awake()
    {
        wheelColiders_trans = transform.FindChild("WheelColiders");
        steering = GetComponent<Steering>();
        //if no wheels added to the car, create RWD Drivetype wheels system.
        if (wheels == null || wheels.Length == 0)
        {
            wheels = new Wheel[4];
            wheels[0] = buildWheel("wheelFR", true, false);
            wheels[1] = buildWheel("wheelFL", true, false);
            wheels[2] = buildWheel("wheelRR", false, true);
            wheels[3] = buildWheel("wheelRL", false, true);
        }

        foreach (Wheel wheel in wheels)
        {
            wheel.car = this;
        }
        setupMotorWheels();// find and saves motor wheels
        setupFrontAndBackWheels();

    }

    void Start()
    {
        accessories.car = this;
        accessories.OnEnabled();
       // GetComponent<PlayerCar>().enabled = true;
       
    }
    void setupFrontAndBackWheels()
    {
        Wheel wheel;
        int tmp_front_i = 0;
        int tmp_back_i = 0;
        int tmp_left_i = 0;
        int tmp_right_i = 0;
        for (int i = 0; i < 4; i++)
        {
            wheel = wheels[i];
            if (wheel.isFront)
            {
                wheel.onStart(frontBrakeTorque);
                frontWheels[tmp_front_i++] = wheel;
            }
            else
            {
                wheel.onStart(backBrakeTorque);
                backWheels[tmp_back_i++] = wheel;
            }

            if (wheel.wheelCollider.transform.position.x < transform.position.x)
            {
                leftWheels[tmp_left_i++] = wheel;
            }
            else
            {
                rightWheels[tmp_right_i++] = wheel;
            }
        }
    }
    Wheel buildWheel(string wheelName, bool isFront, bool isMotor)
    {
        Wheel wheel = new Wheel();
        Transform wheelTrans = transform.FindChild(wheelName);//search for wheel mesh that has "wheelName" name
        WheelCollider wheelCollider = wheelColiders_trans.FindChild(wheelName).GetComponent<WheelCollider>();// find a wheel colider with the same name
        wheel.wheelCollider = wheelCollider;
        wheel.wheelTransform = wheelTrans;
        wheel.isFront = isFront;
        wheel.isMotor = isMotor;
        return wheel;

    }

    public void setupMotorWheels()
    {
        motorWheelsCount = 0;
        Wheel wheel;
        for (int i = 0; i < 4; i++)
        {
            wheel = wheels[i];
            if (wheel.isMotor)
                motorWheelsCount++;
        }
        motorWheels = new Wheel[motorWheelsCount];
        int tmp_i = 0;
        for (int i = 0; i < 4; i++)
        {
            wheel = wheels[i];
            if (wheel.isMotor)
                motorWheels[tmp_i++] = wheel;

        }
    }
}
