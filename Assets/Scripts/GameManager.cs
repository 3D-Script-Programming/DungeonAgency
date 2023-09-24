using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 배경 음악을 재생하는 오디오 소스
    [SerializeField] private AudioSource audioSource;

    // 버튼 클릭 시 재생할 소리 
    [SerializeField] private AudioClip buttonSound;

    // GameManager의 인스턴스 (싱글톤)
    public static GameManager s_Instance { get; private set; }

    // 플레이어 정보를 관리하는 객체
    public Player player { get; set; }

    // Awake 함수는 게임 오브젝트가 활성화되기 전에 호출됩니다.
    // GameManager 인스턴스를 설정하고 다른 씬으로 이동해도 유지되도록 설정합니다.
    // 플레이어 객체를 생성하고 초기 골드를 설정하여 게임을 시작합니다.
    private void Awake()
    {
        if (s_Instance != null)
        {
            // 이미 GameManager 인스턴스가 존재하면 중복 생성을 방지하고 경고 메시지를 출력
            Debug.LogWarning("GameManager instance already exists. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        // GameManager 인스턴스를 설정하고 다른 씬으로 이동해도 유지되도록 설정
        s_Instance = this;
        DontDestroyOnLoad(gameObject);

        // 플레이어 객체를 생성하고 초기 골드를 설정하여 게임을 시작
        player = new Player();
        player.AddGold(5000);

        // 배경 음악을 재생
        audioSource.Play();
    }

    // Start 함수는 게임 오브젝트가 활성화된 후에 호출됩니다.
    // 게임 시작 시 MainScene을 로드합니다.
    private void Start()
    {
        SceneManager.LoadScene("MainScene");
    }

    // MoveScene 함수는 지정된 씬으로 이동하는 정적 메서드입니다.
    public static void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // TogglePause 함수는 일시 정지 상태를 전환 (0 또는 1로)합니다.
    public void TogglePause()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }

    // PlayButtonSound 함수는 버튼 클릭 시 버튼 사운드를 한 번 재생합니다.
    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }

    // SetMusic 함수는 배경 음악을 설정하고 재생합니다.
    public void SetMusic(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void ChangeVolume(float value)
    {
        audioSource.volume = value;
    }
}