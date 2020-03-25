using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : Singleton<ResourceManager>
{
    public SpriteAtlas spriteAtlas;

    public void LoadAtlas()
    {
        spriteAtlas = Resources.Load<SpriteAtlas>("Images/Tiles");
    }
}
