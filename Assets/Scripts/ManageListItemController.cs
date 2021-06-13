using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageListItemController : MonoBehaviour
{    
    public TextMeshProUGUI nameText, levelText, strText, balText, vtpText, cpText, positionText;
    public GameObject natureHolder;
    public GameObject natureIcon;

    public void SetText(Character character)
    {
        if (character.GetNature() == Nature.FIRE)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/fire", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_red", typeof(Sprite)) as Sprite;
        }
        else if (character.GetNature() == Nature.WATER)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/water", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_blue", typeof(Sprite)) as Sprite;
        }
        else if (character.GetNature() == Nature.WIND)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/wind", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_green", typeof(Sprite)) as Sprite;
        }

        nameText.text = character.GetName();
        levelText.text = character.GetLevel().ToString();
        strText.text = character.GetSTR().ToString();
        balText.text = character.GetBAL().ToString();
        vtpText.text = character.GetVTP().ToString();
        cpText.text = character.GetCP().ToString();
    }
}
