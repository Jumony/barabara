using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public Transform pivotPoint;
    public bool facingRight;

    private void Start()
    {
        pivotPoint = GameObject.Find("Pivot").GetComponent<Transform>();    
    }

    public void Rotate(Vector3 mousePos)
    {
        Vector2 direction = mousePos - pivotPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 offsetFromPivot = transform.position - pivotPoint.position;
        float offsetDistance = offsetFromPivot.magnitude;

        // Rotate the weapon around the pivot point
        transform.position = pivotPoint.position + Quaternion.Euler(0, 0, angle) * new Vector3(offsetDistance, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // I SHOULD NEVER HAVE TOUCHED QUATERNIONS. I SHOULDVE JUST FLIPPED THE PLAYER BY -1 SCALE
    public void HandleWeaponFlipping(float currentAngle, float rotationThreshold)
    {
        if (facingRight && Mathf.Abs(currentAngle) > rotationThreshold)
        {
            FlipLeft(currentAngle);
        }
        else if (!facingRight && Mathf.Abs(currentAngle) <= rotationThreshold)
        {
            FlipRight(currentAngle);
        }
    }

    public void AdjustWeaponRotation(float currentAngle, float rotationThreshold)
    {
        if (Mathf.Abs(currentAngle) > rotationThreshold)
        {
            transform.localRotation = Quaternion.Euler(180, 180, -currentAngle);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }

    void FlipLeft(float currentAngle)
    {
        transform.parent.rotation = Quaternion.Euler(0, 180, 0);
        transform.localRotation = Quaternion.Euler(180, 180, -currentAngle);
        facingRight = !facingRight;
    }

    void FlipRight(float currentAngle)
    {
        transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        facingRight = !facingRight;
    }
}
