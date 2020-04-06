using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WorldBounds
{
    private static Rect _worldBoundsRect;

    public static Rect Bounds => _worldBoundsRect.width == 0 ? CreateRect() : _worldBoundsRect;

    private static Rect CreateRect()
    {
        var camera = Camera.main;
        var topRightPos = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, math.abs(camera.transform.position.z)));
        var bottomLeftPos = camera.ScreenToWorldPoint(new Vector3(0, 0, math.abs(camera.transform.position.z)));
        _worldBoundsRect.Set(bottomLeftPos.x, bottomLeftPos.y, topRightPos.x - bottomLeftPos.x, topRightPos.y - bottomLeftPos.y);

        return _worldBoundsRect;
    }
}
