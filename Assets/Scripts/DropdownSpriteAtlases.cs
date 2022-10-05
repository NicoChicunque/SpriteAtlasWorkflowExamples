using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DropdownSpriteAtlases : MonoBehaviour
{
    [SerializeField] private List<SpriteAtlas> spriteAtlases;
    [SerializeField] private List<Image> images = new List<Image>();    

    void Start()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(
            (indexAtlas) =>
            {
                for (int i = 0; i < images.Count; i++)
                {
                    string spriteName = images[i].sprite.name;
                    Sprite spriteToUse = spriteAtlases[indexAtlas].GetSprite(spriteName);
                    spriteToUse.name = spriteName;
                    images[i].sprite = spriteToUse;
                }
            }
        );
    }
}
