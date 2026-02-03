using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 	 * @file   GoalGateContoroller.cs
 	 *
 	 * @brief  ゴールゲートに関するソースファイル
 	 *
 	 * @author 制作者名　小塚太陽
 	 *
 	 * @date   最終更新日　2025/02/03
 	 */


public class GoalGateContoroller : MonoBehaviour
{
    [SerializeField] private string m_ClearSceneName = "null"; ///< クリアした時に遷移するシーン名

    /*
     * 
     * @brief 初期化処理
     * 
     * @param[in] なし
     * 
     * @return なし
     * 
     */
    void Start()
    {
        
    }

    /*
     * @brief 更新処理
     *
     * @param[in] なし
     * 
     * @return なし
     *
     */
    void Update()
    {
        
    }

    /*
     * 
     * @brief 横から当たった時の処理
     * 
     * @param[in] collision 当たったオブジェクト
     * 
     * @return なし
     * 
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //判定はプレイヤーと当たった時のみ
        if(collision.gameObject.CompareTag("Player"))
        {

            //横からの衝突のみ判定し、ゴールとみなす
            if(Mathf.Abs(collision.relativeVelocity.x) > Mathf.Abs(collision.relativeVelocity.y))
            {
                //ゴールしたらシーン遷移(nullの場合はデバッグ用ログを出すだけ)
                if (m_ClearSceneName != "null") SceneManager.LoadScene(m_ClearSceneName);
                else Debug.Log("ゴール！");
            }

        }
    }
}
