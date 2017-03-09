using UnityEngine;
using System.Collections;

[System.Serializable]
public class CarLight : MonoBehaviour
{
    
    public Light _light;
    protected CarControl m_car;

    void Awake()
    {
        _light = GetComponent<Light>();
        m_car = GetComponentInParent<CarControl>();
    }
    protected void lightOn()
    {
        _light.enabled = true;
    }
    protected void lightOff()
    {
        _light.enabled = false;
    }
}
