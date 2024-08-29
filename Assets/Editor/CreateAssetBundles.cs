using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/BuildAllAssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = Application.streamingAssetsPath + "/AssetBundles";
        if ( ! Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}