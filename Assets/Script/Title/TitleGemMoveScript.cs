using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @file   TitleGemMoveScript.cs
*
* @brief  タイトル画面のに関するソースファイル
*
* @author 制作者名　深谷翔太
*
* @date   最終更新日　2026/2/3
*/
public class TitleGemMoveScript : MonoBehaviour
{
    //ロゴとどのくらい離れるか
    [SerializeField] private float m_range = 7.0f;
    //ロゴ
    [SerializeField] private TitleLogoScript m_titleLogoScript;
    /**
    * @brief 初期化処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void Start()
    {
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
        this.transform.position = new Vector2(m_titleLogoScript.transform.position.x + m_range, this.transform.position.y);
    }
}
