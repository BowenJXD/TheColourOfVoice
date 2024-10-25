using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ColorBlindPostProcessing : MonoBehaviour
{
    public Volume volume;
    public VolumeProfile normalProfile;
    public VolumeProfile protanopiaProfile;
    public VolumeProfile deuteranopiaProfile;
    public VolumeProfile tritanopiaProfile;
    public VolumeProfile monochromacyProfile;

    public void SetColorMode(int mode)
    {
        switch (mode)
        {
            case 0: // 正常模式
                volume.profile = normalProfile;
                break;
            case 1: // 红绿色盲（Protanopia）
                volume.profile = protanopiaProfile;
                break;
            case 2: // 蓝黄色盲（Tritanopia）
                volume.profile = tritanopiaProfile;
                break;
            case 3: // 全色盲
                volume.profile = monochromacyProfile;
                break;
        }
    }
}
