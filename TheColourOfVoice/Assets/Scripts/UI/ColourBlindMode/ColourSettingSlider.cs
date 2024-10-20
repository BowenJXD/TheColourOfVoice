using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourSettingSlider : MonoBehaviour
{
    public LinearColour Colour;
    public ColourSettings ColourSettings;
    private Slider Slider;
    
    // Start is called before the first frame update
    void Start()
    {
        Slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColour()
    {
        ColourSettings.AlterKeyframe(Colour, Slider.value);
    }
}
