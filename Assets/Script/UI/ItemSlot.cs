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
    [SerializeField] PlayerScript m_player;

    //アイテムのイメージ群
    [System.Serializable]
    public class ItemUI
    {
        public ItemKinds kind;
        public GameObject imageObj;
    }

    //イメージ群のリスト
    [SerializeField] private ItemUI[] itemUIs;

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
     * @brief 更新処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Update()
    {
        //プレイヤーがnullなら飛ばす
        if(m_player == null) return;

        //現在所持しているアイテムに応じて画像を表示させる
        ItemKinds nowItem = m_player.GetItem();

        // 全部非表示
        foreach (var ui in itemUIs)
        {
            ui.imageObj.SetActive(false);
        }

        // 所持アイテムだけ表示
        foreach (var ui in itemUIs)
        {
            if (ui.kind == nowItem)
            {
                ui.imageObj.SetActive(true);
                break;
            }
        }
    }
}
