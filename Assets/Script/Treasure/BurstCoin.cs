/**
 * @file   BurstCoin.cs
 *
 * @brief  宝がコインに変わるに関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/05
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// クラスの定義 ===============================================================
/**
  * @brief 宝がコインに変わる
  */

public class BurstCoin : MonoBehaviour
{
// クラス定数の宣言 -------------------------------------------------
    //一コイン生成するためのスコア
    const int SCORE_TO_COIN_RATE = 150;
    //宝がコインに変わるまで
    const float TREASURE_BURST = 1.2f;
    // データメンバの宣言 -----------------------------------------------
    //コインプレハブ
    GameObject m_coinPrefab;
    //生成するコインの数
    int m_coins;
    //経過時間
    float m_time;
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
     * @brief 初期化処理
     *
     * @param[in] score
     *
     * @return なし
     */
    public void Initialize(int score,GameObject coinPrefab)
    {
        m_coinPrefab = coinPrefab;
        m_coins = score /SCORE_TO_COIN_RATE;
        //あまりでるなら
        if(score % SCORE_TO_COIN_RATE != 0)
        {
            m_coins++;
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
        m_time += Time.deltaTime;
        if(m_time > TREASURE_BURST)
        {
            for(int i=0; i<m_coins; i++)
            {
                Vector3 pos = new Vector3(gameObject.transform.position.x + Random.Range(-0.5f,0.5f),
                    gameObject.transform.position.y + Random.Range(-0.5f, 0.5f),
                    gameObject.transform.position.z);
                Instantiate(m_coinPrefab,pos,Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
