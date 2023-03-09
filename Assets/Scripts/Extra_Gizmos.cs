using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Extra_Gizmos
{
    //Added by Daniel.

    public static void DrawCapsule(Vector3 position1, Vector3 position2, float radius, Color color = default)
    {
        if (color != default) Handles.color = color;

        //Direction of capsule
        Vector3 forward = position2 - position1;
        //Rotation based on capsule cirection.
        Quaternion rotation = Quaternion.LookRotation(forward);

        float pointOffset = radius / 2f;
        float length = forward.magnitude;
        var center2 = new Vector3(0f, 0, length);

        Matrix4x4 angleMatrix = Matrix4x4.TRS(position1, rotation, Handles.matrix.lossyScale);

        //Draws a capsule.
        using (new Handles.DrawingScope(angleMatrix))
        {
            Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
            Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, radius);
            Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, radius);
            Handles.DrawWireDisc(center2, Vector3.forward, radius);
            Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, radius);
            Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, radius);

            MakeLine(radius, 0f, length);
            MakeLine(-radius, 0f, length);
            MakeLine(0f, radius, length);
            MakeLine(0f, -radius, length);
        }
    }

    private static void MakeLine(float x, float y, float forward)
    {
        Handles.DrawLine(new Vector3(x, y, 0f), new Vector3(x, y, forward));
    }
}
