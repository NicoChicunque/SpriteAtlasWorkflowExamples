using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteAtlasManagerExample : MonoBehaviour
{
    [SerializeField] string _bundleName = "includeinbuilddisabled";
    [SerializeField] SpriteAtlas _spriteAtlas;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] Image[] _testImages;

    void Start()
    {
        SpriteAtlasManager.atlasRequested += AtlasRequested;//https://docs.unity3d.com/ScriptReference/U2D.SpriteAtlasManager-atlasRequested.html
    }    

    void OnDestroy()
    {
        SpriteAtlasManager.atlasRequested -= AtlasRequested;
    }

    void AtlasRequested(string tag, Action<SpriteAtlas> callback)
    {
        Debug.Log("Some sprite requires its atlas with tag: " + tag);

        if (_spriteAtlas == null)
        {
            StartCoroutine(LoadFromStreammingAsset(tag, callback));
            return;
        }

        Debug.Log("Sprite atlas already loaded!");
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

        if (_spriteAtlas == null)
        {
            Debug.Log("Failed to load sprite atlas asset!");
            yield break;
        }

        callback(_spriteAtlas);

        _sprites = new Sprite[_spriteAtlas.spriteCount];
        _spriteAtlas.GetSprites(_sprites);
        _testImages[0].sprite = _sprites[0];
        _testImages[1].sprite = _sprites[1];

        Debug.LogWarning("Save the scene here, or the bundle ??? ... After the customer could check to release the desired assets");
        //bundle.UnloadAsync(true);
    }
}