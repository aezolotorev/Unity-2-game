using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public abstract class BaseWeapon : BaseObject
{
    [SerializeField] protected Transform _gunT;
    [SerializeField] protected ParticleSystem _muzzleFlash;
    [SerializeField] protected GameObject _hitParticle;
    [SerializeField] protected GameObject crossHair;

    protected Timer _rechargetimer=new Timer();
    protected bool _fire = true;
    protected bool _reload = false;
    protected bool _aim = false;
    [SerializeField] protected AudioSource _audio;
    [SerializeField] protected AudioClip[] _gunSounds;
    // Start is called before the first frame update
    public abstract void Fire();


    protected override void Awake()
    {
        base.Awake();
        _gunT = _GOTransform.GetChild(2);
        _muzzleFlash = GetComponentInChildren<ParticleSystem>();
        _hitParticle = Resources.Load<GameObject>("Prefab/Flare");
        if (GetComponent<AudioSource>())
        {
            _audio = GetComponent<AudioSource>();
        }
        
    }    

    // Update is called once per frame
    protected virtual void Update()
    {
        _rechargetimer.Update();
        if (_rechargetimer.IsEvent())
        {
            _fire = true;
        }
    }
}
