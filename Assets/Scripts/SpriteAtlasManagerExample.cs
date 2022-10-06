using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Networking;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class SpriteAtlasManagerExample : MonoBehaviour
{
    enum LoadOption { ResourcesFolderSpriteAtlas, LocalFolderAssetBundle, HostFolderAssetBundle };
    [SerializeField] private LoadOption loadOption = LoadOption.HostFolderAssetBundle;
    [SerializeField] private string bundleName = "includeinbuilddisabled";

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
        SpriteAtlas spriteAtlas = null;

        if (loadOption == LoadOption.ResourcesFolderSpriteAtlas)//Load Sprite Atlases from Resources folder 
        {
            spriteAtlas = Resources.Load<SpriteAtlas>(tag);
            callback(spriteAtlas);
            return;
        }

        //https://docs.unity3d.com/Manual/AssetBundles-Workflow.html
        if (loadOption == LoadOption.LocalFolderAssetBundle)//Load Sprite Atlases as AssetBundle from Local folder
        {
            AssetBundle bundle = AssetBundle.LoadFromFile("C:/xampp/htdocs/AssetBundles/" + bundleName);
            spriteAtlas = bundle.LoadAsset<SpriteAtlas>(tag);
            callback(spriteAtlas);
            return ;
        }

        //Load Sprite Atlases as AssetBundle from Host folder
        StartCoroutine(GetHostedAssetBundle((assetBundle) => {
            if (assetBundle == null)
            {
                Debug.Log("Can't get spriteatlas assetbundle with the option " + loadOption);
                return;
            }
            spriteAtlas = assetBundle.LoadAsset<SpriteAtlas>(tag);
            callback(spriteAtlas);
        }));
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
}