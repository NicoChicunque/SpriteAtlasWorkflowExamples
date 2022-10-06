using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class SpriteAtlasManagerExample : MonoBehaviour
{
    enum LoadOption { ResourcesFolderSpriteAtlas, LocalFolderAssetBundle, HostFolderAssetBundle, AddressableByAddress };
    [SerializeField] private LoadOption loadOption = LoadOption.HostFolderAssetBundle;
    [SerializeField] private string bundleName = "includeinbuilddisabled", spriteAtlasAddress = string.Empty;
    private AsyncOperationHandle<SpriteAtlas> spriteAtlasOperation;   

    void OnEnable()
    {
        SpriteAtlasManager.atlasRequested += AtlasRequested;//https://docs.unity3d.com/ScriptReference/U2D.SpriteAtlasManager-atlasRequested.html
    }    

    void OnDisable()
    {
        SpriteAtlasManager.atlasRequested -= AtlasRequested;
    }

    private void AtlasRequested(string tag, Action<SpriteAtlas> callback)//https://docs.unity3d.com/Manual/MethodDistribution.html
    {
        Debug.Log(tag + " Sprite Atlas Requested");

        if (loadOption == LoadOption.ResourcesFolderSpriteAtlas)//Load Sprite Atlases from Resources folder 
        {
            callback(Resources.Load<SpriteAtlas>(tag));
            return;
        }

        //https://docs.unity3d.com/Manual/AssetBundles-Workflow.html
        if (loadOption == LoadOption.LocalFolderAssetBundle)//Load Sprite Atlases as AssetBundle from Local folder
        {
            AssetBundle bundle = AssetBundle.LoadFromFile("C:/xampp/htdocs/AssetBundles/" + bundleName);
            if (bundle == null)
            {
                Debug.LogError("Can't get spriteatlas assetbundle with the option " + loadOption);
                return;
            }
            callback(bundle.LoadAsset<SpriteAtlas>(tag));
            bundle.Unload(false);
            return ;
        }

        if (loadOption == LoadOption.HostFolderAssetBundle)//Load Sprite Atlases as AssetBundle from Host folder
        {
            StartCoroutine(GetHostedAssetBundle((bundle) =>
            {
                if (bundle == null)
                {
                    Debug.LogError("Can't get spriteatlas assetbundle with the option " + loadOption);
                    return;
                }
                callback(bundle.LoadAsset<SpriteAtlas>(tag));
            }));
            return;
        }

        //Load Sprite Atlases AddressableByAddress
        spriteAtlasOperation = Addressables.LoadAssetAsync<SpriteAtlas>(spriteAtlasAddress);
        spriteAtlasOperation.Completed += (operation) =>
        {
            if (operation.Status.Equals(AsyncOperationStatus.Succeeded))
            {
                callback(operation.Result);
                return;
            }
            Debug.LogError("Sprite load failed. Using default sprite.");
        };
    }

    IEnumerator GetHostedAssetBundle(Action<AssetBundle> onGetAssetBundle)
    {
        string url = "http://localhost/AssetBundles/" + bundleName;
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.SendWebRequest();
        if (request.error == null)
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            onGetAssetBundle(bundle);            
            bundle.Unload(false);
        }
    }

    void OnDestroy()
    {
        if (spriteAtlasOperation.IsValid())
        {
            Addressables.Release(spriteAtlasOperation);
            Debug.Log("Successfully released atlasOperation.");
        }
    }
}