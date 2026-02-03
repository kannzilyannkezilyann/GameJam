/**
 * @file   DestroyEffect.cs
 *
 * @brief  消滅エフェクトに関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryEffect : MonoBehaviour
{
   /**
    * @brief アニメーション終了時の処理
    *  
    * @param[in] なし
    *
    * @return なし
    */
    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
