using UnityEngine;


public class UserInput : MonoBehaviour
{
    private CarControl m_Car; 

    float h;
    float accel;
    float brake;
    float handbrake;
    float revears;
    bool toggleDriftSystem;


   const string Horizontal_KEY = "Horizontal";
    const string Vertical_KEY = "Drive";
    const string NITRO_KEY = "Boost";
    const string HANDBRAKE_KEY = "Brake";
    //const string DRIFT_SYSTEM_KEY = KeyCode.A;
    bool activeNitro;

    
    void Start()
    {
    
        m_Car = GetComponent<CarControl>();
    }

    void Update()
    {
        toggleDriftSystem = Input.GetKeyUp(KeyCode.A);
        if (toggleDriftSystem)
            m_Car.toggleDriftSystem();
    }

 

    void FixedUpdate()
    {

       
         h =  Input.GetAxis(Horizontal_KEY) ;
      
        accel = brake = Input.GetAxis(Vertical_KEY) ;
        handbrake = Input.GetAxis(HANDBRAKE_KEY);

        m_Car.Move(h, accel, brake, handbrake); // pass the input to the car!
        activeNitro = Input.GetButton(NITRO_KEY);
        m_Car.nitro(activeNitro);

       
    }
   
}

