using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : BaseObject
{
    public int  weaponID=0;

    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int PreviousSelectyWeapon=weaponID;
        if ((Input.GetAxis("Mouse ScrollWheel") < 0))
        {
            if (weaponID <= 0)
            {
                weaponID = ChildCount - 1;
            }
            else
            {
                weaponID--;
            }
        }
        if ((Input.GetAxis("Mouse ScrollWheel") > 0))
        {
            if (weaponID >= ChildCount - 1)
            {
                weaponID = 0;
            }
            else
            {
                weaponID++;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponID = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && ChildCount > 2)
        {
            weaponID = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && ChildCount > 3)
        {
            weaponID = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && ChildCount > 4)
        {
            weaponID = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && ChildCount > 5)
        {
            weaponID = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && ChildCount >= 6)
        {
            weaponID = 5;
        }

     
        if (PreviousSelectyWeapon!=weaponID)
        {
            SelectWeapon();
        }
    
    }
    private void SelectWeapon()
       
    {
        int i = 0;
        foreach(Transform weapon in _GOTransform)
        {
            if (i == weaponID)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    
}
