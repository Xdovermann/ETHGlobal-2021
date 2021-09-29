using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MiniMapCamera : MonoBehaviour
{
    public static MiniMapCamera mapCamera;

    public RawImage MiniMapImage;
    public Transform CharacterSprite;

    private void Awake()
    {
        mapCamera = this;
    }

    public void MoveCamToTile(Vector2 pos)
    {
        CharacterSprite.DOMove(pos,0.5f);

        transform.DOMoveX(pos.x,0.1f);
        transform.DOMoveY(pos.y, 0.1f);
    }

  
}
