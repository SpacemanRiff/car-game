using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{


    public Transform car;
    public float distance = 12.5f;
    public float height = 3.7f;

    public float rotaionDamping = 0.2f;
    public float heightDamping = 0.4f;
    public float zoomRation = 0.35f;
    public float zRotaionDamping = 3f;
    float oldZ = 0;
    Vector3 rotaionVector;
    CarControl m_Car;

    Camera _camera;

    public float defaultFOV = 50f;

    void OnEnable()
    {

    }

    void Start()
    {
        car = transform.root.GetComponentInChildren<Car>().transform;
        oldZ = car.rotation.eulerAngles.y;
        m_Car = car.GetComponent<CarControl>();
        target = car.FindChild("camToLookAt");
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
       
       // inRaceMove();
    }

    Quaternion currentRotaion;
    Transform target;
    float wantedAngle;
    float wantedHeight;
    float wantedX;

    float wantedZAngle;

    float myAngle;
    float myHeight;
    float myX;
    float myZAngle;

    void inRaceMove()
    {
        wantedAngle = rotaionVector.y;
        wantedHeight = car.position.y + height;
        wantedX = car.position.x;

        wantedZAngle = rotaionVector.z;

        myAngle = transform.eulerAngles.y;
        myHeight = transform.position.y;
        myX = transform.position.x;
        myZAngle = oldZ;

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotaionDamping * Time.deltaTime);
        myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping * Time.deltaTime);
        myX = Mathf.Lerp(myX, wantedX, heightDamping * Time.deltaTime);
        myZAngle = Mathf.LerpAngle(myZAngle, wantedZAngle, zRotaionDamping * Time.deltaTime);

        currentRotaion = Quaternion.Euler(0, myAngle, 0);
        transform.position = car.position;
        transform.position -= currentRotaion * Vector3.forward * distance;
        transform.position += height * Vector3.up;

        transform.position.Set(myX, myHeight, transform.position.z);


        transform.LookAt(target);
        transform.Rotate(0, 0, myZAngle - oldZ * distance);
        oldZ = myZAngle;
    }


   // Vector3 locationVector;
    float acc;
    void FixedUpdate()
    {
      //  locationVector = car.InverseTransformDirection(m_Car.m_Rigidbody.velocity);

       // if (locationVector.z < -1 && m_Car.AccelInput < 0)
        //    rotaionVector.y = car.eulerAngles.y + 180;
       // else if (m_Car.AccelInput >= 0 && locationVector.z > -1)
            rotaionVector.y = car.eulerAngles.y;
        calcZRotation();

         acc = m_Car.m_Rigidbody.velocity.magnitude;
        _camera.fieldOfView = defaultFOV + acc * zoomRation;
        inRaceMove();
    }

    void calcZRotation()
    {
        rotaionVector.z = m_Car.m_Rigidbody.angularVelocity.y / 2;
    }

}
