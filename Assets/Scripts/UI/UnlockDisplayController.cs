using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDisplayController : MonoBehaviour
{
    [SerializeField]
    GameObject _unlockPanel;

    Animator _anim = default;

    const float WaitTime = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        _unlockPanel.TryGetComponent(out _anim);
    }

    public void UnlockDisplay(bool isUnlock)
    {
        const float WaitTime = 0.5f;
        IEnumerator UnlockAnim()
        {
            _anim.SetBool("IsUnlocked", true);
            yield return new WaitForSeconds(WaitTime);
            _unlockPanel.SetActive(false);
        }
        if (isUnlock) StartCoroutine(UnlockAnim());
        else _unlockPanel.SetActive(true);
    }
}
