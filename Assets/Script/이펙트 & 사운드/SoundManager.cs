using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound
{
    public enum SoundType
    {
        Bgm = 0,
        Effect = 1,
        MaxCount
    }
}


public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField]
    AudioSource[] _AudioSources = new AudioSource[(int)Sound.SoundType.MaxCount];

    private Dictionary<string, AudioClip> _AudioClips = new Dictionary<string, AudioClip>();


    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider effectSlider;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //
        AudioPlay("Sound/Bgm/Bgm_Lobby", Sound.SoundType.Bgm);
        float volume = 0;
        volume = SettingData.BgmVolumeLoad();
        _AudioSources[(int)Sound.SoundType.Bgm].volume = volume;
        bgmSlider.value = volume;
        volume = SettingData.EffectVolumeLoad();
        _AudioSources[(int)Sound.SoundType.Effect].volume = volume;
        effectSlider.value = volume;

    }
    public void Clear()
    {
        foreach (AudioSource source in _AudioSources)
        {
            source.clip = null;
            source.Stop();
        }
        _AudioClips.Clear();

    }
    public void AudioPlay(AudioClip audioClip, Sound.SoundType type = Sound.SoundType.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Sound.SoundType.Bgm)
        {
            //배경음
            AudioSource audioSource = _AudioSources[(int)Sound.SoundType.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();


            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();

        }
        else
        {
            //효과음
            AudioSource audioSource = _AudioSources[(int)Sound.SoundType.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }

    }
    public void AudioPlay(string path, Sound.SoundType type = Sound.SoundType.Effect, float pitch = 1.0f)
    {

        AudioClip audioClip = GetAudioClip(path, type);
        if (audioClip == null)
            return;


        AudioPlay(audioClip, type, pitch);




    }
    private AudioClip GetAudioClip(string path, Sound.SoundType type = Sound.SoundType.Effect)
    {

        AudioClip audioClip = null;


        if (type == Sound.SoundType.Bgm)
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else
        {

            if (_AudioClips.ContainsKey(path))
                audioClip = _AudioClips[path];
            else
            {
                audioClip = Resources.Load<AudioClip>(path);
                _AudioClips.Add(path, audioClip);
            }

        }



        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;

    }



    public void BgmVolumnSet(Slider slider)
    {
        _AudioSources[(int)Sound.SoundType.Bgm].volume = slider.value;
        SettingData.BgmVolumeSave(slider.value);
    }
    public void EffectVolumnSet(Slider slider)
    {
        _AudioSources[(int)Sound.SoundType.Effect].volume = slider.value;
        SettingData.EffectVolumeSave(slider.value);
    }







}