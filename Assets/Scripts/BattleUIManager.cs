using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
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

    // 새로운 속성: 전투 승리 텍스트
    public string VictoryText
    {
        get { return "Victory!"; }
    }

    // 새로운 속성: 전투 패배 텍스트
    public string DefeatText
    {
        get { return "Defeat!"; }
    }

    private void Start()
    {
        ApplyUIEvents();
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
        winHome.onClick.AddListener(() => GameManager.MoveScene("MainScene"));
        winRestart.onClick.AddListener(() => GameManager.MoveScene("BattleScene"));
        winNext.onClick.AddListener(() => GameManager.MoveScene("ManageScene"));
        failHome.onClick.AddListener(() => GameManager.MoveScene("MainScene"));
        failRestart.onClick.AddListener(() => GameManager.MoveScene("BattleScene"));
        failNext.onClick.AddListener(() => GameManager.MoveScene("ManageScene"));
        quitOK.onClick.AddListener(() => GameManager.MoveScene("MainScene"));
    }
}
