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
        Init();
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
        _FullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
        _ResolutionDropdown.RefreshShownValue();
    }

    //해상도 버튼클릭
    public void OptionChange(int x)
    {
        _ResolutionValue = x;
    }
    //전체 화면 설정
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    //실제 적용
    public void VidioBtnClick()
    {
        Screen.SetResolution(_Resolutions[_ResolutionValue].width, _Resolutions[_ResolutionValue].height, screenMode);
    }
}