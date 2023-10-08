using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    public GameObject winUI;
    public GameObject failUI;
    public TextMeshProUGUI winGoldText;
    public TextMeshProUGUI WinInfamyText;
    public TextMeshProUGUI failGoldText;
    public TextMeshProUGUI failInfamyText;
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

    public void SetWinText(int gold, int infamy)
    {
        winGoldText.text = GameManager.s_Instance.player.Gold.ToString() + " + " + gold.ToString();
        WinInfamyText.text = GameManager.s_Instance.player.Infamy.ToString() + " + " + infamy.ToString();
    }

    public void SetFailText(int gold, int infamy)
    {
        failGoldText.text = GameManager.s_Instance.player.Gold.ToString() + " + " + gold.ToString();
        failInfamyText.text = GameManager.s_Instance.player.Infamy.ToString() + " + " + infamy.ToString();
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
