using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight :BaseObject
{
    public KeyCode control = KeyCode.F;
    [SerializeField]
    private Light _light;
    [SerializeField]
    public float _lightintensity;//базовая интенсивность света
    [SerializeField]
    private float _lightintensityStep;//шаг уменьшения/увеличения интенсивности
    [SerializeField]
    private float _kdown; // коэффициент уменьшения  заряда фонаря с количеством полных использований
    [SerializeField]
    private float _kup; // коэффициент увеличения  заряда фонаря с количеством полных использований
    [SerializeField]
    private float _timerecharge; // для просмотра в инспекторе полного времени заряда
    [SerializeField]
    private AudioClip _flashoffon;
    

    

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponentInChildren<Light>();
        _lightintensity = 5;
        _light.intensity=_lightintensity;
        _lightintensityStep=2;
        _kdown = 10000;
        _kup = 5000;
    }

    // Update is called once per frame
    void Update()
    {
        if (_kdown <= 0) _light.enabled = false;
        if (Input.GetKeyDown(control))
        {
            _light.enabled = !_light.enabled;
            _timerecharge = 0;
            _audioSource.PlayOneShot(_flashoffon);
            if (_light.intensity == 0) ///если произошла полная разрядка, то время зарядки будет увеличиваться, а время свечения уменьшаться
            {
                _kup += 1000;
            _kdown -= 500;
            
            }
        }
        if (_light.enabled)
        {
            _light.intensity -= _lightintensityStep/(_kdown*Time.deltaTime);//уменьшение интенсивности света со временем
         
        }
        if (_light.enabled == false && _light.intensity< _lightintensity)//если выключен и интенсивность меньше максимальной, то набор интенсивности(энергии)
        {
            _light.intensity += _lightintensityStep/(_kup*Time.deltaTime);
            _timerecharge += Time.deltaTime;
        }
        
    }
}
