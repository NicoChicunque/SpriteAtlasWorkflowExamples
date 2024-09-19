using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadFromLabel : MonoBehaviour
{
    enum UserChoice
    {
        SD,
        HD
    }
    [SerializeField] UserChoice userChoice;
    // List to hold the loaded textures
    List<Texture> loadedTextures = new List<Texture>();

    [ContextMenu("Load Textures By Addressables Label")]
    void Start()
    {
        StartCoroutine(LoadAssetsByLabelAsync<Texture>(userChoice.ToString(), OnTexturesLoaded));
    }
    // Coroutine to load assets by label
    IEnumerator LoadAssetsByLabelAsync<T>(string label, System.Action<IList<T>> callback)
    {
        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            callback?.Invoke(handle.Result);
        }
        else
        {
            Debug.LogError("Failed to load assets with label: " + label);
        }
        // Release the handle
        Addressables.Release(handle);
    }
    // Callback method when textures are loaded
    void OnTexturesLoaded(IList<Texture> textures)
    {
        loadedTextures.Clear();
        loadedTextures.AddRange(textures);
        Debug.Log("Loaded " + textures.Count + " textures.");
        foreach (var tex in textures)
        {
            Debug.Log(tex.name + " ");
        }
    }
}