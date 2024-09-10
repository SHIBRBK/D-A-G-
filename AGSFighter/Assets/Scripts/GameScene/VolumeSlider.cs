using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    private void Start()
    {
        // SoundManagerから現在の音量を取得してスライダーの初期値を設定
        float bgmVolume = SoundManager.Instance.bgmSource.volume;
        bgmSlider.value = bgmVolume;
        SoundManager.Instance.SetBGMVolume(bgmVolume);

        float sfxVolume = SoundManager.Instance.sfxSource.volume;
        sfxSlider.value = sfxVolume;
        SoundManager.Instance.SetSFXVolume(sfxVolume);

        float uiVolume = SoundManager.Instance.uiSource.volume;
        uiSlider.value = uiVolume;
        SoundManager.Instance.SetUIVolume(uiVolume);

        // スライダーの値が変更されたときに呼び出されるイベントを設定
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        uiSlider.onValueChanged.AddListener(OnUIVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value); // 音量を保存
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value); // 音量を保存
    }

    private void OnUIVolumeChanged(float value)
    {
        SoundManager.Instance.SetUIVolume(value);
        PlayerPrefs.SetFloat("UIVolume", value); // 音量を保存
    }
}
