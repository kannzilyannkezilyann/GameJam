/**
 * @file   NextMoveText.cs
 *
 * @brief  キー案内のテキストに関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/04
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// クラスの定義 ===============================================================
/**
  * @brief キー案内のテキスト
  */

public class NextMoveText : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //参照するスコアテキスト
    private CountUpText m_countUpText;
 // メンバ関数の定義 -------------------------------------------------
    /**
     * @brief 生成時処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Start()
    {
        //透明にする
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
        //参照するスコアテキスト
        m_countUpText = countUpText;
    }



    /**
     * @brief 更新処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Update()
    {
        if (m_countUpText == null) return;
        Debug.Log("Update");
        TextMeshProUGUI textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        //加算し終えたら
        if (m_countUpText.IsFinish())
        {
            //表示する
            if (textMeshProUGUI.color.a <= 0.001f)
            {
                Color color = textMeshProUGUI.color;
                color.a = 1.0f;
                textMeshProUGUI.color = color;
            }
        }
    }
}
