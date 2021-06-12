using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventManager : MonoBehaviour
{
    public Button playButton;
    public Button shopButton;
    public Button settingButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(GameManager.MoveManageScene);
        shopButton.onClick.AddListener(GameManager.MoveShopScene);
        settingButton.onClick.AddListener(GameManager.MoveSettingScene);
    }
}
