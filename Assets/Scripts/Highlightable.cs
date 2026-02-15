using System;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponentInChildren<Renderer>();
    }
}
