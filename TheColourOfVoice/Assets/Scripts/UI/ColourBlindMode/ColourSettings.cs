using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColourSettings : MonoBehaviour
{
    private Keyframe[] Keys = new Keyframe[7];
    private int Segment;
    private ColorCurves ColorCurves;
    private float InitValue = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        InitValue = 0.5f;
        Segment = Keys.Length - 1;
        Volume VolumeComp = GetComponent<Volume>();
        VolumeProfile Profile = VolumeComp.sharedProfile;
        Profile.TryGet<ColorCurves>(out ColorCurves);

        for (int i = 0; i < Keys.Length; i++)
        {
            Keys[i] = new Keyframe((float)i / Segment, InitValue);
        }
        ColorCurves.hueVsHue.value = new TextureCurve(Keys, 0, true, Vector2.one);
        DontDestroyOnLoad(gameObject);
    }

    public void AlterKeyframe(LinearColour Colour, float Value)
    {
        int Key = (int)Colour;
        float KeyValueAlter = -(float)(Key - 3) / 6;
        if (KeyValueAlter < 0) KeyValueAlter += 1;
        float AlteredValue = (Value + KeyValueAlter) % 1;
        Keys[Key] = new Keyframe((float)Key / Segment, AlteredValue);
        
        if (Colour == LinearColour.RED)
        {
            Keys[Segment] = new Keyframe(1, AlteredValue);
        }
        
        ColorCurves.hueVsHue.value = new TextureCurve(Keys, 0, true, Vector2.one);
    }
}