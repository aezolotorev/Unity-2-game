using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class GunPrefab : BaseWeapon
{
    private int _bulletCount = 30;
    private float _shootDistance = 100F;
    private int _damage = 20;
    private KeyCode Reload = KeyCode.R;
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject bullet;

    // Update is called once per frame
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetWidth(0.02f, 0.02f);
    }
    private Vector3 GetDirection(Vector3 HitPoint, Vector3 bulletPos)
    {
        Vector3 decr = HitPoint - bulletPos;
        float dist = decr.magnitude;
        return decr / dist;
    }
    public override void Fire()
    {
        if (_bulletCount > 0 && _fire)
        {

            _audioSource.PlayOneShot(_gunSounds[0]);
            Anim.SetBool("shoot", true);
            _muzzleFlash.Play();
            _bulletCount--;
            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(crossHair.transform.position);
            GameObject temp = Instantiate(bullet, _gunT.position, transform.rotation);
            Rigidbody BulletRB = temp.GetComponent<Rigidbody>();
            line.SetPosition(0, _gunT.position);
            if (Physics.Raycast(ray, out hit, _shootDistance))
            {
                BulletRB.AddForce(GetDirection(hit.point, temp.transform.position) * _shootDistance, ForceMode.Impulse);
                line.SetPosition(1, hit.point);
                
            }
            else
            {
                BulletRB.AddForce(GetDirection(ray.GetPoint(1000000f), temp.transform.position)*_shootDistance,ForceMode.Impulse);
                line.SetPosition(1, _gunT.position + ray.GetPoint(1000000f));
            }


        }
        else Anim.SetBool("shoot", false);
    }
    protected override void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();

        }
        else
        {
            Anim.SetBool("shoot", false);

        }
        if (Input.GetKeyDown(Reload))
        {

            _bulletCount = 30;

        }
    }
    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.SetDamage(_damage);
        }
    }
}
