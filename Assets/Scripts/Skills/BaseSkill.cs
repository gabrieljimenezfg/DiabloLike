using System;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void StartCast()
    {
        gameObject.SetActive(true);
    }
}
