using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.Profiling;
[RequireComponent(typeof(NavMeshAgent))]

public class Bot : Unit
{
    private NavMeshAgent _agent;
    private Transform _playerPos;
    private float _grounCheckDistance=0.1f;
    private Vector3 _groundedNormal;
   
    private float _turnAmount;
    private float _forwardAmount;
    private float _movingTurnSpeed = 360;
    private float _stTurnSpeed = 180;

    private List<Vector3> _wayPoints = new List<Vector3>();
    [SerializeField]
    private int _wayCounter;
    private GameObject _wayPointMain;
    [SerializeField]
    private float _timeWait = 2f;
    [SerializeField]
    private float _timeOut;

    private int _stoppingDistance = 0;
    private int _activeDistance = 20;

    [SerializeField]
    private bool patrol;
    [SerializeField]
    private bool shooting;
    [SerializeField]
    private bool isTarget;
    [Space(20)]
    [SerializeField]
    private float _shootDistance = 1000f;
    [SerializeField]
    private int _damage = 20;

    [Tooltip("Объект должен находится на дуле оружия, добавляется атоматически")]
    [SerializeField]
    protected Transform _gunT;
    [SerializeField]
    protected ParticleSystem _muzzleFlash;
    [SerializeField]
    protected GameObject _hitParticle;

    [Header("Список целей")]
    [SerializeField]
    private List<Transform> visibleTargets = new List<Transform>();
    
    [Header("Угол обзора")][ContextMenuItem("Рандомные значения угла и радиуса", nameof(Randomize))]
    [Header("Угол обзора")] [Range(10, 40)] [SerializeField] private float _maxAngle = 15f;
    [Header("Зона обзора")] [Range(10, 40)] [SerializeField] private float _maxRadius = 10f;
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstackeMask;

    [Multiline(5)]
    public string Test;
    [TextArea(5,10)]
    public string Test2;

    [SerializeField] protected AudioSource _audio;
    [SerializeField] protected AudioClip[] _botsounds;

    private void Randomize()
    {
        _maxAngle = Random.Range(30, 90);
        _maxRadius = Random.Range(10, 40);
    }


    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _wayPointMain = GameObject.FindGameObjectWithTag("Waypoints");
        
        foreach (Transform T in _wayPointMain.transform)
        {
            _wayPoints.Add(T.position);
            Debug.Log("Размер" + _wayPoints.Count);
        }
        patrol = true;
        StartCoroutine(FindTargets());
        Health = 100;
        _agent.stoppingDistance = 3;
        
        _gunT = GameObject.FindGameObjectWithTag("GunT").transform;
        _muzzleFlash=GetComponentInChildren<ParticleSystem>();
        _hitParticle = Resources.Load<GameObject>("Prefab/Flare");
        if (GetComponent<AudioSource>())
        {
            _audioSource = GetComponent<AudioSource>();
        }

    }
    IEnumerator Shoot(RaycastHit playerHit)
    {
        
        yield return new WaitForSeconds(0.5f);
        Profiler.BeginSample("TEST");
        _audioSource.PlayOneShot(_botsounds[0]);
        _muzzleFlash.Play();
        playerHit.collider.GetComponent<ISetDamage>().SetDamage(_damage);
        GameObject tempHit = Instantiate(_hitParticle, playerHit.point, Quaternion.identity);
        tempHit.transform.parent = playerHit.transform;
        Destroy(tempHit, 0.5F);
        shooting = false;
        Profiler.EndSample();
    }
    IEnumerator FindTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            FindVisibleTargets();
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + Vector3.up;
        Handles.color = new Color(1, 0, 1, 0.1f);
        Handles.DrawSolidArc(pos, Vector3.up, transform.forward, _maxAngle, _maxRadius);
        Handles.DrawSolidArc(pos, Vector3.up, transform.forward, -_maxAngle, _maxRadius);
        Vector3 LineLeft = Quaternion.AngleAxis(-_maxAngle, transform.up) * transform.forward * _maxRadius;
        Vector3 LineRight = Quaternion.AngleAxis(_maxAngle, transform.up) * transform.forward * _maxRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, LineLeft);
        Gizmos.DrawRay(transform.position + Vector3.up, LineRight);
        Gizmos.DrawIcon(pos, "/img/bot.png");
        if (visibleTargets.Count > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, visibleTargets[0].position);
        }
    }
    private void FindVisibleTargets()
    {
        
        Collider[] targertinVeiwRadius = Physics.OverlapSphere(Position, _maxRadius, targetMask);
        for(int i=0; i<targertinVeiwRadius.Length; i++)
        {
            Transform target = targertinVeiwRadius[i].transform;
            Vector3 dirToTarget = (target.position - Position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < _maxAngle)
            {
                float distToTarget = Vector3.Distance(Position, target.position);
                if(!Physics.Raycast(new Vector3(Position.x, Position.y + 1, Position.z), dirToTarget, obstackeMask))
                {
                    if (!visibleTargets.Contains(target))
                    {
                        visibleTargets.Add(target);

                        
                    }
                    
                }
            }


        }
        
    }
        

    private void CheckGroudStatus()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.2F), Vector3.down, out hit, _grounCheckDistance))
        {
            
            _groundedNormal = hit.normal;
            Anim.applyRootMotion = true;

        }
        else
        {
            
            _groundedNormal = Vector3.up;
            Anim.applyRootMotion = false;

        }

    }
    private void TurnRotation()
    {
        float turnSpeed = Mathf.Lerp(_stTurnSpeed, _movingTurnSpeed , _forwardAmount);
        transform.Rotate(0, _turnAmount * _stTurnSpeed * Time.deltaTime, 0);
    }

    public void Move(Vector3 move)
    {
        if (move.magnitude > 1f)
        {
            move.Normalize();
            
        }
        move = transform.InverseTransformDirection(move);
        CheckGroudStatus();
        _turnAmount = Mathf.Atan2(move.x, move.z);
        _forwardAmount = move.z;
        TurnRotation();
        
    }
    private void ChangeWayPoint()
    {
        if (_wayCounter < _wayPoints.Count - 1)
        {
            _wayCounter++;
            _audioSource.PlayOneShot(_botsounds[1]);
        }
        else
        {
            _wayCounter = 0;
            _audioSource.PlayOneShot(_botsounds[1]);
        }
    }

    void Update()
    {
        if (visibleTargets.Count > 0)
        {
            patrol = false;
            
        }
        else
        {
            patrol = true;
        }
        if (_agent)
        {

            if (Dead)
            {
                _agent.ResetPath();
                
                Move(Vector3.zero);
                _rigidbody.isKinematic = true;
                Destroy(_GOInstance, 5f);
                return;
            }

            if (true)
            {
                Move(_agent.desiredVelocity);
                Anim.SetBool("move", true);
            }
            else
            {

                Move(Vector3.zero);
                Anim.SetBool("move", false);
            }

            if (patrol)
            {
                if (_wayPoints.Count >=2)
                {
                    _agent.stoppingDistance = _stoppingDistance;
                    _agent.SetDestination(_wayPoints[_wayCounter]);
                    
                    if (!_agent.hasPath)
                    {                        
                        _timeOut +=0.1f;
                        Debug.Log("счетчик" +_timeOut+"время смены" + _timeWait);
                        if (_timeOut > _timeWait)
                            {
                                _timeOut = 0;
                                ChangeWayPoint();
                            }
                    }                    
                }
                else if (_wayPoints.Count == 0)
                {
                    _agent.stoppingDistance = 5f;
                    _agent.SetDestination(_playerPos.position);
                }
            }
            else
            {
                _agent.stoppingDistance = 5f;
                _agent.SetDestination(_playerPos.position);
                Vector3 rayOrigin = new Vector3(Position.x, Position.y+1, Position.z);
                Ray ray = new Ray(rayOrigin,transform.forward);
                RaycastHit hit;
                Debug.DrawRay(ray.origin, ray.direction, Color.green);
                    if (Physics.Raycast(ray, out hit, _shootDistance))
                    {
                        if (hit.collider.tag == "Player" && !shooting)
                        {
                            StartCoroutine(Shoot(hit));
                            shooting = true;
                        }
                        else
                        {
                            return;
                        }
                    }
            }
        }
    }
}
