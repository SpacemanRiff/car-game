using UnityEngine;
using System.Collections;

public class Nitro : MonoBehaviour
{


    public float maxDauration = 10f;
    public float addForce = 400f;
    public float minForce;
    public float restTime;
    public bool isActive;

    Rigidbody m_Rigidbody;
    ParticleSystem nitroParticleSystem;
    Vector3 forceVector = Vector3.zero;
    float carTopSpeed;
    void Start()
    {
        restTime = maxDauration;

        m_Rigidbody = GetComponentInParent<Car>().GetComponent<Rigidbody>();
        nitroParticleSystem = GetComponent<ParticleSystem>();
        minForce = addForce / 2;
        carTopSpeed = GetComponentInParent<Car>().MaxSpeed;
    }

    public void FixedUpdate()
    {
        if (isActive && restTime > 0)
        {
            applyNitro();
            GetComponentInParent<Car>().MaxSpeed = carTopSpeed + 60;
        }
        else
        {
            nitroParticleSystem.Stop();
            GetComponentInParent<Car>().MaxSpeed = carTopSpeed;
        }
    }

    public void applyNitro()
    {

        forceVector.z = addForce * restTime / maxDauration + minForce;
        m_Rigidbody.AddRelativeForce(forceVector, ForceMode.Impulse);
        nitroParticleSystem.Play();
        restTime -= Time.deltaTime;
    }

    public void reFill(float time)
    {
        restTime += time;
        if (restTime > maxDauration)
            restTime = maxDauration;
    }
    void LateUpdate()
    {
        if (!isActive)
            reFill(0.0005f);
    }

}
