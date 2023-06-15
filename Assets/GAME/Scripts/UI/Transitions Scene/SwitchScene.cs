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

    public SpriteRenderer _backGround;
    public float fadeDuration = 2f;
    private bool isFading = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFading && _backGround.color.a == 0) 
        {
            isFading = true;
            FadeIn();
        }

        if (Input.GetMouseButtonDown(1) && !isFading&& _backGround.color.a == 1)
        {
            isFading = true;
            FadeOut();
        }
    }

    private void FadeIn()
    {
        StartCoroutine(FadeRoutine(0f, 1f));
    }

    private void FadeOut()
    {
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha)
    {
        float currentTime = 0f;
        float fadeDuration = 2f; // Thời gian phai mờ

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);

            Color color = _backGround.color;
            color.a = alpha;
            _backGround.color = color;

            yield return null;
        }

        isFading = false;
    }

    private void Start()
    {
        ScaleBackground();
    }
    private void ScaleBackground()
    {
        // Lấy kích thước của camera
        Camera camera = Camera.main;
        float cameraHeight = camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * camera.aspect;

        // Lấy kích thước của sprite
        float spriteWidth = _backGround.sprite.bounds.size.x;
        float spriteHeight = _backGround.sprite.bounds.size.y;

        // Tính toán tỉ lệ phóng to theo chiều ngang và chiều dọc
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        // Chọn tỉ lệ phóng to lớn nhất để điền hết màn hình
        float scale = Mathf.Max(scaleX, scaleY);

        // Áp dụng tỉ lệ phóng to
        _backGround.transform.localScale = new Vector3(scale, scale, 1f);
    }
    
    
}
