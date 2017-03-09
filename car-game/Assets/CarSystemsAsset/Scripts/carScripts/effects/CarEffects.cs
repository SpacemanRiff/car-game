using System.Collections;
using UnityEngine;




public class CarEffects : MonoBehaviour
{
    public Car car;

    CarAudio carAudio;
    private void Start()
    {
        car = GetComponent<Car>();
        carAudio = GetComponent<CarAudio>();
    }


    void Update()
    {
        CheckForWheelSpin();

    }


    void stopSoundEffects()
    {
        foreach (Wheel wheel in car.wheels)
        {
            wheelEffects = wheel.wheelCollider.GetComponent<WheelEffects>();
            if (wheelEffects != null)
                wheelEffects.StopAudio();
        }
    }



    // checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
   // WheelHit wheelHit;
    Wheel wheel;
    WheelEffects wheelEffects;
    private void CheckForWheelSpin()
    {
        for (int i = 0; i < 4; i++)
        {
            wheel = car.wheels[i];

            wheelEffects = wheel.wheelCollider.GetComponent<WheelEffects>();
            if (wheelEffects == null  )
                continue;

            // is the tire slipping above the given threshhold
            if (wheel.wheelCollider.isGrounded &&
                Mathf.Abs(wheel.slipRatio) > wheel.wheelCollider.forwardFriction.extremumSlip + 0.3f
                && Mathf.Abs(wheel.speedInKmH - wheel.m_Car.speedInKmH) >1
                 //  ||
                 //Mathf.Abs(wheel.wheelCollider.GetGroundHit.sidewaysSlip) > car.m_SlipLimit + 0.1f
                 )
            {
                wheelEffects.EmitTyreSmoke();

                // avoiding all four tires screeching at the same time
                // if they do it can lead to some strange audio artefacts
                if (!AnySkidSoundPlaying())
                {
                    if (carAudio.camDist < carAudio.maxRolloffDistance * carAudio.maxRolloffDistance)
                        wheelEffects.PlayAudio();
                }
               
            }
            else
            {
                stopEffect(wheelEffects);
            }
        }
    }

    void stopEffect(WheelEffects wheelEffects)
    {
        if (wheelEffects.PlayingAudio)
        {
            wheelEffects.StopAudio();
        }
        // end the trail generation
        wheelEffects.EndSkidTrail();
    }
    private bool AnySkidSoundPlaying()
    {

        for (int i = 0; i < 4; i++)
        {
            wheel = car.wheels[i];
            wheelEffects = wheel.wheelCollider.GetComponent<WheelEffects>();
            if (wheelEffects == null)
                continue;
            if (wheelEffects.PlayingAudio)
            {
                return true;
            }
        }
        return false;
    }
}

