﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}