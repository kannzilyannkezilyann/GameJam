using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @file   GamePlayCameraScript.cs
*
* @brief  ゲームプレイ中のカメラに関するソースファイル
*
* @author 制作者名　福地貴翔,小塚太陽
*
* @date   最終更新日　2025/12/16
*/
public class GamePlayCameraScript : MonoBehaviour
{
    [SerializeField] private Transform m_player;

    [SerializeField] private float m_centerRange = 0.05f;
    [SerializeField] private float m_startPosX = 0.0f; //< カメラの左端(X)
    [SerializeField] private float m_endPosX = 3.0f;   //< カメラの右端(X)

    [SerializeField] private float m_topPosY = 3.0f;  //< カメラの上端(Y)
    [SerializeField] private float m_bottomPosY = 0.0f;    //< カメラの下端(Y)

    [SerializeField] private float m_cameraMoveYBorder; ///< カメラを動かすYのボーダー

    private float m_firstPosY; ///< 最初のYの位置

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

        m_firstPosY = transform.position.y;

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

        Vector3 targetPos = new Vector3(
              transform.position.x,
              transform.position.y,
              transform.position.z
          );


        //カメラのX座標が外に出ないようにする
        targetPos.x = Mathf.Clamp(m_player.position.x, m_startPosX, m_endPosX);


        //カメラの座標
        if(Mathf.Abs(m_player.position.y - m_firstPosY) >= m_cameraMoveYBorder)
        {
            //targetPos.y = m_player.position.y;
            targetPos.y = Mathf.Clamp(m_player.position.y, m_bottomPosY, m_topPosY);

            targetPos.y = Mathf.Lerp(transform.position.y, targetPos.y, Time.deltaTime * 2.0f);
        }
        else
        {
            targetPos.y = Mathf.Lerp(transform.position.y, m_firstPosY, Time.deltaTime * 2.0f);
        }



            //if (this.transform.position.y < m_bottomPosY)
            //{
            //    targetPos.y = m_bottomPosY;

            //    //m_inCenterY = false;
            //}
            //if (this.transform.position.y > m_topPosY)
            //{
            //    targetPos.y = m_topPosY;

            //    //m_inCenterY = false;
            //}



            //位置をカメラに反映
            transform.position = targetPos;
    }
}
