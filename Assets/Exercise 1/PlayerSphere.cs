using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSphere : MonoBehaviour
{
    [SerializeField] private Text playTimeText;
    [SerializeField] private Text bestTimeText;
    
    [SerializeField] private AudioClip sfxJump;
    
    private Rigidbody _body;
    private AudioSource _sfxPlayer;
    
    private float _forwardVelocity = 3f;
    private float _horizontalVelocity = 5f;
    private float _jumpForce = 130f;

    private bool _hasGroundContact = false;
    private bool _reachedFinish = false;

    private float _playTime = 0f;
    private float _bestTime;

    private const string BestTimeKey = "bestTime";
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _sfxPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _bestTime = PlayerPrefs.GetFloat(BestTimeKey, -1f);

        playTimeText.text = $"{_playTime:N2}";
        bestTimeText.text = _bestTime > 0 ? $"{_bestTime:N2}" : "--";
    }

    private void Update()
    {
        if (!_reachedFinish)
        {
            _playTime += Time.deltaTime;
            playTimeText.text = $"{_playTime:N2}";
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            _forwardVelocity = 6f;
        }
        else
        {
            _forwardVelocity = 3f;
        }
    }

    private void FixedUpdate()
    {
        var velocity = _body.velocity;
        velocity.z = _forwardVelocity;
        _body.velocity = velocity;

        ProcessPlayerMoves();
        CheckFallingOffPlatform();
    }

    private void ProcessPlayerMoves()
    {
        var move = Input.GetAxis("Horizontal") * _horizontalVelocity;
        var velocity = _body.velocity;
        velocity.x = move;
        _body.velocity = velocity;

        if (Input.GetButtonDown("Jump") && _hasGroundContact)
        {
            _body.AddForce(new Vector3(0, _jumpForce, 0));
            _hasGroundContact = false;
            _sfxPlayer.PlayOneShot(sfxJump, 0.2f);
        }
    }

    private void CheckFallingOffPlatform()
    {
        if (transform.position.y < -4)
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        _hasGroundContact = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Finish"))
        {
            _reachedFinish = true;

            if (_playTime < _bestTime || _bestTime < 0)
            {
                _bestTime = _playTime;
                bestTimeText.text = $"{_bestTime:N2}";
                PlayerPrefs.SetFloat(BestTimeKey, _bestTime);
            }
        }
    }
}