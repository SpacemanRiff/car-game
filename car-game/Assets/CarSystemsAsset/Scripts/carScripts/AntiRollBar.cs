using UnityEngine;
using System.Collections;

public class AntiRollBar : MonoBehaviour {

    public WheelCollider WheelL;
    public WheelCollider WheelR;
    public float AntiRoll = 5000.0f;
    Rigidbody m_rigidbody;
    void Start () {
        m_rigidbody = GetComponent<Rigidbody>();

    }
    WheelHit hit;
    float travelL = 1.0f;
    float travelR = 1.0f;
    bool groundedL;
    bool groundedR;
    float antiRollForce;
    void FixedUpdate()
    {
        

         groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) 
                / WheelL.suspensionDistance;

       groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

         antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            m_rigidbody.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                   WheelL.transform.position);
        if (groundedR)
            m_rigidbody.AddForceAtPosition(WheelR.transform.up * antiRollForce,
                   WheelR.transform.position);
    }

}
