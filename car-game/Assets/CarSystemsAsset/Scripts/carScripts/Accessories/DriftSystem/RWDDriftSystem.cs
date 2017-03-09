using UnityEngine;
using System.Collections;
using System;

public class RWDDriftSystem : DriftSystem {


    public new void OnEnabled()
    {
        base.OnEnabled();
    }
   
    public override void activateDriftSystem()
    {
        setWheelsSlip(car.backWheels, car.forwardStifftness / 1.0116f, car.sideStifftness / 1.11f);
        setWheelsSlip(car.frontWheels, car.forwardStifftness/1.001f , car.sideStifftness/1.0001f);
    }


    public override void deactivateDriftSystem()
    {
        setWheelsSlip(car.backWheels, car.forwardStifftness, car.sideStifftness);
        setWheelsSlip(car.frontWheels, car.forwardStifftness, car.sideStifftness);
    }




}
