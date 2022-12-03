using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILockOnTargetable
{
    void ShowLockOnIcon();

    void HideLockOnIcon();

    Transform GetCamPoint();
}
