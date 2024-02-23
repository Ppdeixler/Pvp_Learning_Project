using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Main variables

    private CharacterController CC;
    [SerializeField] private Transform cam;

    //Rotation Variables

    private float mouseX;
    private float mouseY;
    [SerializeField] private float speedRotation;
    private Vector2 virar;

    //Movement Variables

    private float horizontal;
    private float vertical;
    public float speedMovement;

    //Dash Variables
    private bool canDash = true;
    private bool isDashing = false;
    public float speedDash;
    private int dashDir;

    public float frDash;
    public float dashCD;

    private Vector3 directionDash = default;

    //TP Variables

    private Vector3 thiscombo;
    private Vector3 thisglobal;

    public float velocidadeprafrente;

    private Renderer rend;

    private bool tptiming;

    [SerializeField] private GameObject tp;

    //Combat Variables;

    [SerializeField] private AttackSystem combat;
    private float stunnedSpeed = 1f;

    //Gravity Variables

    private float gravityVelocity;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float gravityFixed;


    void Awake()
    {
        rend = tp.GetComponent<Renderer>();
        CC = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        Rotation();

        if (tptiming) return;

        Gravity();
        Movement();

        if (combat.stunned) return;

        TP();

        if (combat.attacking) return;

        Dash();

    }

    void Gravity()
    {
            
        if(CC.isGrounded) gravityVelocity = -1f; else gravityVelocity += gravityMultiplier * gravityFixed * Time.deltaTime;

        CC.Move(new Vector3(0f, gravityVelocity, 0f) * Time.deltaTime);

    }

    void Dash()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine("DashCD");
        }

        if (!isDashing)
        {
            directionDash = new Vector3(horizontal, 0f, vertical);
        }

        float targetAngle = Mathf.Atan2(directionDash.x, directionDash.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if (isDashing) CC.Move(moveDir.normalized * speedDash * Time.deltaTime);

    }


    void Rotation()
    {
        mouseX += Input.GetAxisRaw("Mouse X");
        mouseY += Input.GetAxisRaw("Mouse Y");

        virar.x = mouseX;
        virar.y = mouseY;

        transform.localRotation = Quaternion.Euler(0f, virar.x, 0f);
    }

    void Movement()
    {

        if (combat.stunned) stunnedSpeed = 0.35f; else stunnedSpeed = 1f;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if(direction.magnitude >= 0.1f)
        {
            CC.Move(moveDir.normalized * speedMovement * stunnedSpeed * Time.deltaTime);
        }
    }

    void TP()
    {
        thiscombo = tp.transform.localPosition;
        thisglobal = tp.transform.position;

        if (combat.attacking)
        {
            rend.enabled = false;
            tp.gameObject.transform.localPosition = new Vector3(0f, -0.736f, 1.46f);
            return;
        }

        if (Input.GetMouseButton(1))
        {   

            tp.gameObject.transform.localPosition = new Vector3(thiscombo.x, thiscombo.y, thiscombo.z += 1f * Time.deltaTime * velocidadeprafrente);
            rend.enabled = true;
        }

        tp.transform.localPosition = new Vector3(thiscombo.x, thiscombo.y, Mathf.Clamp(tp.transform.localPosition.z, 1.46f, 10.46f));

        if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine("TPCooldown");
            this.transform.position = new Vector3(thisglobal.x, this.transform.position.y, thisglobal.z);
            tp.gameObject.transform.localPosition = new Vector3(0f, -0.736f, 1.46f);
            rend.enabled = false;
        }
    }

    private IEnumerator DashCD()
    {
        canDash = false;
        isDashing = true;

        yield return new WaitForSeconds(frDash);

        isDashing = false;

        yield return new WaitForSeconds(dashCD);

        canDash = true;
    }
    private IEnumerator TPCooldown()
    {
        tptiming = true;
        yield return new WaitForSeconds(0.03f);
        tptiming = false;
    }
}