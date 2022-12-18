using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipChecker : MonoBehaviour
{
    public void Search(int index)
    {
        IEnumerator Delay(int index)
        {
            yield return new WaitForEndOfFrame();
            ServiceLocator.GetInstance<EquipChangeManager>().SetState(index);
        }
        StartCoroutine(Delay(index));
    }
}
