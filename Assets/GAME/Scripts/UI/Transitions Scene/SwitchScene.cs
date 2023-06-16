using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    #region Singleton Pattern

    private static SwitchScene instance;

    public static SwitchScene Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SwitchScene>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "BasePlayerController";
                    instance = obj.AddComponent<SwitchScene>();
                    Debug.Log("Khởi tạo mới instance");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public Animator _animator;
    public void FadeInGame()
    {
        _animator.SetBool("FadeIn",true);
    }
    
    public void FadeOutGame()
    {
        _animator.SetBool("FadeIn",false);
    }
}