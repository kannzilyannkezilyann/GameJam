using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @file   GamePlayCameraScript.cs
*
* @brief  ゲームプレイ中のカメラに関するソースファイル
*
* @author 制作者名　福地貴翔
*
* @date   最終更新日　2025/12/16
*/
public class GamePlayCameraScript : MonoBehaviour
{
    [SerializeField] private Transform m_player;

    [SerializeField] private float m_centerRange = 0.05f;
    [SerializeField] private float m_startPosX = 0.0f;
    [SerializeField] private float m_endPosX = 3.0f;
    bool m_inCenterX = false;

    /**
    * @brief 初期化処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    // Start is called before the first frame update
    void Start()
    {
        Vector3 targetPos = new Vector3(
                m_startPosX,
                transform.position.y,
                transform.position.z
            );

        transform.position = targetPos;
    }

   /**
   * @brief 更新処理
   *
   * @param[in] なし
   *
   * @return なし
   */
    // Update is called once per frame
    void Update()
    {

    }
     private void LateUpdate()
    {
        if(this.transform.position.x > m_endPosX)
        {
            Vector3 targetPos = new Vector3(
                m_endPosX,
                transform.position.y,
                transform.position.z
            );

            transform.position = targetPos;
            m_inCenterX = false;
        }
        if(this.transform.position.x < m_startPosX)
        {
            Vector3 targetPos = new Vector3(
                m_startPosX,
                transform.position.y,
                transform.position.z
            );

            transform.position = targetPos;
            m_inCenterX = false;
        }
        if (!m_inCenterX)
        {
            m_inCenterX = m_player.position.x >= this.transform.position.x - m_centerRange &&
                          m_player.position.x <= this.transform.position.x + m_centerRange;
        }
        else
        {
            Vector3 targetPos = new Vector3(
                m_player.position.x,
                transform.position.y,
                transform.position.z
            );

            transform.position = targetPos;
        }
    }
}
