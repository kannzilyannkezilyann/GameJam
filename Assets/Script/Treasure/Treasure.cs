/**
 * @file   Treasure.cs
 *
 * @brief  宝に関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/02
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
// クラスの定義 ===============================================================
/**
  * @brief 宝
  */
public class Treasure : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //重量
    [SerializeField] private float m_weight = 50;
    //加算スコア
    [SerializeField] private int m_score = 1;
    //消滅エフェクト
    [SerializeField] private GameObject m_destroyEffect;
    //消滅エフェクトサイズ
    [SerializeField] private float m_destroyEffectScale = 5.0f;
    //スコア表示テキスト
    [SerializeField] GameObject m_scoreText;
    //プレイヤー判定オフ
    private bool m_playerCollisionOff = false;
    //取得時の効果音
    [SerializeField] private GameObject m_getSE;
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
        //重量によってRigidBodyのGravityScaleを変える
        GetComponent<Rigidbody2D>().gravityScale *= m_weight/50; 
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
     * @brief 衝突処理
     *
     * @param[in] collision  衝突したオブジェクト
     *
     * @return なし
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーの衝突応答処理がオフなら
        if (m_playerCollisionOff)
        {
            return;
        }

        //プレイヤーと接触したら
        if (collision.gameObject.CompareTag("Player"))
        {
            //エフェクト生成
            GameObject effect = Instantiate(m_destroyEffect, gameObject.transform.position, Quaternion.identity);
            //効果音の生成
            Instantiate(m_getSE, gameObject.transform.position, Quaternion.identity);
            //エフェクトの大きさを調整
            float scaleX = this.transform.localScale.x * m_destroyEffectScale;
            float scaleY = this.transform.localScale.y * m_destroyEffectScale;
            float scaleZ = this.transform.localScale.z * m_destroyEffectScale;
            effect.transform.localScale = new Vector3(scaleX,scaleY,scaleZ);
            //プレイヤーに重量とスコアを渡す
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (player != null) 
            {
                //プレイヤーに情報を渡す
                player.TakeTreasure(gameObject);

                //スコア表示
                GameObject text = Instantiate(m_scoreText, transform.position, Quaternion.identity);
                if(text.TryGetComponent<ScorePopUp>(out var scorePopUp))
                {
                    scorePopUp.SetUp(m_score);
                }
            }
            
            //消去
            Destroy(gameObject);
        }
    }


    /**
     * @brief プレイヤーの判定を一定時間オフにする
     *
     * @param[in] なし
     *
     * @return なし
     */
    public void DisableColliderTemporarily()
    {
        StartCoroutine(DisableCoroutine());
        StartCoroutine(DisablePlayerCoroutine());
    }

    /**
     * @brief プレイヤーの衝突応答をオフに
     *
     * @param[in] なし
     *
     * @return なし
     */
    private IEnumerator DisablePlayerCoroutine()
    {
        m_playerCollisionOff = true;        // 衝突応答をオフ
        yield return new WaitForSeconds(0.7f); // 待つ
        m_playerCollisionOff = false;         // 衝突応答を再度オン
    }

    /**
     * @brief コライダーをオフに
     *
     * @param[in] なし
     *
     * @return なし
     */
    private IEnumerator DisableCoroutine()
    {
        GetComponent<Collider2D>().enabled = false;        // コライダーをオフ
        yield return new WaitForSeconds(0.1f); // 待つ
        GetComponent<Collider2D>().enabled = true;         // コライダーを再度オン
    }

    /**
     * @brief 重量取得
     *
     * @param[in] なし
     *
     * @return 重量
     */
    public float GetWeight()
    {
        return m_weight;
    }
    /**
     * @brief スコア取得
     *
     * @param[in] なし
     *
     * @return スコア
     */
    public int GetScore()
    {
        return m_score;
    }
}
