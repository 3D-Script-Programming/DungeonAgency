using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class listItemController : MonoBehaviour
{
    public TextMeshProUGUI name, level, str, bal, vtp, cp;
    public GameObject btnHire;
    public GameObject natureHolder;
    public GameObject natureIcon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText(Character character, int money)
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

        name.text = character.GetName();
        level.text = character.GetLevel().ToString();
        str.text = character.GetSTR().ToString();
        bal.text = character.GetBAL().ToString();
        vtp.text = character.GetVTP().ToString();
        cp.text = character.GetCP().ToString();

        btnHire.GetComponentInChildren<TextMeshProUGUI>().text = character.GetPrice().ToString();

        if (character.GetPrice() > money)
            btnHire.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        else
            btnHire.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
    }

    public void SetButtonActive(bool isActive)
    {
        if (isActive)
        {
            btnHire.SetActive(true);
            btnHire.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
        }
        else
        {
            btnHire.SetActive(false);
        }
    }
}
