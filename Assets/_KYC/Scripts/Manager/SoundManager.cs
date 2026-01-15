using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유니티 6 기반 농장 시뮬레이션 사운드 컨트롤러
/// 문자열 카테고리와 인덱스를 활용하여 직관적인 사운드 재생을 지원합니다.
/// </summary>
public class SoundManager : MonoBehaviour
{
    // 포트폴리오용 데이터 구조 정의 (내부 클래스)
    [Serializable]
    public class SoundGroup
    {
        public string categoryName; // "진영", "영찬", "여름" 등
        public AudioClip[] clips;   // 해당 카테고리의 오디오 파일들
    }

    [Header("사운드 데이터 설정")]
    [SerializeField] private List<SoundGroup> bgmGroups = new List<SoundGroup>();
    [SerializeField] private List<SoundGroup> sfxGroups = new List<SoundGroup>();

    [Header("오디오 소스 (플레이어)")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("볼륨 설정")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;

    // 빠른 검색을 위한 자료구조
    private Dictionary<string, AudioClip[]> bgmDict = new Dictionary<string, AudioClip[]>();
    private Dictionary<string, AudioClip[]> sfxDict = new Dictionary<string, AudioClip[]>();

    /// <summary>
    /// MasterManager에서 호출하는 초기화 함수
    /// </summary>
    public void Init()
    {
        // 1. 리스트 데이터를 딕셔너리로 변환 (런타임 성능 최적화)
        bgmDict.Clear();
        foreach (var group in bgmGroups)
            if (!bgmDict.ContainsKey(group.categoryName)) bgmDict.Add(group.categoryName, group.clips);

        sfxDict.Clear();
        foreach (var group in sfxGroups)
            if (!sfxDict.ContainsKey(group.categoryName)) sfxDict.Add(group.categoryName, group.clips);

        // 2. 오디오 소스 설정 확인
        SetupAudioSources();

        // 3. 초기 볼륨 적용
        ApplyAllVolumes();

        Debug.Log("<color=green>SoundManager: 딕셔너리 기반 사운드 시스템 초기화 완료</color>");
    }

    private void SetupAudioSources()
    {
        if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;      // 배경음은 루프
        bgmSource.playOnAwake = false;
        sfxSource.loop = false;     // 효과음은 단발성
        sfxSource.playOnAwake = false;
    }

    #region BGM 재생 로직
    /// <summary>
    /// 카테고리 이름과 인덱스로 BGM 재생
    /// 예: PlayBGM("영찬", 0);
    /// </summary>
    public void PlayBGM(string name, int index)
    {
        if (bgmDict.TryGetValue(name, out AudioClip[] clips))
        {
            if (index < 0 || index >= clips.Length || clips[index] == null) return;

            // 현재 재생중인 곡과 같은지 확인 (중복 재생 방지)
            if (bgmSource.clip == clips[index] && bgmSource.isPlaying) return;

            bgmSource.clip = clips[index];
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"SoundManager: {name} 카테고리의 BGM을 찾을 수 없습니다.");
        }
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
    }
    #endregion

    #region SFX 재생 로직
    /// <summary>
    /// 카테고리 이름과 인덱스로 SFX 재생
    /// 예: PlaySFX("진영", 1);
    /// </summary>
    public void PlaySFX(string name, int index)
    {
        if (sfxDict.TryGetValue(name, out AudioClip[] clips))
        {
            if (index < 0 || index >= clips.Length || clips[index] == null) return;

            // 효과음은 중첩 재생이 가능하도록 PlayOneShot 사용
            sfxSource.PlayOneShot(clips[index]);
        }
    }
    #endregion

    #region 볼륨 관리 로직
    public void ApplyAllVolumes()
    {
        if (bgmSource != null) bgmSource.volume = masterVolume * bgmVolume;
        if (sfxSource != null) sfxSource.volume = masterVolume * sfxVolume;
    }

    // 외부(UI 등)에서 실시간으로 호출하여 볼륨 조절 가능
    public void SetMasterVolume(float vol) { masterVolume = vol; ApplyAllVolumes(); }
    public void SetBGMVolume(float vol) { bgmVolume = vol; ApplyAllVolumes(); }
    public void SetSFXVolume(float vol) { sfxVolume = vol; ApplyAllVolumes(); }
    #endregion
}