using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 DistanceToTarget;
    public Vector3 LookAtOffset;
    [Range(0, 1)] public float LookAtTargetModifier = 0;
    public float movementSpeed;
    public float rotationSpeed;


    private void LateUpdate()
    {
        if (!Target) return;
        Vector3 targetPosition = Target.position;
        Vector3 cameraPosition = targetPosition + new Vector3(0, DistanceToTarget.y, 0);
        Vector3 focusPointOffset = new Vector3(0, LookAtOffset.y, 0);

        Vector3 targetForward = Target.forward;
        targetForward.y = 0;
        targetForward.Normalize();

        cameraPosition += targetForward * DistanceToTarget.z;
        focusPointOffset += targetForward * LookAtOffset.z;

        if(DistanceToTarget.x != 0 || LookAtOffset.x != 0)
        {
            Vector3 targetRight = Target.right;
            targetRight.y = 0;
            targetRight.Normalize();

            cameraPosition += targetRight * DistanceToTarget.x;
            focusPointOffset += targetRight * LookAtOffset.x;

        }

        Vector3 newPos = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * movementSpeed);
        transform.position = newPos;

        Quaternion cameraRotation;
        if (LookAtTargetModifier <= 0)
        {
            cameraRotation = Quaternion.LookRotation(targetForward);

        }
        else
        {
            Vector3 lookAtdirection = targetPosition + focusPointOffset - newPos;
            if (LookAtTargetModifier >= 1)
            {

                cameraRotation = Quaternion.LookRotation(lookAtdirection);


            }
            else
            {
                cameraRotation = Quaternion.LerpUnclamped(Quaternion.LookRotation(targetForward), Quaternion.LookRotation(lookAtdirection), LookAtTargetModifier);

            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, Time.deltaTime * rotationSpeed);
    }
}
