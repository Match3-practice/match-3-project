using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardResizer : MonoBehaviour
{
    private Board _board;
    private GridLayoutGroup _gridLayout;
    void Start()
    {
        _board = GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
