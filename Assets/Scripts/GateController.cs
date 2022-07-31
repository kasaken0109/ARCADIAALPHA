using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲートの処理を制御する
/// </summary>
public class GateController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("カメラオブジェクト")]
    GameObject _camera = null;

    [SerializeField]
    [Tooltip("ズームする時間")]
    float _zoomTime = 2f;
    void Start()
    {
        StartCoroutine(nameof(ZoomMe));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.SceneLoad();
            SoundManager.Instance.PlayMove();
        }
    }

    /// <summary>
    /// ズームカメラを有効にする
    /// </summary>
    /// <returns></returns>
    IEnumerator ZoomMe()
    {
        _camera?.SetActive(true);
        yield return new WaitForSeconds(_zoomTime);
        _camera?.SetActive(false);
    }
}
