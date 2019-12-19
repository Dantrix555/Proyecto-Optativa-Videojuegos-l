using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _speed;
    public float _characterScale;

    void Start()
    {
        //Create shot and assign it a velocity
        _speed = 12;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = new Vector2(_characterScale * _speed, _rigidbody2D.velocity.y);
        Destroy(gameObject, 5);
    }
}
