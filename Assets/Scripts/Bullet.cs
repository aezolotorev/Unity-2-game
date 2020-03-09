using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseObject
{
    private int _damage = 20;
    private float _destructTime = 10;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Destroy(_GOInstance, _destructTime);
    }
    protected virtual void SetDamage(ISetDamage obj)
    {

        if (obj != null)
        {
            obj.SetDamage(_damage);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        SetDamage(collision.gameObject.GetComponent<ISetDamage>());

    }
}
