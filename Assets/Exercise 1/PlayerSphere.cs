using UnityEngine;

public class PlayerSphere : MonoBehaviour
{
    [SerializeField] private AudioClip sfxJump;
    
    private Rigidbody _body;
    private AudioSource _sfxPlayer;
    
    private float _velocity = 3f;
    private float _moveForce = 20f;
    private float _jumpForce = 130f;

    private bool _hasGroundContact = false;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _sfxPlayer = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (_hasGroundContact)
        {
            _body.velocity = new Vector3(0, 0, _velocity);
        }

        ProcessPlayerMoves();
    }

    private void ProcessPlayerMoves()
    {
        var moveForce = Input.GetAxis("Horizontal") * _moveForce;
        _body.velocity = new Vector3(moveForce, _body.velocity.y, _body.velocity.z);

        if (Input.GetButtonDown("Jump") && _hasGroundContact)
        {
            _body.AddForce(new Vector3(0, _jumpForce, 0));
            _hasGroundContact = false;
            _sfxPlayer.PlayOneShot(sfxJump, 0.2f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Ground"))
        {
            _hasGroundContact = true;
        }
    }
}