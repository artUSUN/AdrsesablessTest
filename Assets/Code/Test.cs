using System;
using System.Collections.Generic;
using Code;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button loadPrefabButton;
    
    [SerializeField] private Button consoleButton;
    [SerializeField] private TMP_Text consoleText;

    [SerializeField] private List<Sprite> sprites;

    [SerializeField] private Transform prefabRoot;
    
    [SerializeField] private AssetReference dictionary;
    
    private readonly string[] _labelsConstant = { "Prefabs" };

    private PrefabsList _prefabsList;

    private PrefabScript _spawnedPrefab;
    
    private void Awake()
    {
        consoleButton.onClick.AddListener(() => consoleText.text = "");
        loadPrefabButton.interactable = false;

        DownloadAsset();
    }

    private async void DownloadAsset()
    {
        var handler = Addressables.DownloadDependenciesAsync(_labelsConstant as IEnumerable<object>, Addressables.MergeMode.Union);
        var bundleSize = handler.GetDownloadStatus().TotalBytes;
        ConsoleLog($"Loading ({bundleSize.ToString()})");
        
        await handler.Task;

        if (handler.IsValid() && handler.Status == AsyncOperationStatus.Succeeded)
        {            
            Addressables.Release(handler);
            ConsoleLog("Loading Bundle Success");
            
            var handler2 = Addressables.LoadAssetAsync<PrefabsList>(dictionary);
            
            await handler2.Task;
            
            ConsoleLog("Loading prefab list Success");

            _prefabsList = handler2.Result;
            
            loadPrefabButton.onClick.AddListener(OnLoadButtonClick);
            loadPrefabButton.interactable = true;
        }
        else
        {
            ConsoleLog("Loading Bundle Error");
        }
    }
    
    private void OnLoadButtonClick()
    {
        if (_prefabsList == null)
        {
            ConsoleLog("_prefabsList == null");
            return;
        }

        if (_spawnedPrefab != null)
        {
            Destroy(_spawnedPrefab.gameObject);
            _spawnedPrefab = null;
        }
        
        var prefabName = inputField.text;

        var result =_prefabsList.Data.Find(x => string.Equals(prefabName, x.PrefabName, StringComparison.OrdinalIgnoreCase));

        if (result == null)
        {
            ConsoleLog($"{prefabName} not found");
            return;
        }

        LoadPrefab(result);
    }

    private async void LoadPrefab(PrefabData prefabData)
    {
        ConsoleLog("Loading Prefab...");
        
        var handler = Addressables.LoadAssetAsync<GameObject>(prefabData.PrefabReference);

        await handler.Task;

        if (handler.IsValid() && handler.Status == AsyncOperationStatus.Succeeded)
        {
            ConsoleLog("Prefab loaded");

            var prefab = handler.Result;
            
            _spawnedPrefab = Instantiate(prefab, prefabRoot).GetComponent<PrefabScript>();
        
            _spawnedPrefab.FillText(prefabData.PrefabName);
            _spawnedPrefab.FillImages(sprites);
        }
        else
        {
            ConsoleLog("Loading Prefab Error");
        }
    }

    private void ConsoleLog(string log)
    {
        consoleText.text = log;
    }
}
