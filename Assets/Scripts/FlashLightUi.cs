using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FlashLightUi : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Light  _light;
    private void Start()
    {
        _light = GetComponentInChildren<Light>();
        slider.maxValue = _light.intensity;
    }

    void Update()
    {
        slider.value = _light.intensity;
    }
}
