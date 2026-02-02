/**
 * @file   ItemSlot.cs
 *
 * @brief  アイテムスロットUIに関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// クラスの定義 ===============================================================
/**
  * @brief アイテムスロットUI
  */
public class ItemSlot : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //プレイヤー
    [SerializeField] PlayerScript player;
    //アイテム画像
    [SerializeField] Image m_itemImage;
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
        m_itemImage.enabled = false;
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
        //プレイヤーがアイテムを所持していたらアイテムの画像を表示
    }
}
