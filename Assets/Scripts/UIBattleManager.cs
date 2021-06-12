using UnityEngine;
using TMPro;

public class UIBattleManager : MonoBehaviour
{
    public GameObject winUI;
    public GameObject failUI;
    public TextMeshProUGUI winGoldText;
    public TextMeshProUGUI WinEvilPointText;
    public TextMeshProUGUI failGoldText;
    public TextMeshProUGUI failEvilPointText;

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

    public void OnPause()
    {
        Time.timeScale = 0f;
    }

    public void OffPause()
    {
        Time.timeScale = 1f;
    }

}
