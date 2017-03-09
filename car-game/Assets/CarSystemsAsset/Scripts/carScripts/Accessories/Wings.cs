using UnityEngine;
using System.Collections;

public class Wings : MonoBehaviour
{

    public float frontDownforce = 500f;
    public float backDownforce = 700f;

    CarControl m_Car;

    void Start()
    {
        m_Car = GetComponent<CarControl>();
    }
    void FixedUpdate()
    {
        AddDownForce();
    }
    Vector3 frontPosition;
    Vector3 backPosition;
    public void AddDownForce()
    {
        frontPosition = m_Car.car.frontWheels[0].wheelCollider.transform.position;
        frontPosition.x = m_Car.m_Rigidbody.position.x;
        backPosition = m_Car.car.backWheels[0].wheelCollider.transform.position;
        backPosition.x = m_Car.m_Rigidbody.position.x;
        m_Car.m_Rigidbody.AddForceAtPosition(-transform.up * frontDownforce *
                                                m_Car.getSpeedFactor()
                                                 , frontPosition);
        m_Car.m_Rigidbody.AddForceAtPosition(-transform.up * backDownforce *
                                                 m_Car.getSpeedFactor()
                                                , backPosition);
    }
}
