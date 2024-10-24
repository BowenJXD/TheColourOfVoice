using UnityEngine;
using UnityEngine.Rendering;

public class ColorBlindSettings : MonoBehaviour
{
    public Volume volume;
    public VolumeProfile normalProfile;
    public VolumeProfile protanopiaProfile;
    public VolumeProfile deuteranopiaProfile;
    public VolumeProfile tritanopiaProfile;
    public VolumeProfile monochromacyProfile;

    public void SetColorBlindMode(int mode)
    {
        switch (mode)
        {
            case 0: // 正常模式
                volume.profile = normalProfile;
                break;
            case 1: // 红绿色盲（Protanopia）
                volume.profile = protanopiaProfile;
                break;
            case 2: // 红绿色盲（Deuteranopia）
                volume.profile = deuteranopiaProfile;
                break;
            case 3: // 蓝黄色盲（Tritanopia）
                volume.profile = tritanopiaProfile;
                break;
            case 4: // 全色盲
                volume.profile = monochromacyProfile;
                break;
        }
    }
}