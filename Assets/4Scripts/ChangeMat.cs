using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMat : MonoBehaviour
{
    [SerializeField]
    public GameObject gameObj;

    [SerializeField]
    private Material[] materials;
    private MeshRenderer rend;
    int index = 0;

    void Start()
    {
        rend = gameObj.GetComponent<MeshRenderer>();
        rend.material = materials[index];
    }
    public void ChangeMatBtn()
    {
        index++;
        if (index >= materials.Length)
            index = 0;
        rend.material = materials[index];
    }
}
