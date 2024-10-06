using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Fire(bool continuous);

    public void Reload();

    public void Discard();

    public void IncreaseAmmo(int amount);

}