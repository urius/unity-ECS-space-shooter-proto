using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FireByMouseSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var isMousePressed = Input.GetMouseButton(0);
        Entities.ForEach((Entity entity, ref FireByMouseComponent f, ref WeaponsControlComponent weaponsControl) =>
        {
            weaponsControl.IsFiring = isMousePressed;
        });
    }
}
