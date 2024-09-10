using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ゲーム全体のサウンド管理を行うクラス。SFX、UI、BGMなどのオーディオを管理。
/// </summary>
public class SoundManager : MonoBehaviour
{
    // インスタンスへの参照（シングルトンパターン）
    public static SoundManager Instance;

    // 各オーディオソースとオーディオクリップの配列
    public AudioSource sfxSource;  // 効果音（SFX）用のAudioSource
    public AudioSource uiSource;   // UI用のAudioSource
    public AudioSource bgmSource;  // BGM用のAudioSource

    public AudioClip[] sfxSounds;   // 効果音クリップの配列
    public AudioClip[] missClips;  // 空振り用の効果音クリップの配列
    public AudioClip[] uiSounds;    // UI効果音クリップの配列
    public AudioClip[] bgmSounds;   // BGMクリップの配列

    // 初期音量の設定
    public float initBGMVolume; // 初期BGM音量
    public float initSFXVolume; // 初期SFX音量
    public float initUIVolume;  // 初期UI音量

    // 音声クリップの辞書（名前で検索できるように）
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> uiDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> missDict = new Dictionary<string, AudioClip>(); // 空振り用の辞書

    /// <summary>
    /// シングルトンパターンのインスタンス初期化と辞書の設定。
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン間でオブジェクトを破棄しない
            InitializeAudioDictionaries();
            SetInitialVolumes(); // 初期音量を設定
        }
        else
        {
            Destroy(gameObject); // 既にインスタンスが存在する場合、重複を避けるために破棄
        }
    }

    /// <summary>
    /// 各種オーディオクリップを辞書に登録。
    /// </summary>
    private void InitializeAudioDictionaries()
    {
        foreach (var sound in sfxSounds)
        {
            sfxDict[sound.name] = sound;
        }

        foreach (var sound in uiSounds)
        {
            uiDict[sound.name] = sound;
        }

        foreach (var sound in bgmSounds)
        {
            bgmDict[sound.name] = sound;
        }

        foreach (var clip in missClips)
        {
            missDict[clip.name] = clip;
        }
    }

    /// <summary>
    /// 効果音を再生。
    /// </summary>
    /// <param name="clipName">再生するクリップの名前</param>
    public void PlaySFX(string clipName)
    {
        if (sfxDict.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip); // 指定された効果音を一回だけ再生
        }
        else
        {
            Debug.LogWarning("SFX clip not found: " + clipName); // クリップが見つからない場合は警告を出力
        }
    }

    /// <summary>
    /// UI効果音を再生。
    /// </summary>
    /// <param name="clipName">再生するクリップの名前</param>
    public void PlayUIClip(string clipName)
    {
        if (uiDict.TryGetValue(clipName, out var clip))
        {
            uiSource.PlayOneShot(clip); // 指定されたUI効果音を一回だけ再生
        }
        else
        {
            Debug.LogWarning("UI clip not found: " + clipName); // クリップが見つからない場合は警告を出力
        }
    }

    /// <summary>
    /// BGMを再生。
    /// </summary>
    /// <param name="clipName">再生するクリップの名前</param>
    /// <param name="loop">ループ再生するかどうか（デフォルトはtrue）</param>
    public void PlayBGM(string clipName, bool loop = true)
    {
        if (bgmDict.TryGetValue(clipName, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play(); // 指定されたBGMを再生
        }
        else
        {
            Debug.LogWarning("BGM clip not found: " + clipName); // クリップが見つからない場合は警告を出力
        }
    }

    /// <summary>
    /// 空振り音を再生。
    /// </summary>
    /// <param name="clipName">再生するクリップの名前</param>
    public void PlayMissSound(string clipName)
    {
        if (missDict.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip); // 指定された空振り音を一回だけ再生
        }
        else
        {
            Debug.LogWarning("Miss sound clip not found: " + clipName); // クリップが見つからない場合は警告を出力
        }
    }

    /// <summary>
    /// 各オーディオソースの初期音量を設定。
    /// </summary>
    private void SetInitialVolumes()
    {
        bgmSource.volume = initBGMVolume;
        sfxSource.volume = initSFXVolume;
        uiSource.volume = initUIVolume;
    }

    /// <summary>
    /// BGMの音量を設定。
    /// </summary>
    /// <param name="value">設定する音量</param>
    public void SetBGMVolume(float value)
    {
        bgmSource.volume = value;
    }

    /// <summary>
    /// SFXの音量を設定。
    /// </summary>
    /// <param name="value">設定する音量</param>
    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }

    /// <summary>
    /// UI効果音の音量を設定。
    /// </summary>
    /// <param name="value">設定する音量</param>
    public void SetUIVolume(float value)
    {
        uiSource.volume = value;
    }

    /// <summary>
    /// 再生中のBGMを停止。
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
