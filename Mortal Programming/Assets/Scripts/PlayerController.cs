using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private string _buttonInput;
    private string _actualCharacter;
    private bool _isGrounded;
    private Transform _groundPoint;
    private Transform _shotPoint;
    private int _health;
    private bool _invincible;
    private bool _canShot;

    public float _speed;
    public float _jumpForce;
    public bool _zeroSelected;
    public bool _xSelected;
    public LayerMask _groundLayer;
    public GameObject _shot;
    public GameController _controller;
    public GameObject _healthBar;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundPoint = transform.GetChild(0);
        _invincible = false;
        _health = 8;
        //If zero is selected we set the zero controllers and ignore everything related with shot (variables)
        if (_zeroSelected)
        {
            _buttonInput = "ZeroHorizontal";
            _actualCharacter = "Zero";
            _shotPoint = null;
            _shot = null;
            _canShot = false;
        }
        //If x is selected then can shot is set it as true
        else
        {
            _buttonInput = "XHorizontal";
            _actualCharacter = "X";
            _shotPoint = transform.GetChild(1);
            _canShot = true;
        }
    }

    void Update()
    {
        Movement(_buttonInput);
        CharacterGround();
        Attack();

        //Uodate the health bar gameObject scale
        _healthBar.transform.localScale = new Vector3(_health, 0.4f, 1);

        //Determines if the there's a winner
        if(_health < 1 && _actualCharacter.Equals("X"))
        {
            GameOver("Zero");
        }
        else if(_health < 1 && _actualCharacter.Equals("Zero"))
        {
            GameOver("X");
        }
    }

    void Movement(string _inputButton)
    {
        //Horizontal Move According to the character input
        if(Input.GetAxisRaw(_inputButton) != 0)
            transform.localScale = new Vector2(Input.GetAxisRaw(_inputButton), transform.localScale.y);
        _rigidbody.velocity = new Vector2(_speed * Input.GetAxisRaw(_inputButton), _rigidbody.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(_speed * Input.GetAxisRaw(_inputButton)));

        //The character jumps depending who is selected and its button
        if((_actualCharacter.Equals("Zero") && Input.GetKeyDown(KeyCode.O) || 
            _actualCharacter.Equals("X") && Input.GetKeyDown(KeyCode.N)) && _isGrounded)
                _rigidbody.AddForce(new Vector2(0, _jumpForce));
    }

    //Method to check if the character is grounded or not
    void CharacterGround()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundPoint.position, 0.2f, _groundLayer);
        _animator.SetBool("Jump", !_isGrounded);
    }

    void Attack()
    {
        //Detect if character is attacking
        if(((Input.GetKeyDown(KeyCode.P) && _actualCharacter.Equals("Zero")) 
            || (Input.GetKeyDown(KeyCode.M) && _actualCharacter.Equals("X") && _canShot)))
        {
            _animator.SetTrigger("Attack");
            //If character is X, instatiate a shot
            if(_actualCharacter.Equals("X") && _canShot)
            {
                GameObject _actualShot;
                _actualShot = Instantiate(_shot, _shotPoint.position, Quaternion.identity);
                _actualShot.GetComponent<Shot>()._characterScale = transform.localScale.x;
                _canShot = false;
                Invoke("CanShot", 0.75f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        Color _damagedColor;
        //Detect who is taking damage and from what is taking damage
        if(_other.gameObject.tag == "Slash" && _actualCharacter.Equals("X") && !_invincible)
        {
            _health--;
            _invincible = true;
            _damagedColor = new Color(1,1,1,0.5f);
            GetComponent<SpriteRenderer>().color = _damagedColor;
        }
        else if(_other.gameObject.tag == "Shot" && _actualCharacter.Equals("Zero") && !_invincible)
        {
            Destroy(_other.gameObject);
            _health--;
            _invincible = true;
            _damagedColor = new Color(1, 1, 1, 0.5f);
            GetComponent<SpriteRenderer>().color = _damagedColor;
        }

        Invoke("FinishInvinvible", 1);
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        Color _damagedColor;
        if ((_other.gameObject.name == "Zero" && _actualCharacter.Equals("X"))
            || (_other.gameObject.name == "X" && _actualCharacter.Equals("Zero")))
        {
            _health--;
            _invincible = true;
            _damagedColor = new Color(1, 1, 1, 0.5f);
            GetComponent<SpriteRenderer>().color = _damagedColor;
            Invoke("FinishInvinvible", 1);
        }
    }

    void FinishInvinvible()
    {
        _invincible = false;
        Color _normalColor = new Color(1, 1, 1, 1);
        GetComponent<SpriteRenderer>().color = _normalColor;
    }

    void CanShot()
    {
        _canShot = true;
    }

    //Game Over function with the name of the winner
    void GameOver(string _winner)
    {
        _controller.GameOver(_winner);
    }
}
