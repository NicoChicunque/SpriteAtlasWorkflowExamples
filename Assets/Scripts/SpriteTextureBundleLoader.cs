using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpriteTextureBundleLoader : MonoBehaviour
{
    [SerializeField] private string bundleName = string.Empty, spriteName;
    // Start is called before the first frame update
    void Start()
    {
        //Load Sprite Atlases as AssetBundle from Host folder
        StartCoroutine(GetHostedAssetBundle((assetBundle) => {
            Debug.Log("SpriteTextureBundleLoader " + assetBundle);
            if (assetBundle == null)
            {
                Debug.Log("Can't get assetbundle");
                return;
            }
            GetComponent<Image>().sprite = assetBundle.LoadAsset<Sprite>(spriteName);
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
