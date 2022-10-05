using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ToggleSpriteManager : MonoBehaviour
{
    [SerializeField] private GameObject spriteAtlasManagerExample;   
    [SerializeField] private List<Image> images;
    [SerializeField] private Sprite spriteGeneric;
    [SerializeField] private List<Sprite> spritesToUse = new List<Sprite>();

    private void Start()
    {
        for (int i = 0; i < images.Count; i++)
        {
            spritesToUse.Add(images[i].sprite);
        }

        GetComponent<Toggle>().onValueChanged.AddListener(
            (value) =>
            {
                spriteAtlasManagerExample.SetActive(value);
                List<Sprite> sprites = new List<Sprite>();
                for (int i = 0; i < images.Count; i++)
                {
                    //string spriteName = images[i].sprite.name;
                    //Sprite spriteToUse = images[i].sprite;
                    images[i].sprite = value ? spritesToUse[i] : spriteGeneric;
                    //images[i].sprite = spriteToUse;
                }
                
            }
        );
    }
}
