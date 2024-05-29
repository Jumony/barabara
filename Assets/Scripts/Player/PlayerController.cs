using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    Vector3 mousePos;

    public float speed;
    public float flipThresholdRadius;

    public Rigidbody2D rb;

    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    public GameObject currentWeapon;
    public GameObject defaultFists;

    private Animator animator;

    public WeaponRotation weaponRotation;

    public List<GameObject> weapons = new List<GameObject>();
    private int currentWeaponIndex = 0;

    enum Direction { Up, Right, Down, Left };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (weapons.Count == 0)
        {
            SpawnWithFist(defaultFists);
        }

        if (weapons.Count > 0)
        {
            ActivateWeapon(currentWeaponIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
        }
    }

    void FixedUpdate()
    {
        // Calculates the angle from the player to the mouse position
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        rb.velocity = moveDirection * speed * Time.deltaTime;

        Vector2 lookDirection = mousePos - weaponRotation.gameObject.transform.position;


        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        float distanceToPlayer = Vector2.Distance(mousePos, transform.position);
        bool withinFlipThreshold = distanceToPlayer < flipThresholdRadius;

        if (withinFlipThreshold)
        {
            weaponRotation.Rotate(mousePos);
        }

        else
        {
            weaponRotation.Rotate(mousePos);

            //Debug.Log(angle);

            float threshold = 0;

            if (weaponRotation.facingRight)
            {
                threshold = 130;
            }

            else if (!weaponRotation.facingRight)
            {
                threshold = 60;
            }

            weaponRotation.HandleWeaponFlipping(angle, threshold);

            weaponRotation.AdjustWeaponRotation(angle, threshold);
        }
        FindQuadrant(angle);
    }

    Direction FindQuadrant(float angle)
    {
        Direction directionToFace;

        if (angle >= 45 && angle < 135)
        {
            directionToFace = Direction.Up;
            animator.SetFloat("MouseHorizontal", 0);
            animator.SetFloat("MouseVertical", 1);
            return directionToFace;
        }

        else if (angle >= -45 && angle < 45)
        {
            directionToFace = Direction.Right;
            animator.SetFloat("MouseHorizontal", 1);
            animator.SetFloat("MouseVertical", 0);
            return directionToFace;
        }

        else if (angle >= -135 && angle < -45)
        {
            directionToFace = Direction.Down;
            animator.SetFloat("MouseHorizontal", 0);
            animator.SetFloat("MouseVertical", -1);
            return directionToFace;
        }

        else
        {
            directionToFace = Direction.Right;
            animator.SetFloat("MouseHorizontal", -1);
            animator.SetFloat("MouseVertical", 0);
            return directionToFace;
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position * Vector2.right * 3, transform.rotation);
    }

    public void PickUpWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        GameObject newWeapon = Instantiate(weaponPrefab);
        newWeapon.transform.SetParent(gameObject.transform, false);
        newWeapon.SetActive(false);
        weapons.Add(newWeapon);

        currentWeaponIndex = weapons.Count - 1;
        ActivateWeapon(currentWeaponIndex);
    }

    private void SpawnWithFist(GameObject fistsPrefab)
    {
        GameObject fist = Instantiate(fistsPrefab);
        fist.transform.SetParent(gameObject.transform, false);
        weapons.Add(fist);
        currentWeaponIndex = weapons.Count - 1;
        ActivateWeapon(currentWeaponIndex);
    }

    void SwitchWeapon()
    {
        if (weapons.Count == 0) return;
        currentWeapon.SetActive(false);
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;

        ActivateWeapon(currentWeaponIndex);
    }

    void ActivateWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;
        currentWeapon = weapons[index];
        currentWeapon.SetActive(true);
        weaponRotation = currentWeapon.GetComponent<WeaponRotation>();
    }
}
