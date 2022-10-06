using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/BuildAllAssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "C:/xampp/htdocs/AssetBundles";
        if ( ! Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}