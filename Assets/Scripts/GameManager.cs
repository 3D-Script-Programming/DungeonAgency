using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;
    public static GameManager instance { get; set; }
    public Player player { get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        player = new Player();
        player.AddGold(5000);
        audioSource.Play();
    }

    private void Start()
    {
        SceneManager.LoadScene("MainScene");
    }

    public static void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TogglePause()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }

    public void SetMusic(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }
}