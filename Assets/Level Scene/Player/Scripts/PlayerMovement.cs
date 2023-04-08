
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerMovement : MonoBehaviour
{
    private Joystick _joy;
    private Transform _player;
    [SerializeField] public float speed;
    [HideInInspector] public Animator anim;
    public static PlayerMovement movement›nstance;

    private void Awake()
    {
        if (movement›nstance == null)
        {
            movement›nstance = this;
        }
    }
    private void Start()
    {
        _joy = Joystick.instance;
        _player = transform.GetChild(0);
        anim = _player.transform.GetComponent<Animator>();


    }

    public void Update()
    {
        if (_joy.Moved)
        {
            _player.forward = Time.deltaTime * speed * new Vector3(_joy.direction.x, 0, _joy.direction.y);
            transform.position += (Vector3.right * _joy.direction.x + Vector3.forward * _joy.direction.y) * (Time.deltaTime * speed);
            anim.SetBool("move", true);
        }
        else
        {
            anim.SetBool("move", false);

        }

        if(_joy.direction== Vector2.zero)
        {
            anim.SetBool("move", false);

        }

    }
}
