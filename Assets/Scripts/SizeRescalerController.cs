using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeRescalerController : MonoBehaviour
{
    [SerializeField]
    SizeRescaler[] _sizeRescalers;

    int id = 0; 
    // Start is called before the first frame update
    void OnEnable()
    {
        SelectRescaler();
    }

    public void SelectRescaler()
    {
        if (id > _sizeRescalers.Length) return;
        for (int i = 0; i < _sizeRescalers.Length; i++)
        {
            if (id == i) _sizeRescalers[i].ScaleExpand();
            else _sizeRescalers[i].ScaleShrink();
        }
    }

    public void SelectSwordDisplay(bool isPrev)
    {
        id = isPrev ?
                (id == 0 ? _sizeRescalers.Length - 1 : id -= 1) :
                (id == _sizeRescalers.Length - 1 ? 0 : id += 1);
        SelectRescaler();
    }

    // Update is called once per frame
    void Update()
    {
        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }
}
