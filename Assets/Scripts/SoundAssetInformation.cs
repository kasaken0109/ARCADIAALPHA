using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// soundAsset�Q�Ɨp�̃N���X
/// </summary>
public class SoundAssetInformation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("SoundAssets")]
    SoundAssets _soundAssets;
    public SoundAssets SoundAssets => _soundAssets;
}
