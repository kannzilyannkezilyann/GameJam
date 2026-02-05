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
using System.Linq;
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
    //コインオブジェクト
    [SerializeField] GameObject m_coin;
    //生成したテキスト
    CountUpText m_countUpText;
    //ハイスコア判定
    private bool m_highScoreChecked = false;
    //生成した宝
    List<GameObject> m_treasures = new List<GameObject>();
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
        m_countUpText = scoreText.GetComponent<CountUpText>();
        m_countUpText.Initialize(0, GameManager.instance.GetCurrentScore(), 800);
        m_countUpText.StopNumber(0);
        scoreText.transform.SetParent(m_canvas.transform, false);
        Debug.Log("Initialize OK");
        //キー案内テキスト生成
        GameObject nextMoveText = Instantiate(m_nextMove);
        nextMoveText.GetComponent<NextMoveText>().Initialize(scoreText.GetComponent<CountUpText>());
        nextMoveText.transform.SetParent(m_canvas.transform, false);
        //「ハイスコア」テキストを非表示
        m_highScoreText.SetActive(false);

        StartCoroutine(SpawnTreasuresCoroutine());
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
        if (!m_countUpText.IsFinish())
        {
            //飛ばす
            return;
        }

        //ハイスコアの判定をしていなかったら
        if (!m_highScoreChecked)
        {
            //ハイスコアを更新したか
            bool isHighScore = GameManager.instance.SetHighScore(GameManager.instance.GetStageName(), GameManager.instance.GetCurrentScore());
            //テキストをアクティブ状態を設定
            m_highScoreText.SetActive(isHighScore);

            //ハイスコアかチェックした
            m_highScoreChecked = true;
        }
        //スペースキーを押したら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(m_scene);
        }
    }

    private IEnumerator SpawnTreasuresCoroutine()
    {
        var data = TreasureManager.instance.GetGotTreasureData(); // Dictionary<int, TreasureData>
        var sortedKeys = data.Keys.OrderBy(k => k);

        foreach (var key in sortedKeys)
        {
            // Prefab取得
            GameObject prefab = TreasureManager.instance.GetPrefab(data[key].masterID);
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab not found for masterID {data[key].masterID}");
                continue;
            }

            // 生成
            Vector3 pos = new Vector3(Random.Range(-4.0f,4.0f), 5.0f + 1.0f*data[key].scale.y, 0f); // 必要に応じてランダムにする
            GameObject treasure = Instantiate(prefab, pos, Quaternion.identity);
            treasure.transform.localScale = data[key].scale;
            treasure.AddComponent<BurstCoin>().Initialize(data[key].score,m_coin);
            m_countUpText.AddStopNumber(data[key].score);
            // 1秒待機
            yield return new WaitForSeconds(0.1f);
        }
    }
}