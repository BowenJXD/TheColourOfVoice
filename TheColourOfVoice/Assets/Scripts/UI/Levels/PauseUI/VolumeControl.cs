using UnityEngine;
using UnityEngine.UI; // 需要使用UI系统
using FMODUnity;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private string busPath = "bus:/";  // 总线路径
    private FMOD.Studio.Bus masterBus;
    private Slider volumeSlider;

    void Start()
    {
        // 获取总线
        masterBus = RuntimeManager.GetBus(busPath);

        // 获取 Slider 组件
        volumeSlider = GetComponent<Slider>();
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            // 初始化滑块值为当前音量
            float currentVolume;
            masterBus.getVolume(out currentVolume);
            volumeSlider.value = currentVolume;
        }
    }

    // 调整音量的方法
    public void SetVolume(float volume)
    {
        masterBus.setVolume(volume);
    }
}
