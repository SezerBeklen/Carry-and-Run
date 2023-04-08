using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlueAIMove))]
public class FOVBlueAı : Editor
{
    private void OnSceneGUI()
    {
        BlueAIMove blue = (BlueAIMove)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(blue.transform.position, Vector3.up, Vector3.forward, 360, blue.radius);

        /* Vector3 viewAngle01 = DirectionFromAngle(aı.transform.eulerAngles.y, -aı.angle / 2);
         Vector3 viewAngle02 = DirectionFromAngle(aı.transform.eulerAngles.y, aı.angle / 2);

         Handles.color = Color.yellow;
         Handles.DrawLine(aı.transform.position, aı.transform.position + viewAngle01 * aı.radius);
         Handles.DrawLine(aı.transform.position, aı.transform.position + viewAngle02 * aı.radius);*/

        if (blue.Blue_canSeePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(blue.transform.position, blue.playerRef.transform.position);
        }
    }

}
