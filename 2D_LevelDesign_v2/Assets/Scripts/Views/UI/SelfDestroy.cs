﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    void RemoveGameObject()
    {
        Destroy(gameObject);
    }
}
