using System;
using UnityEngine;


public class BrakeLight : CarLight
{
   

    void Start()
    {
        lightOff();
        m_car.brakingListner += brakingListner;
        m_car.forwardListner += offListner;
        m_car.backwardListner += offListner;
        m_car.neutralListner += offListner;
        m_car.stoppedListner += offListner;
    }

    void brakingListner()
    {
        lightOn();
    }
    void offListner()
    {
        lightOff();
    }
}

