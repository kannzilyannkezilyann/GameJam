using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextMoveText : MonoBehaviour
{
    [SerializeField] CountUpText m_countUpText;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        Color color = textMeshProUGUI.color;
        color.a = 0.0f;
        textMeshProUGUI.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_countUpText == null) return;
        Debug.Log("Update");
        TextMeshProUGUI textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        if (m_countUpText.IsFinish())
        {
            Debug.Log("フラグオッケー");
            if (textMeshProUGUI.color.a <= 0.001f)
            {
                Color color = textMeshProUGUI.color;
                color.a = 1.0f;
                textMeshProUGUI.color = color;
            }
        }
    }
}
