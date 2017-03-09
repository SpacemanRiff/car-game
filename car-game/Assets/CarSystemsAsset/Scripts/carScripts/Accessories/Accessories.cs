using UnityEngine;
using System.Collections;

[System.Serializable]
public class Accessories {

    // if the car has Antilock Brake System
    public bool hasABS = true;
   // public bool hasCountersteering = true;


    public TCSSystem tcsSystem; // Car's TCS

    public Nitro nitro;

    public Car car;

    public DriftSystem driftSystem;
    public EPSSystem epsSystem;
    public Wings wings;


    public void OnEnabled()
    {
        tcsSystem= car.GetComponent<TCSSystem>();
        nitro = car.GetComponentInChildren<Nitro>();
        epsSystem= car.GetComponent<EPSSystem>();
        wings = car.GetComponent<Wings>();
        // driftSystem= car.GetComponent<DriftSystem>();
        // setupDriftSystem();
    }
    
    public void enableAtStart()
    {
        enableEPS();
       
       // wings.enabled = true;
    }

    public void enableEPS()
    {
        if (epsSystem != null)
            epsSystem.enabled = true;
    }
    public void disableEPS()
    {
        if (epsSystem != null)
            epsSystem.enabled = false;
    }
    public void setupDriftSystem()
    {
        if (driftSystem != null)
            return;
      
            driftSystem=car.gameObject.AddComponent<RWDDriftSystem>();
        driftSystem.enabled = true;
        driftSystem.OnEnabled();
       // Debug.Log(car.carDriveType + " " + driftSystem.GetType());
    }
   


}
