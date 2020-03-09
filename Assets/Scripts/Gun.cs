using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseWeapon
{
    [SerializeField]
    private int _bulletCount = 30;
    private float _shootDistance = 1000F;
    private int _damage = 20;
    private KeyCode Reload = KeyCode.R;
    private Vector3 _rayOrigin;

   
   
    

    // Update is called once per frame
    public override void Fire()
    {
        if(_bulletCount>0 && _fire)
        {
            
            _audioSource.PlayOneShot(_gunSounds[0]);
            _audioSource.PlayOneShot(_gunSounds[2]);

            _muzzleFlash.Play();
            _bulletCount--;
            RaycastHit hit;
            Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            _damage = 20;
            if(Physics.Raycast(ray,out hit, _shootDistance))
            {
                if (hit.collider.tag == "Player")
                {
                    return;
                }
                else
                {
                    if (hit.collider.tag == "Wall")
                    {
                        Debug.Log("Wall");

                        RaycastHit check;
                        _rayOrigin = hit.point;
                        for (int i = 0; i < 5; i++)
                        {
                            if (Physics.Raycast(_rayOrigin, ray.direction, out check))
                            {
                                if (check.collider.tag == "Wall")
                                {
                                    _rayOrigin = check.point + ray.direction * 0.2F;
                                    CreateParticle(check);
                                    if (_damage > 0)
                                    {
                                        _damage -= 5;
                                    }
                                }
                                else
                                {
                                    CreateParticle(hit);
                                    SetDamage(hit.collider.GetComponent<ISetDamage>());
                                    Debug.Log("Curret Damage:" + _damage);
                                    break;
                                }
                            }

                            else
                            {
                                break;
                            }
                        }
                        
                       
                    }
                    SetDamage(hit.collider.GetComponent<ISetDamage>());
                    CreateParticle(hit);
                }
               
            }

            
        }
        
        
    }
    private void CreateParticle(RaycastHit hit)
    {
        GameObject tempHit = Instantiate(_hitParticle, hit.point + (hit.normal * 0.1f), Quaternion.FromToRotation(Vector3.up, hit.normal));
        tempHit.transform.parent = hit.transform;
        Destroy(tempHit, 0.1f);
    }

    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.SetDamage(_damage);
        }
    }
    public void Reset()
    {
        _bulletCount = 30;
        _fire = true;
        _reload = false;
        _animator.SetBool("shot", true);
        _animator.SetBool("relod", false);
    }

    protected override void Update()
    {
        if (!_reload)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
                _animator.SetTrigger("shoot");

                if (_bulletCount == 0) _animator.SetBool("shot", false);
            }
        }
        if (Input.GetKeyDown(Reload))
        {

            _fire = false;
            _reload = true;
            _animator.SetTrigger("reload");
            _animator.SetBool("shot", false);
            _animator.SetBool("relod", true);
            _audioSource.PlayOneShot(_gunSounds[1]);
        }
     
    }
}
