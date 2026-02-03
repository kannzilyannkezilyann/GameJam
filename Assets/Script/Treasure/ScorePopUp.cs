/**
 * @file   ScorePopUp.cs
 *
 * @brief  スコア表示に関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/02
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// クラスの定義 ===============================================================
/**
  * @brief スコア表示
  */
public class ScorePopUp : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //上昇スピード
    [SerializeField] float m_moveUpSpeed = 1.0f;
    //生存時間
    [SerializeField] float m_lifeTime = 1.0f;
    //テキスト
    TextMeshPro m_text;
    //色
    Color m_startColor;
// メンバ関数の定義 -------------------------------------------------
    /**
     * @brief 初期化処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Awake()
    {
        m_text = GetComponent<TextMeshPro>();
        m_startColor = m_text.color;
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
        // 上に移動
        transform.position += Vector3.up * m_moveUpSpeed * Time.deltaTime;

        // フェードアウト
        float t = Time.deltaTime / m_lifeTime;
        m_text.color = new Color(
            m_startColor.r,
            m_startColor.g,
            m_startColor.b,
            m_text.color.a - t
        );

        // 完全に透明になったら消す
        if (m_text.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }

    /**
     * @brief テキスト設定
     *
     * @param[in] score  スコア
     *
     * @return なし
     */
    public void SetUp(int score)
    {
        Debug.Log(score.ToString());
        m_text.text = score.ToString();
    }
}
