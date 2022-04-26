using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrefabScript : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private TMP_Text tmpText;

    public void FillImages(List<Sprite> sprites)
    {
        for (var i = 0; i < Mathf.Min(images.Count, sprites.Count); i++)
            images[i].sprite = sprites[i];
    }

    public void FillText(string text)
    {
        tmpText.text = text;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
    
}
