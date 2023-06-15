using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    public GameObject prefab; // Prefab bạn muốn sinh ra
    public Transform content; // Object Content để chứa các Prefab
    private void Start()
    {
        for (int i = 1; i <= 40; i++)
        {
            GameObject instance = Instantiate(prefab, content);
            Text textComponent = instance.GetComponentInChildren<Text>();
            instance.GetComponentInChildren<Level>()._indexLevel = i;//gán level cho button
            textComponent.text = i.ToString();
        }
    }

}
