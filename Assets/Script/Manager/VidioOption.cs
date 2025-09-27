using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VidioOption : MonoBehaviour
{
    private FullScreenMode screenMode;
    [SerializeField]
    public Toggle _FullScreenBtn;
    [SerializeField]
    private TMP_Dropdown _ResolutionDropdown;
    List<Resolution> _Resolutions = new List<Resolution>();

    private int _ResolutionValue;
    

    private void Start()
    {
        //Init();
        StartCoroutine(InitCoroutine());
        //StartCoroutine(ApplyResolutionDeferred());
     
    }

    private void Init()
    {


        //해상도가 높은것으로 나열될 수 있도록 Reverse함수 선언
        _Resolutions.AddRange(Enumerable.Reverse(Screen.resolutions));
        //생성전 드롭다운 초기화
        _ResolutionDropdown.options.Clear();
        int optionvalue = 0;
        foreach (Resolution resolution in _Resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = resolution.width + " x " + resolution.height + " " + resolution.refreshRate+"hz";
            _ResolutionDropdown.options.Add(option);
            //현재 해상도에 맞게 밸류설정
            if (resolution.width == Screen.width && resolution.height == Screen.height)
                _ResolutionDropdown.value = optionvalue;

            optionvalue++;

        }
        _ResolutionDropdown.RefreshShownValue();

    }
    

    // 옵션 초기화 + TMP_Dropdown 설정
    private IEnumerator InitCoroutine()
    {
        // 해상도를 높은 순서대로 가져오기
        _Resolutions.AddRange(Enumerable.Reverse(Screen.resolutions));

        // 드롭다운 옵션 초기화
        _ResolutionDropdown.options.Clear();

        foreach (Resolution res in _Resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{res.width} x {res.height} {res.refreshRate}hz";
            _ResolutionDropdown.options.Add(option);
        }

        // 한 프레임 기다리기 (Dropdown 내부 초기화 안정화)
        yield return null;

        // 현재 화면 해상도와 일치하는 옵션 찾기
        _ResolutionValue = _Resolutions.FindIndex(r => r.width == Screen.width && r.height == Screen.height);
        if (_ResolutionValue < 0) _ResolutionValue = 0; // 없으면 첫 옵션
        _ResolutionDropdown.value = _ResolutionValue;

        // 드롭다운 이벤트 연결
        _ResolutionDropdown.onValueChanged.AddListener(OnResolutionChange);
    }

    private IEnumerator ApplyResolutionDeferred()
    {
        yield return null; // 한 프레임 대기
        Screen.SetResolution(_Resolutions[_ResolutionValue].width, _Resolutions[_ResolutionValue].height, screenMode);
    }


    //해상도 버튼클릭
    public void OnResolutionChange(int index)
    {
        _ResolutionValue = index;


    }
    //전체 화면 설정
    //실제 적용 
    public void VidioBtnClick()
    {
        if(_FullScreenBtn.isOn)
        {
            screenMode = FullScreenMode.FullScreenWindow;
        }else
        {
            screenMode = FullScreenMode.Windowed;
        }
        Screen.SetResolution(_Resolutions[_ResolutionValue].width, _Resolutions[_ResolutionValue].height, screenMode);
    }
}