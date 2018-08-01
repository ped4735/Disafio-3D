using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public int lives = 3;
    public float moveSpeed = 5;
    public float rotationSpeed = 5;
    public float jumpForce = 5;

    [HideInInspector]
    public bool disableRotation = false;

    public float invincibilityTime = 2;

    public bool thirdPerson = true;

    public Material damage;
    Material def;

    public Follow camera;

    public float heightToDeath = -20;

    public GUIManager gui;

    Rigidbody body;

    float distToGround;

    Quaternion defaultRotation;
    Vector3 defaultPosition;

    float invincibility = 0;

    Renderer renderer;

    float originalDist;

    AudioSource audio;

    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public bool won = false;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        body = GetComponent<Rigidbody>();

        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;

        defaultRotation = transform.rotation;
        defaultPosition = transform.position;

        renderer = GetComponent<Renderer>();
        def = renderer.material;

        originalDist = transform.GetChild(1).GetChild(0).localPosition.z;
        SetController(thirdPerson);

        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        //SetController(thirdPerson);
        // Movement
        float horiz = Input.GetAxis("Horizontal");
        float vert  = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horiz, vert);
        move.Normalize();
        move *= Time.deltaTime * moveSpeed;
        transform.Translate(move.y, 0, -move.x);

        // Rotation
        if (!disableRotation) {
            horiz = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime * 100;
            vert = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime * 100;
            Transform holder = transform.GetChild(1);

            transform.Rotate(transform.up, horiz);

            Quaternion quat = holder.localRotation;
            float r = quat.eulerAngles.x - vert;
            if (r > 180) {
                r -= 360;
            }
            float rotat = Mathf.Clamp(r, thirdPerson ? 0 : -90, 90);
            quat.eulerAngles = new Vector3(rotat, 90, 0);
            holder.localRotation = quat;
        }
        else {
            transform.rotation = defaultRotation;
        }

        // Cursor
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(!dead && !won && Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Jumping
        if (Input.GetAxis("Jump") == 1 && Mathf.Abs(body.velocity.y) <= 0.1 && IsGrounded()) {
            SetVelY(jumpForce);
            audio.Play();
        }
        if(Mathf.Abs(body.velocity.y) > jumpForce) {
            SetVelY(jumpForce * Mathf.Sign(body.velocity.y));
        }

        // Damaging
        if (invincibility > 0) {
            invincibility -= Time.deltaTime;
            if(((invincibility * 10) % 2) > 1) {
                renderer.material = damage;
            } else {
                renderer.material = def;
            }
        }

        // Resetting
        if (Input.GetKeyDown(KeyCode.R)) {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
            body.velocity = Vector3.zero;
        }

        // Check death
        if(transform.position.y < heightToDeath) {
            Die();
        }
	}

    public void Reset() {
        lives = 3;
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
        body.velocity = Vector3.zero;
        SetController(true);
        dead = false;
    }

    public void SetController(bool b) {
        thirdPerson = b;
        if(thirdPerson) {
            Debug.Log(originalDist);
            transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, 0, originalDist);
            SetLayerRecursevely(transform.GetChild(0).gameObject, LayerMask.NameToLayer("Default"));
            SetLayerRecursevely(transform.GetChild(2).gameObject, LayerMask.NameToLayer("Default"));
            camera.speed = 8;
        } else {
            transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, 0, 0);
            SetLayerRecursevely(transform.GetChild(0).gameObject, LayerMask.NameToLayer("Invisible"));
            SetLayerRecursevely(transform.GetChild(2).gameObject, LayerMask.NameToLayer("Invisible"));
            camera.speed = 20;
        }
    }

    void SetLayerRecursevely(GameObject obj, int layer) {
        obj.layer = layer;
        foreach (Transform child in obj.transform) {
            SetLayerRecursevely(child.gameObject, layer);
        }
    }

    void SetVelY(float n) {
        body.velocity = new Vector3(0, n, 0);
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    public void Damage(int damage) {
        if (invincibility <= 0 && lives > 0) {
            invincibility = invincibilityTime;
            lives -= damage;

            Debug.Log("Lives: " + lives);

            if (lives <= 0) {
                Die();
            }
        }
    }

    void Die() {
        gui.Lose();
        dead = true;
    }
}
