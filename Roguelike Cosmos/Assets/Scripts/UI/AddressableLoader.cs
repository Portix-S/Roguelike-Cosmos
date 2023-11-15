using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace Tools
{
    public class AddressableLoader : MonoBehaviour
    {
        public static AddressableLoader instance;
        [Header("Graphics")]
        public AssetReferenceGameObject damagePopupPrefab;

        [Header("Player")]
        [SerializeField] private AssetReference _playerData;
        [HideInInspector] public Player.PlayerData playerData;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            Addressables.InitializeAsync();
            StartCoroutine(PreloadReferences());
        }

        private IEnumerator PreloadReferences()
        {
            AsyncOperationHandle<Player.PlayerData> playerDataLoadHandle = _playerData.LoadAssetAsync<Player.PlayerData>();
            yield return playerDataLoadHandle;
            playerData = playerDataLoadHandle.Result;
        }

        private void OnApplicationQuit()
        {
            _playerData.ReleaseAsset();
        }
    }
}