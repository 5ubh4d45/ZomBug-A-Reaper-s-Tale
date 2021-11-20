using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollderController : MonoBehaviour
{
    private TilemapRenderer _tilemapRenderer;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();

        _tilemapRenderer.enabled = false;
    }

}
