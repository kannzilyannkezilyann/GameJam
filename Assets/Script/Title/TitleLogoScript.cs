using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @file   TitleLogoScript.cs
*
* @brief  タイトルロゴに関するソースファイル
*
* @author 制作者名　深谷翔太
*
* @date   最終更新日　2026/2/3
*/
public class TitleLogoScript : MonoBehaviour
{
    //プレイヤーの位置
    [SerializeField] private Transform m_player;
    //プレイヤーとどのくらい離れるか
    [SerializeField] private float m_range = 7.0f;
    //画面中央に到着したかどうかのフラグ
    bool m_arrival = false;
    /**
    * @brief 初期化処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void Start()
    {
        //値の初期化
        this.transform.position = new Vector2(m_player.position.x - m_range,this.transform.position.y);
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
        if (m_player == null) return;
        if (!m_arrival)
        {
            this.transform.position = new Vector2(m_player.position.x - m_range, this.transform.position.y);
        }
        if(this.transform.position.x >= 0.0f)
        {
            m_arrival = true;
            this.transform.position = new Vector2(0.0f, this.transform.position.y);
        }
    }

    /**
    * @brief 画面中央に到着したかどうかのフラグの取得
    *
    * @param[in] なし
    *
    * @return 画面中央に到着したかどうかのフラグ
    */
    public bool GetArrival()
    {
        return m_arrival;
    }
}
