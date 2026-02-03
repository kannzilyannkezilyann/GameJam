/**
 * @file   GameManager.cs
 *
 * @brief  ゲーム管理に関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クラスの定義 ===============================================================
/**
  * @brief ゲーム管理
  */

public class GameManager : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //ゲーム管理クラスインスタンス
    public static GameManager instance;
    //スコア
    private int m_score =  0;

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
        
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        
    }

    /**
    * @brief スコアの設定
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void SetScore(int score)
    {
        m_score = score;
    }

    /**
    * @brief スコアの取得
    *
    * @param[in] なし
    *
    * @return なし
    */
    public int GetScore()
    {
        return m_score;
    }
}
