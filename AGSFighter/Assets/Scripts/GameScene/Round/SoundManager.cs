using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �Q�[���S�̂̃T�E���h�Ǘ����s���N���X�BSFX�AUI�ABGM�Ȃǂ̃I�[�f�B�I���Ǘ��B
/// </summary>
public class SoundManager : MonoBehaviour
{
    // �C���X�^���X�ւ̎Q�Ɓi�V���O���g���p�^�[���j
    public static SoundManager Instance;

    // �e�I�[�f�B�I�\�[�X�ƃI�[�f�B�I�N���b�v�̔z��
    public AudioSource sfxSource;  // ���ʉ��iSFX�j�p��AudioSource
    public AudioSource uiSource;   // UI�p��AudioSource
    public AudioSource bgmSource;  // BGM�p��AudioSource

    public AudioClip[] sfxSounds;   // ���ʉ��N���b�v�̔z��
    public AudioClip[] missClips;  // ��U��p�̌��ʉ��N���b�v�̔z��
    public AudioClip[] uiSounds;    // UI���ʉ��N���b�v�̔z��
    public AudioClip[] bgmSounds;   // BGM�N���b�v�̔z��

    // �������ʂ̐ݒ�
    public float initBGMVolume; // ����BGM����
    public float initSFXVolume; // ����SFX����
    public float initUIVolume;  // ����UI����

    // �����N���b�v�̎����i���O�Ō����ł���悤�Ɂj
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> uiDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> missDict = new Dictionary<string, AudioClip>(); // ��U��p�̎���

    /// <summary>
    /// �V���O���g���p�^�[���̃C���X�^���X�������Ǝ����̐ݒ�B
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���ԂŃI�u�W�F�N�g��j�����Ȃ�
            InitializeAudioDictionaries();
            SetInitialVolumes(); // �������ʂ�ݒ�
        }
        else
        {
            Destroy(gameObject); // ���ɃC���X�^���X�����݂���ꍇ�A�d��������邽�߂ɔj��
        }
    }

    /// <summary>
    /// �e��I�[�f�B�I�N���b�v�������ɓo�^�B
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
    /// ���ʉ����Đ��B
    /// </summary>
    /// <param name="clipName">�Đ�����N���b�v�̖��O</param>
    public void PlaySFX(string clipName)
    {
        if (sfxDict.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip); // �w�肳�ꂽ���ʉ�����񂾂��Đ�
        }
        else
        {
            Debug.LogWarning("SFX clip not found: " + clipName); // �N���b�v��������Ȃ��ꍇ�͌x�����o��
        }
    }

    /// <summary>
    /// UI���ʉ����Đ��B
    /// </summary>
    /// <param name="clipName">�Đ�����N���b�v�̖��O</param>
    public void PlayUIClip(string clipName)
    {
        if (uiDict.TryGetValue(clipName, out var clip))
        {
            uiSource.PlayOneShot(clip); // �w�肳�ꂽUI���ʉ�����񂾂��Đ�
        }
        else
        {
            Debug.LogWarning("UI clip not found: " + clipName); // �N���b�v��������Ȃ��ꍇ�͌x�����o��
        }
    }

    /// <summary>
    /// BGM���Đ��B
    /// </summary>
    /// <param name="clipName">�Đ�����N���b�v�̖��O</param>
    /// <param name="loop">���[�v�Đ����邩�ǂ����i�f�t�H���g��true�j</param>
    public void PlayBGM(string clipName, bool loop = true)
    {
        if (bgmDict.TryGetValue(clipName, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play(); // �w�肳�ꂽBGM���Đ�
        }
        else
        {
            Debug.LogWarning("BGM clip not found: " + clipName); // �N���b�v��������Ȃ��ꍇ�͌x�����o��
        }
    }

    /// <summary>
    /// ��U�艹���Đ��B
    /// </summary>
    /// <param name="clipName">�Đ�����N���b�v�̖��O</param>
    public void PlayMissSound(string clipName)
    {
        if (missDict.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip); // �w�肳�ꂽ��U�艹����񂾂��Đ�
        }
        else
        {
            Debug.LogWarning("Miss sound clip not found: " + clipName); // �N���b�v��������Ȃ��ꍇ�͌x�����o��
        }
    }

    /// <summary>
    /// �e�I�[�f�B�I�\�[�X�̏������ʂ�ݒ�B
    /// </summary>
    private void SetInitialVolumes()
    {
        bgmSource.volume = initBGMVolume;
        sfxSource.volume = initSFXVolume;
        uiSource.volume = initUIVolume;
    }

    /// <summary>
    /// BGM�̉��ʂ�ݒ�B
    /// </summary>
    /// <param name="value">�ݒ肷�鉹��</param>
    public void SetBGMVolume(float value)
    {
        bgmSource.volume = value;
    }

    /// <summary>
    /// SFX�̉��ʂ�ݒ�B
    /// </summary>
    /// <param name="value">�ݒ肷�鉹��</param>
    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }

    /// <summary>
    /// UI���ʉ��̉��ʂ�ݒ�B
    /// </summary>
    /// <param name="value">�ݒ肷�鉹��</param>
    public void SetUIVolume(float value)
    {
        uiSource.volume = value;
    }

    /// <summary>
    /// �Đ�����BGM���~�B
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
