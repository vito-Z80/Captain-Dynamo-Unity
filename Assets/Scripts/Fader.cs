using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.U2D;

public class Fader : MonoBehaviour
{
    [SerializeField] public PixelPerfectCamera camera;

    private Texture2D _texture2D;
    private Sprite _sprite;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _texture2D = new Texture2D(camera.refResolutionX, camera.refResolutionY,DefaultFormat.LDR,TextureCreationFlags.None);
        Draw();
        _sprite = Sprite.Create(_texture2D, new Rect(0, 0, camera.refResolutionX, camera.refResolutionY),
            new Vector2(0.5f,0.5f));
        _spriteRenderer.sprite = _sprite;
    }


    private void Draw()
    {
        for (int y = 0; y < camera.refResolutionY; y++)
        {
            for (int x = 0; x < camera.refResolutionX; x++)
            {
                if (x/10 == 1)
                {
                    _texture2D.SetPixel(x, y, Color.red);
                }
            }
        }
        _texture2D.Apply();
    }
}