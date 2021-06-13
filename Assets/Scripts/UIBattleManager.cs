using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBattleManager : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject winUI;
    public GameObject failUI;
    public TextMeshProUGUI winGoldText;
    public TextMeshProUGUI WinEvilPointText;
    public TextMeshProUGUI failGoldText;
    public TextMeshProUGUI failEvilPointText;
    public Button winHome;
    public Button winRestart;
    public Button winNext;
    public Button failHome;
    public Button failRestart;
    public Button failNext;
    public Button quitOK;
    public AudioClip buttonSound;


    private void Start()
    {
        ApplyUIEvents();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetWinText(int gold, int evilPoint)
    {
        winGoldText.text = gold.ToString();
        WinEvilPointText.text = evilPoint.ToString();
    }
    
    public void SetFailText(int gold, int evilPoint)
    {
        failGoldText.text = gold.ToString();
        failEvilPointText.text = evilPoint.ToString();
    }

    private void ApplyUIEvents()
    {
        winHome.onClick.AddListener(GameManager.MoveMainScene);
        winRestart.onClick.AddListener(GameManager.MoveBattleScene);
        winNext.onClick.AddListener(GameManager.MoveManageScene);
        failHome.onClick.AddListener(GameManager.MoveMainScene);
        failRestart.onClick.AddListener(GameManager.MoveBattleScene);
        failNext.onClick.AddListener(GameManager.MoveManageScene);
        quitOK.onClick.AddListener(GameManager.MoveMainScene);
    }

    public void ButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }

}
