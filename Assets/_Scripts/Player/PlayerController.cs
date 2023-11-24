using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action OnProjectileShot;
    public event Action OnProjectilesDepleted;
    public int RemainingProjectiles => remainingProjectiles;
    public bool ProjectilesInScene => GameObject.FindGameObjectsWithTag("Projectile").Length > 0;

    [Header("Projectile Stats")]
    [SerializeField] private float minShotPower = 1.0f;
    [SerializeField] private float maxShotPower = 25.0f;
    [SerializeField] private float chargeRate = 5.0f;
    [SerializeField] private int maxProjectiles = 15;
    private int remainingProjectiles;
    private float currentShotPower;
    private bool isCharging;

    [Header("Aimer and Projectile")]
    [SerializeField] private GameObject aimer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileSpawnPoint;
    private Vector2 worldPosition;
    private Vector2 direction;

    [Header("UI Manager")]
    [SerializeField] private MenuManager mm;


    private void Start()
    {
        currentShotPower = minShotPower;
        remainingProjectiles = maxProjectiles;
        mm.UpdateProjectiles(remainingProjectiles);
    }

    private void Update()
    {
        HandleAimerRotation();
        HandleProjectilePower();
        CheckProjectilesDepleted();
    }

    private void CheckProjectilesDepleted()
    {
        if (remainingProjectiles <= 0 && !ProjectilesInScene)
        {
            OnProjectilesDepleted?.Invoke();
        }
    }

    private void HandleProjectilePower()
    {
        if (isCharging)
        {
            currentShotPower += chargeRate * Time.deltaTime;
            currentShotPower = Mathf.Clamp(currentShotPower, minShotPower, maxShotPower);
            mm.SetPowerGauge(currentShotPower, minShotPower, maxShotPower);
        }
    }

    private void HandleAimerRotation()
    {
        // Rotate the aimer to face the mouse position
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)aimer.transform.position).normalized;
        aimer.transform.right = direction;
    }

    public void StartCharging()
    {
        isCharging = true;
    }

    public void HandleShooting()
    {
        if (remainingProjectiles <= 0) return;

        isCharging = false;

        // Spawn and shoot the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, aimer.transform.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetSpeed(currentShotPower);
        }
        remainingProjectiles--;
        mm.UpdateProjectiles(remainingProjectiles);
        OnProjectileShot?.Invoke();

        // Reset shot power
        currentShotPower = minShotPower;
    }
}
