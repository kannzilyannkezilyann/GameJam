/**
 * @file   ScoreData.cs
 *
 * @brief  スコアデータに関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/04
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// クラスの定義 ===============================================================
/**
  * @brief スコアデータ
  */

public class ScoreData : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    
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
     * @brief ハイスコア取得
     *
     * @param[in] stage  保存するステージ
     *
     * @return なし
     */
    public int GetHighScore(string stage)
    {
        return PlayerPrefs.GetInt(stage, 0);
    }

    /**
     * @brief ハイスコア設定
     *
     * @param[in] stage  保存するステージ
     * @param[in] score  スコア
     *
     * @return true   ハイスコア更新した
     * @return false  ハイスコア更新しなかった
     */
    public bool SetHighScore(string stage, int score)
    {
        //現在のハイスコア取得
        int currentHighScore = GetHighScore(stage);
        //ハイスコアか比較
        if (currentHighScore > score) 
        {
            return false;
        }
        //スコアを保存
        PlayerPrefs.SetInt(stage, score);
        return true;
    }
}
