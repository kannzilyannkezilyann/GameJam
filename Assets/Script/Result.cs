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
    //キャンバス
    [SerializeField] Canvas m_canvas;
    //生成したテキスト
    CountUpText m_countUpText;

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
        if (Input.GetKeyDown("space") && m_scoreText.GetComponent<CountUpText>().IsFinish())
        {
            SceneManager.LoadScene(m_scene);
        }
    }
}
