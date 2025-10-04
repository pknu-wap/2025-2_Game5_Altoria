using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Lobby, // 로비 음악
    Forest // 숲 음악   
}

public enum SFX
{
    ButtonClick,     // 0  버튼 클릭 

    UIConfirm,        // 1  인게임 UI 확인(OK) 소리 
    UICancel,         // 2  인게임 UI 취소 소리 

    PlayerJump,      // 3  플레이어 점프 
    PlayerWalk,      // 4  플레이어 걷는 소리 
    MonsterHit,      // 5  몬스터 타격

    FishingCast,     // 6  낚싯대 던지는 소리
    FishingCatch,    // 7  물고기 잡는 소리

    PigOink,         // 8  돼지 우는 소리

    ItemPickUp,      // 9  아이템 줍는 소리
    Crafting,        // 10  제작 소리
    WaterSplash,     // 11  물 튀기는 소리
    TreeChop,        // 12  나무 찍는 소리
    Mining           // 13  돌 캐는 소리

}

public class SoundManager : MonoBehaviour
{
    // AudioClip
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] sfxClips;

    // AudioSource
    [SerializeField] private AudioSource audioBgm;
    [SerializeField] private AudioSource audioSfx;

    private Dictionary<BGM, AudioClip> bgmDict;
    private Dictionary<SFX, AudioClip> sfxDict;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;   //싱글톤 
        DontDestroyOnLoad(gameObject);

        bgmDict = new Dictionary<BGM, AudioClip>();
        for (int i = 0; i < bgmClips.Length; i++)
        {
            bgmDict[(BGM)i] = bgmClips[i];
        }

        sfxDict = new Dictionary<SFX, AudioClip>();
        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxDict[(SFX)i] = sfxClips[i];
        }
    }

    public void PlayBGM(BGM type)
    {
        if (bgmDict.TryGetValue(type, out var clip))
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

    public void PlaySFX(SFX type)
    {
        if (sfxDict.TryGetValue(type, out var clip))
        {
            audioSfx.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] : SFX {type} not found!");
        }
    }

    public void StopSFX()
    {
        audioBgm.Stop();
    }

    public void SetSFXVolume(float value)
    {
        audioSfx.volume = value;   
    }

    public float GetSFXVolume()
    {
        return audioSfx.volume;
    }


}
