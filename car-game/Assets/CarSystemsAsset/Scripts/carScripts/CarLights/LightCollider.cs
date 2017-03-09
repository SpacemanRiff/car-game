using UnityEngine;
using System.Collections;

public class LightCollider : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        CarControl m_car = collider.transform.root.GetComponentInChildren<CarControl>();
        if (m_car != null)
        {
            m_car.frontLightsOn();
        }
    }
    void OnTriggerExit(Collider collider)
    {
        CarControl m_car = collider.transform.root.GetComponentInChildren<CarControl>();
        if (m_car != null)
        {
            m_car.frontLightsOff();
        }
    }
}
