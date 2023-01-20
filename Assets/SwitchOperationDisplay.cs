using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOperationDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject[] _operationDisplays = default;

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

    public void SetEnebleUI(bool isActive)
    {
        _operationDisplays[currentID].SetActive(false);
    }
}
