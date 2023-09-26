using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketListItemController : MonoBehaviour
{
    // UI 요소들을 저장할 변수들
    public TextMeshProUGUI name, level, str, bal, vtp, cp, price; // 몬스터 정보 텍스트
    public Button btnListItem; // 몬스터 선택 버튼
    public Button btnHire; // 몬스터 고용 버튼
    public GameObject natureHolder; // 몬스터 속성 아이콘을 감싸는 오브젝트
    public GameObject natureIcon; // 몬스터 속성 아이콘 이미지

    // 현재 아이템에 대한 몬스터 정보
    private Character monster;
    // MarketManager 스크립트 참조를 저장하는 변수
    private MarketManager marketManager;

    private void Awake()
    {
        // UI 이벤트 등록
        ApplyUIEvents();
    }

    private void ApplyUIEvents()
    {
        btnListItem.onClick.AddListener(OnClickListItem); // 몬스터 선택 버튼 클릭 이벤트 등록
        btnHire.onClick.AddListener(OnClickBtnHire); // 몬스터 고용 버튼 클릭 이벤트 등록
    }

    // 몬스터 선택 버튼 클릭 시 호출되는 메서드
    public void OnClickListItem()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        marketManager.SpawnMonster(monster); // 몬스터 스폰
    }

    // 몬스터 고용 버튼 클릭 시 호출되는 메서드
    public void OnClickBtnHire()
    {
        GameManager.s_Instance.PlayButtonSound(); // 버튼 사운드 재생
        if (monster.GetPrice() > GameManager.s_Instance.player.Gold)
            return;
        GameManager.s_Instance.player.AddMonster(monster); // 몬스터 고용
        GameManager.s_Instance.player.AddGold(-monster.GetPrice()); // 골드 감소
        btnHire.gameObject.SetActive(false); // 몬스터 고용 버튼 비활성화
        marketManager.UIManager.UpdateUI(); // UI 업데이트
        SetButton();
    }

    // 몬스터 정보를 설정하고 UI 업데이트
    public void SetText(Character monster)
    {
        this.monster = monster;
        // 몬스터 속성에 따라 아이콘 및 프레임 설정
        if (monster.Nature == Nature.FIRE)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/fire", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_red", typeof(Sprite)) as Sprite;
        }
        else if (monster.Nature == Nature.WATER)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/water", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_blue", typeof(Sprite)) as Sprite;
        }
        else if (monster.Nature == Nature.WIND)
        {
            natureIcon.GetComponent<Image>().sprite = Resources.Load("UI/wind", typeof(Sprite)) as Sprite;
            natureHolder.GetComponent<Image>().sprite = Resources.Load("UI/slider_skill_frame_green", typeof(Sprite)) as Sprite;
        }

        // 몬스터 정보 텍스트 업데이트
        name.text = monster.Name;
        level.text = monster.Level.ToString();
        str.text = monster.Strength.ToString();
        bal.text = monster.Balance.ToString();
        vtp.text = monster.Vitality.ToString();
        cp.text = monster.GetCP().ToString();
        price.text = monster.GetPrice().ToString();

        // 버튼 상태 업데이트
        SetButton();
    }

    // 몬스터 고용 버튼의 활성화/비활성화 상태 설정
    public void SetButton()
    {
        if (monster.GetPrice() > GameManager.s_Instance.player.Gold)
            btnHire.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_red", typeof(Sprite)) as Sprite;
        else
            btnHire.gameObject.GetComponent<Image>().sprite = Resources.Load("UI/btn_color_green", typeof(Sprite)) as Sprite;
    }

    // MarketManager 스크립트 참조 설정
    public void SetMarketManager(MarketManager marketManager)
    {
        this.marketManager = marketManager;
    }
}