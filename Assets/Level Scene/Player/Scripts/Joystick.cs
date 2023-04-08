using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public RectTransform center;
    public RectTransform knob;
    public float range;
    public bool fixedJoystic;
    [HideInInspector] public bool Moved = false;

    [HideInInspector] public Vector2 direction;
    public static Joystick instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        
        if (GameManager._instance.gamestate == GameManager.GameState.Start)
        {
            ShowHide(false);
        }
        if(GameManager._instance.gamestate == GameManager.GameState.Ingame)
        {
            Vector2 pos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                center.position = pos;
                knob.position = pos;
                ShowHide(true);
                Moved = true;
            }
            else if (Input.GetMouseButton(0))
            {
                knob.position = pos;
                knob.position = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * range);

                if (knob.position != Input.mousePosition && !fixedJoystic)
                {
                    Vector3 outsideBoundsVector = Input.mousePosition - knob.position;
                    center.position += outsideBoundsVector;
                }
                direction = (knob.position - center.position).normalized;
            }
            else
            {
                Moved = false;
                direction = Vector2.zero;
                ShowHide(false);
            }

        }
       
    }


    void ShowHide(bool state)
    {
        center.gameObject.SetActive(state);
        knob.gameObject.SetActive(state);
    }
}
