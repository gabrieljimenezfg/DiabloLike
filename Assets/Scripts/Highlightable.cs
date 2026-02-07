using System;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    public MeshRenderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponentInChildren<MeshRenderer>();
    }
}
