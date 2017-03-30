using UnityEngine;
using System.Collections;
using UnityEditor;

public class ButtonController : MonoBehaviour {

    public enum ButtonType {START, CARSELECT, QUIT, CARTYPE}
    public enum CarType {CAR, VAN}
    public ButtonType type;
    public CarType car = CarType.CAR;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public ButtonType GetButtonType()
    {
        return type;
    }

    public CarType GetCarType()
    {
        return car;
    }
}

[CustomEditor(typeof(ButtonController))]
public class ButtonControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as ButtonController;
        script.type = (ButtonController.ButtonType)EditorGUILayout.EnumPopup(script.type);

        if (script.type == ButtonController.ButtonType.CARTYPE)
        {
            script.car = (ButtonController.CarType)EditorGUILayout.EnumPopup(script.car);
        }
    }
}