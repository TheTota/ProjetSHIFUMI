﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the AI of range units during a units fight.
/// Range units work in the following way : 
/// - Unit gets attributed a target when instantiated
/// - Unit moves towards a shooting position (archers can shoot from further than mages)
/// - Unit waits a bit to "prepare weapon"
/// - Unit shoots periodically a projectile forward by instantiating a prefab that has the UnitProjectile script
/// - Projectile effects are defined in the UnitProjectile script
/// </summary>
public class RangeUnitAI : UnitAI
{
    [Header("Projectiles prefab")]
    public GameObject arrowPrefab;
    public GameObject fireballPrefab;

    // movement vars
    private Vector2 rangeUnitTargetPos;
    private float offsetX;
    private const float MAGES_OFFSET_X = 7f;
    private const float ARCHERS_OFFSET_X = 4f;

    // positionning / more move vars
    private bool isInShootingPosition;
    private bool goingRight;

    private void Start()
    {
        // sets how far the unit will have to go to be in shooting position
        if (ut == UnitType.Mages)
            offsetX = MAGES_OFFSET_X;
        else if (ut == UnitType.Archers)
            offsetX = ARCHERS_OFFSET_X;

        SetShootingPositionTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInShootingPosition && base.IsAlive) // check to only start the ShootTarget coroutine once
        {
            // if we haven't reached target pos, keep moving
            if (transform.position.x != rangeUnitTargetPos.x)
            {
                Move();
            }
            else // once target pos is reached, start the target shooting coroutine 
            {
                if (DemoMode)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    StartCoroutine(ShootTarget(1.25f));
                    isInShootingPosition = true;
                }
            }
        }
    }

    /// <summary>
    /// Sets an actual vector2 position that the unit will have to move towards to be ready to shoot.
    /// </summary>
    private void SetShootingPositionTarget()
    {
        if (base.DemoMode)
        {
            rangeUnitTargetPos = base.targetPos;
        }
        else
        {
            isInShootingPosition = false;

            Camera mainCam = Camera.main;
            float targetXPos;

            // going to the right
            if (mainCam.WorldToScreenPoint(this.transform.position).x < 0f)
            {
                targetXPos = this.transform.position.x + offsetX;
                goingRight = true;
            }
            else // going to the left
            {
                targetXPos = this.transform.position.x - offsetX;
                goingRight = false;
            }

            rangeUnitTargetPos = new Vector2(targetXPos, this.transform.position.y);
        }
    }

    /// <summary>
    /// Moves the unit towards a target position, while apply a "hop" effect.
    /// </summary>
    private void Move()
    {
        // Move our position a step closer to the target.
        float step = base.speed * Time.deltaTime; // calculate distance to move
        Vector3 pos = Vector2.MoveTowards(transform.position, rangeUnitTargetPos, step);
        transform.position = pos + transform.up * Mathf.Sin(Time.time * base.hopFrequency) * base.hopMagnitude;
    }

    /// <summary>
    /// Waits a bit and handles the periodic shooting.
    /// </summary>
    /// <param name="delayBetweenShots"></param>
    /// <returns></returns>
    private IEnumerator ShootTarget(float delayBetweenShots)
    {
        yield return new WaitForSeconds(Random.Range(.5f,1f));
        while (this.targetEnemy.IsAlive && base.IsAlive) // if we're alive and enemy target is alive
        {
            Shoot();
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    /// <summary>
    /// Shoots a projectile foward (direction of which the unit is facing) by instantiating
    /// a prefab that contains a script that will handle the projectile movement and collision.
    /// </summary>
    private void Shoot()
    {
        GameObject proj;

        // set rotation of the projectile on instantiation
        Quaternion rot;
        if (goingRight)
        {
            rot = Quaternion.Euler(new Vector3(0f, 0f, 180f));
        }
        else
        {
            rot = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        // instantiate projectile
        if (ut == UnitType.Archers) // is archer => instantiate arrow prefab
        {
            proj = Instantiate(arrowPrefab, this.transform.position, rot);
        }
        else // is mage => instantiate fireball prefab
        {
            proj = Instantiate(fireballPrefab, this.transform.position, rot);
        }

        // tells the projectile if a mage or archer shot it (useful to makes behaviours differ a bit)
        proj.GetComponent<UnitProjectile>().ProjectileOrigin = ut;
    }
}
