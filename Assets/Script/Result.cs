/**
 * @file   Result.cs
 *
 * @brief  リザルトに関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/04
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
// クラスの定義 ===============================================================
/**
  * @brief リザルト
  */

public class Result : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //遷移先のシーン
    [SerializeField] private string m_scene;
    //スコアテキスト
    [SerializeField] GameObject m_scoreText;
    //キー案内
    [SerializeField] GameObject m_nextMove;
    //ハイスコアテキスト
    [SerializeField] GameObject m_highScoreText;
    //キャンバス
    [SerializeField] Canvas m_canvas;
    //生成したテキスト
    CountUpText m_countUpText;
    //ハイスコア判定
    private bool m_highScoreChecked = false;
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
        //スコアテキスト生成
        GameObject scoreText = Instantiate(m_scoreText);
        scoreText.GetComponent<CountUpText>().Initialize(0, GameManager.instance.GetScore(), 800);
        scoreText.transform.SetParent(m_canvas.transform, false);
        Debug.Log("Initialize OK");
        //キー案内テキスト生成
        GameObject nextMoveText = Instantiate(m_nextMove);
        nextMoveText.GetComponent<NextMoveText>().Initialize(scoreText.GetComponent<CountUpText>());
        nextMoveText.transform.SetParent(m_canvas.transform, false);
        //「ハイスコア」テキストを非表示
        m_highScoreText.SetActive(false);
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
        //スコアカウントアップが終わっていなかったら
        if (!m_scoreText.GetComponent<CountUpText>().IsFinish())
        {
            //飛ばす
            return;
        }

        //ハイスコアの判定をしていなかったら
        if (!m_highScoreChecked)
        {
            //ハイスコアを更新したか
            //bool isHighScore = GameManager.instance.GetScoreData().SetHighScore(GameManager.instance.GetStageName(),GameManager.instance.GetScore());
            //テキストをアクティブ状態を設定
            //m_highScoreText.SetActive(isHighScore);
            
            //ハイスコアかチェックした
            m_highScoreChecked = true;
        }
        //スペースキーを押したら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(m_scene);
        }
    }
}
