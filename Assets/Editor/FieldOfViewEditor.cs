using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AImovement))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        AImovement a� = (AImovement)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(a�.transform.position, Vector3.up, Vector3.forward, 360, a�.radius);

        /* Vector3 viewAngle01 = DirectionFromAngle(a�.transform.eulerAngles.y, -a�.angle / 2);
         Vector3 viewAngle02 = DirectionFromAngle(a�.transform.eulerAngles.y, a�.angle / 2);

         Handles.color = Color.yellow;
         Handles.DrawLine(a�.transform.position, a�.transform.position + viewAngle01 * a�.radius);
         Handles.DrawLine(a�.transform.position, a�.transform.position + viewAngle02 * a�.radius);*/

        if (a�.canSeePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(a�.transform.position, a�.playerRef.transform.position);
        }
    }


    /*private Vector3 DirectionFromAngle(float eulerY, float angleInDegress)
    {
        angleInDegress += eulerY;

        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }
    */

}
