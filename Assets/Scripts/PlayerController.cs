using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputAction;

    public float Speed = 4.0f;

    private Rigidbody2D m_Rigidbody;

    private InputAction m_MoveAction;

    private Vector2 m_CurrentLookDirection;

    public SpriteRenderer Body;

    void Awake()
    {


        m_Rigidbody = GetComponent<Rigidbody2D>();

        gameObject.transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        m_CurrentLookDirection = Vector2.right;

        m_MoveAction = InputAction.FindAction("Gameplay/Move");
        m_MoveAction.Enable();

        Body = transform.Find("Visual/0").GetComponent<SpriteRenderer>();

        Body.flipX = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        var move = m_MoveAction.ReadValue<Vector2>();

        //note: == and != for vector2 is overriden to take in account floating point imprecision.
        if (move != Vector2.zero)
        {
            SetLookDirectionFrom(move);
        }


        var movement = move * Speed;
        var speed = movement.sqrMagnitude;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement * Time.deltaTime);
    }

    void SetLookDirectionFrom(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            m_CurrentLookDirection = direction.x > 0 ? Vector2.right : Vector2.left;

            if (direction.x > 0)
            {
                // transform.localScale = new Vector3(1, 1, 1);
                Body.flipX = true;
            }
            else
            {
                // transform.localScale = new Vector3(-1, 1, 1);
                Body.flipX = false;
            }
        }
    }
}
