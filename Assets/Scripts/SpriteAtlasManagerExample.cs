using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpriteAtlasManagerExample : MonoBehaviour
{
    [SerializeField] string _bundleName = "includeinbuilddisabled";
    [SerializeField] SpriteAtlas _spriteAtlas;
    [SerializeField] List<Sprite> _sprites = new List<Sprite>();
    [SerializeField] Image _image;

    void OnEnable()
    {
        SpriteAtlasManager.atlasRequested += AtlasRequested;//https://docs.unity3d.com/ScriptReference/U2D.SpriteAtlasManager-atlasRequested.html
    }    

    void OnDisable()
    {
        SpriteAtlasManager.atlasRequested -= AtlasRequested;
    }

    void AtlasRequested(string tag, Action<SpriteAtlas> callback)
    {
        Debug.Log("Some sprite requires its atlas with tag: " + tag);
        if (_spriteAtlas == null)
        {
            Debug.Log("Save the scene here ??? ... After the customer could check to release the desired assets");
            StartCoroutine(LoadFromStreammingAsset(tag, callback));
        }
        else
        {
            callback(_spriteAtlas);
        }
    }

    IEnumerator LoadFromStreammingAsset(string tag, Action<SpriteAtlas> callback)
    {
        AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/AssetBundles/" + _bundleName);
        yield return bundleLoadRequest;
        AssetBundle bundle = bundleLoadRequest.assetBundle;

        if (bundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        AssetBundleRequest request = bundle.LoadAssetAsync(tag);
        yield return request;
        _spriteAtlas = request.asset as SpriteAtlas;

        if(_spriteAtlas == null)
        {
            Debug.Log("Failed to load sprite atlas asset!");
            yield break;
        }

        Sprite[] sprites = new Sprite[_spriteAtlas.spriteCount];
        _spriteAtlas.GetSprites(sprites);
        _sprites = new List<Sprite>(sprites);
        _image.sprite = _sprites[1];
        callback(_spriteAtlas);
        Debug.Log("Sprite Atlas Loaded");
    }
}