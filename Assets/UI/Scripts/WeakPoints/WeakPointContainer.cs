using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class WeakPointContainer : MonoBehaviour
{
    public static WeakPointContainer Instance
    {
        get { return instance; }
    }

    private static WeakPointContainer instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private WeakPoint weakPointPrefab;

    private RectTransform rectTransform;
    private readonly Dictionary<Collider, WeakPoint> weakPointMap = new();

    public WeakPoint GetWeakPoint(Collider collider)
    {
        if(!weakPointMap.ContainsKey(collider)) { return null; }

        return weakPointMap[collider];
    }

    private void Awake()
    {
        if (instance) throw new Exception("WeakPoinContainer instance already exists.");
        instance = this;
        rectTransform = GetComponentInChildren<RectTransform>();
    }

    public void Add(Collider collider, bool isColorChange = false)
    {
        if (weakPointMap.ContainsKey(collider)) { return; }

        var weakPoint = Instantiate(weakPointPrefab, transform);
        weakPoint.Initialize(rectTransform, mainCamera, collider, isColorChange);
        weakPointMap.Add(collider, weakPoint);
    }

    public void Remove(Collider collider)
    {
        Destroy(weakPointMap[collider].gameObject);
        weakPointMap.Remove(collider);
    }

    public void AllRemove()
    {
        foreach(WeakPoint weakPoint in weakPointMap.Values)
        {
            Destroy(weakPoint.gameObject);
        }

        weakPointMap.Clear();
    }
}
