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
using UnityEngine;
// クラスの定義 ===============================================================
/**
  * @brief 宝
  */
public class Treasure : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //重量
    [SerializeField] private float m_weight;
    //加算スコア
    [SerializeField] private int m_score;
    ////スプライト
    //[SerializeField] private Sprite m_sprite;
    ////アニメーション
    //[SerializeField] private AnimationClip m_clip;
    //消滅エフェクト
    [SerializeField] private GameObject m_destroyEffect;
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
        ////スプライト設定
        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = m_sprite;
        ////アニメーション設定
        //Animator animator = GetComponent<Animator>();
        ////animator.runtimeAnimatorController = m_clip;
        //    var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //animator.runtimeAnimatorController = overrideController;

        //// "BaseClip" は Animator Controller 側に設定してある元のClip名
        //overrideController["BaseClip"] = newClip;
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
        //プレイヤーと接触したら
        if (collision.gameObject.CompareTag("Player"))
        {
            //エフェクト生成
            Instantiate(m_destroyEffect, gameObject.transform.position, Quaternion.identity);
            //消去
            Destroy(gameObject);
        }
    }
}
