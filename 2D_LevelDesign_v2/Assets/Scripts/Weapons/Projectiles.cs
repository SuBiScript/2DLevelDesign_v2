using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class Projectiles : MonoBehaviour
{
    [Header("Projectiles Settings")]
    public string enemyTag;
    public int damage;

    [HideInInspector] public Vector2 direction;

    public virtual void Disable()
    {
        this.gameObject.SetActive(false);
    }
}