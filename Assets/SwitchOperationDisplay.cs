using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOperationDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject[] _operationDisplays = default;
    [SerializeField]
    GameObject[] _operationImageDisplays = default;

    int currentID = 0;
    // Start is called before the first frame update
    public void SwitchOperation(int id)
    {
        currentID = id;
        for (int i = 0; i < _operationDisplays.Length; i++)
        {
            _operationDisplays[i].SetActive(i == id);
        }
    }

    public void SwitchOperationImage(int id,float displayDuraration = 3f)
    {
        IEnumerator DelayNonActive()
        {
            Debug.Log(id);
            for (int i = 0; i < _operationImageDisplays.Length; i++)
            {
                _operationImageDisplays[i].SetActive(i == id);
            }
            yield return new WaitForSeconds(displayDuraration);
            _operationImageDisplays[id].SetActive(false);
        }
        StartCoroutine(DelayNonActive());
    }

    public void SetEnebleUI(bool isActive)
    {
        _operationDisplays[currentID].SetActive(isActive);
    }
}
