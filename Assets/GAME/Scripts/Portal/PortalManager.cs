using System;
using System.Collections;
using System.Collections.Generic;
using Ring;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    #region Singleton pattern

    private static PortalManager instance;

    public static PortalManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    #endregion

    public Portal_Spawn _portalSpawn;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}