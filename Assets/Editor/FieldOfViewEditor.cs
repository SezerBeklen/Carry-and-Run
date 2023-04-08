using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AImovement))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        AImovement aý = (AImovement)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(aý.transform.position, Vector3.up, Vector3.forward, 360, aý.radius);

        /* Vector3 viewAngle01 = DirectionFromAngle(aý.transform.eulerAngles.y, -aý.angle / 2);
         Vector3 viewAngle02 = DirectionFromAngle(aý.transform.eulerAngles.y, aý.angle / 2);

         Handles.color = Color.yellow;
         Handles.DrawLine(aý.transform.position, aý.transform.position + viewAngle01 * aý.radius);
         Handles.DrawLine(aý.transform.position, aý.transform.position + viewAngle02 * aý.radius);*/

        if (aý.canSeePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(aý.transform.position, aý.playerRef.transform.position);
        }
    }


    /*private Vector3 DirectionFromAngle(float eulerY, float angleInDegress)
    {
        angleInDegress += eulerY;

        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }
    */

}
