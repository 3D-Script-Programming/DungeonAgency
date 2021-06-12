using UnityEngine;
using TMPro;

public class UIBattleManager : MonoBehaviour
{
    public GameObject winUI;
    public GameObject failUI;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI winGoldText;
    public TextMeshProUGUI WinEvilPointText;
    public TextMeshProUGUI failScoreText;
    public TextMeshProUGUI failGoldText;
    public TextMeshProUGUI failEvilPointText;

    public void SetWinText(int score, int gold, int evilPoint)
    {
        winScoreText.text = score.ToString();
        winGoldText.text = gold.ToString();
        WinEvilPointText.text = evilPoint.ToString();
    }
    
    public void SetFailText(int score, int gold, int evilPoint)
    {
        failScoreText.text = score.ToString();
        failGoldText.text = gold.ToString();
        failEvilPointText.text = evilPoint.ToString();
    }


}
