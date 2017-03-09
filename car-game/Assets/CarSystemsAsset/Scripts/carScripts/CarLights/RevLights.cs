using UnityEngine;
using System.Collections;

public class RevLights : CarLight
{
    
    void Start()
    {
        lightOff();
        m_car.brakingListner += offListner;
        m_car.forwardListner += offListner;
        m_car.backwardListner += brakingListner;
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

