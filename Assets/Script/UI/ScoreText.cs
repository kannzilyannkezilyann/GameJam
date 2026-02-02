/**
 * @file   ScoreText.cs
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
// クラスの定義 ===============================================================
/**
  * @brief スコア表示
  */
public class ScoreText : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //プレイヤー
    [SerializeField] PlayerScript m_player;
    //テキスト
    private TextMeshProUGUI m_text;
// メンバ関数の定義 -------------------------------------------------
    /**
     * @brief 初期化処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Start()
    {
        //テキスト取得
        m_text = GetComponent<TextMeshProUGUI>();
        //0.0kgに初期化
        SetUp(0);
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
        //テキスト更新
        SetUp(m_player.GetScore());
    }

    /**
     * @brief テキストセット
     *
     * @param[in] weight  表示する重量
     *
     * @return なし
     */
    void SetUp(int weight)
    {
        m_text.text = weight.ToString("000000");
    }
}
