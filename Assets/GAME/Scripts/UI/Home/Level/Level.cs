using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : BaseButton
{
    public int _indexLevel;
    public bool isPass;
    public int _countStar;
    protected override void OnPress()
    {
        //Debug.Log("Lấy data từ đây");
        Debug.Log($"Level {_indexLevel}");
        SceneManager.LoadScene(_indexLevel);
    }

    protected override void OnRelease()
    {
    }
}
