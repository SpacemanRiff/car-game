using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerCar : MonoBehaviour {
    [SerializeField]
    public Image tcs_off_texture;
    [SerializeField]
    public Image tcs_texture;
    [SerializeField]
    public Image abs_texture;


    Car car;

    public class BlinksWrapper
    {
        public bool isVisable;
        public bool isRunning;
        public bool stop;
        public float duration = 1f;
        public BlinksWrapper(bool isVisable) { this.isVisable = isVisable; }
    }
    public BlinksWrapper is_abs = new BlinksWrapper(false);
    public BlinksWrapper is_tcs = new BlinksWrapper(false);


    void setupPlayer()
    {
        
        CarControl carControl = car.GetComponent<CarControl>();
      
       // car.GetComponent<CarAudio>().enabled = true;
        //car.GetComponent<CarEffects>().enabled = true;
        car.GetComponent<TCSSystem>().enabled = true;
        
        car.gameObject.AddComponent<UserInput>();
       
        carControl.enabled = true;

        SpeedoMeter speedoMeter = GameObject.FindObjectOfType<SpeedoMeter>();
        if (speedoMeter != null)
        {
            speedoMeter.m_Car = carControl;
            speedoMeter.enabled = true;
        }
    }

    void Awake()
    {

        car = GetComponent<Car>();

        setupPlayer();

        //tcs_off_texture = GameObject.Find("tcsOff").GetComponent<Image>();
        /*tcs_off_texture.enabled = false;
        tcs_texture = GameObject.Find("tcs").GetComponent<Image>();
        tcs_texture.enabled = false;
        abs_texture = GameObject.Find("abs").GetComponent<Image>();
        abs_texture.enabled = false;*/
        

    }
    void OnGUI()
    {
        /*if (car.accessories.tcsSystem != null && car.accessories.tcsSystem.m_TractionControl == 0)
            tcs_off_texture.enabled = true;
        else
        {
            if (car.accessories.tcsSystem != null)
                tcs_off_texture.enabled = false;
            if (is_tcs.isVisable)
                tcs_texture.enabled = true;
            else tcs_texture.enabled = false;
        }

        if (is_abs.isVisable)
            abs_texture.enabled = true;
        else abs_texture.enabled = false;*/

    }
    public IEnumerator DoBlinks(BlinksWrapper is_called_befor, float blinkTime = 0.2f)
    {
        is_called_befor.isRunning = true;
        while (is_called_befor.duration > 0f && !is_called_befor.stop)
        {
            is_called_befor.duration -= blinkTime;
            //toggle renderer
            is_called_befor.isVisable = !is_called_befor.isVisable;
            //wait for a bit
            yield return new WaitForSeconds(blinkTime);
        }
        is_called_befor.isRunning = false;
        is_called_befor.isVisable = false;
        is_called_befor.stop = false;
        is_called_befor.duration = 1;
    }

    public void handleTCS(Wheel wheel)
    {
        if (!is_tcs.isRunning)
        {
            is_tcs.isVisable = true;
            StartCoroutine(DoBlinks(is_tcs));
        }
    }
    public void handleABS()
    {
        if (!is_abs.isRunning)
        {
            is_abs.isVisable = true;
            StartCoroutine(DoBlinks(is_abs));
        }
    }
    void Start()
    {
        foreach (Wheel wheel in car.wheels)
        {
            wheel.brake.onABS_Working += handleABS;
            wheel.handleTCS += handleTCS;
        }
    }
}
