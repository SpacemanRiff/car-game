using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpeedoMeter : MonoBehaviour {
    public CarControl m_Car;

    [SerializeField]
    public Transform speedoMeter_texture;
    [SerializeField]
    public Transform speedoMeterNeedle_texture;

    [SerializeField]
    public Transform speedText;

    [SerializeField]
    public Transform gearText;

    public Slider nitroSlider;


    void OnEnable()
    {
        nitroSlider = GetComponentInChildren<Slider>();
    }

    void OnGUI()
    {
        speedText.GetComponent<Text>().text=(int)m_Car.speedInKmH + string.Empty;
        gearText.GetComponent<Text>().text = m_Car.car.transmission.getCurrentGearLabel() ;

        float z = - Mathf.Lerp(0,  170, m_Car.car.engine.currentRPM / m_Car.car.engine.maxRPM);

        Quaternion roteNeedle = Quaternion.Euler(0,0,z);

        speedoMeterNeedle_texture.GetComponent<Image>().rectTransform.rotation = roteNeedle;

        nitroSlider.value = m_Car.car.accessories.nitro.restTime / m_Car.car.accessories.nitro.maxDauration;
    }
  
}
