using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpriteTextureBundleLoader : MonoBehaviour
{
    [SerializeField] private string bundleName = string.Empty;
    // Start is called before the first frame update
    void Start()
    {
        
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
