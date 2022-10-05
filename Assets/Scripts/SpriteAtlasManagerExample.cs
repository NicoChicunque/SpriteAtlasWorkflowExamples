using System;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtlasManagerExample : MonoBehaviour
{
    void OnEnable()
    {
        SpriteAtlasManager.atlasRequested += AtlasRequested;
    }    

    void OnDisable()
    {
        SpriteAtlasManager.atlasRequested -= AtlasRequested;
    }

    private void AtlasRequested(string tag, Action<SpriteAtlas> callback)
    {
        Debug.Log(tag);
        SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>(tag);//Load Sprite Atlases from Resources folder
        callback(spriteAtlas);
    }
}
