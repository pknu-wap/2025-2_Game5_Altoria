using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Lobby, // 로비 음악
    Forest // 숲 음악   
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // AudioClip
    [SerializeField] AudioClip[] bgmClips;

    // AudioSource
    [SerializeField] AudioSource audioBgm;
    List<AudioSource> sfxSources;

    Dictionary<BGM, AudioClip> bgmDict;
    Dictionary<string, AudioClip> sfxDict;

    [SerializeField] int sfxPoolSize = 10; // 동시에 재생 가능한 SFX 수

    private void Awake()
    {
        Instance = this;   //싱글톤 
        DontDestroyOnLoad(gameObject);

        bgmDict = new Dictionary<BGM, AudioClip>();
        Initialize();
    }
    private void Initialize()
    {
        // BGM 초기화
        bgmDict = new Dictionary<BGM, AudioClip>();
        for (int i = 0; i < bgmClips.Length; i++)
        {
            bgmDict[(BGM)i] = bgmClips[i];
        }

        // SFX 리소스 폴더에서 로드
        sfxDict = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sound");

        for (int i = 0; i < clips.Length; i++)
        {
            string key = clips[i].name; //파일 이름이 키
            if (!sfxDict.ContainsKey(key))
                sfxDict.Add(key, clips[i]);
        }

        Debug.Log($"[SoundManager] : {sfxDict.Count}개의 SFX 자동 등록 완료");

        sfxSources = new List<AudioSource>(sfxPoolSize);
        for (int i = 0; i < sfxPoolSize; i++)
        {
            GameObject sfxObj = new GameObject($"SFXSource_{i}");
            sfxObj.transform.SetParent(transform);
            AudioSource src = sfxObj.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sfxSources.Add(src);
        }
    }

    #region BGM
    public void PlayBGM(BGM type)
    {
        if (bgmDict.TryGetValue(type, out AudioClip clip))
        {
            audioBgm.clip = clip;
            audioBgm.loop = true;
            audioBgm.Play();
        }
        else
        {
            Debug.LogWarning($"[SoundManager] : BGM {type} not found!");
        }
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }
    public void SetBGMVolume(float value)
    {
        audioBgm.volume = value;   
    }
    public float GetBGMVolume()
    {
        return audioBgm.volume;
    }

    #endregion

    #region SFX
    public void PlaySFX(string name)
    {
        if (!sfxDict.TryGetValue(name, out AudioClip clip))
        {
            Debug.LogWarning($"[SoundManager] : SFX \"{name}\" not found!");
            return;
        }

        AudioSource availableSource = GetAvailableSFXSource();
        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.PlayOneShot(clip);
        }
    }
    private AudioSource GetAvailableSFXSource()
    {
        for (int i = 0; i < sfxSources.Count; i++)
        {
            if (!sfxSources[i].isPlaying)
                return sfxSources[i];
        }
        return sfxSources[0]; // 전부 사용 중이면 첫 번째 소스 재사용
    }
    public void StopSFX()
    {
        for (int i = 0; i < sfxSources.Count; i++)
        {
            sfxSources[i].Stop();
        }
    }

    public void SetSFXVolume(float v)
    {
        for (int i = 0; i < sfxSources.Count; i++)
        {
            sfxSources[i].volume = v;
        }
    }

    public float GetSFXVolume()
    {
        if (sfxSources.Count > 0)
            return sfxSources[0].volume;
        return 1f;
    }

    #endregion

    #region Save & Load
    public void saveToSettingData()
    {
    }
    #endregion
}
