using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextMoveText : MonoBehaviour
{
    //参照するスコアテキスト
    private CountUpText m_countUpText;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        Color color = textMeshProUGUI.color;
        color.a = 0.0f;
        textMeshProUGUI.color = color;
    }

    /**
    　* @brief 初期化処理
    　*
    　* @param[in] start  開始数字
    　* @param[in] target 目標数字
    　* @param[in] step　 増加数字 1秒で
    　*
    　* @return なし
    　*/
    public void Initialize(CountUpText countUpText)
    {
        m_countUpText = countUpText;
    }


    // Update is called once per frame
    void Update()
    {
        if (m_countUpText == null) return;
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
