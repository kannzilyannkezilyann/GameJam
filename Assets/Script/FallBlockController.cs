using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @brief 落下するタイルブロックのクラス
 * 
 * @auto 制作者　小塚太陽
 * 
 * @detail 落下するブロックのクラスです。やることは基本的にしばらくしたら消える処理負荷の軽減です
 * 
 * 
 * 
 */
public class FallBlockController : MonoBehaviour
{

    [SerializeField] float m_viewTime = 1.0f; ///< 表示時間（これを超えると消滅する）

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
        //指定した表示時間の後に破壊を予約する
        Invoke("DestroyYourSelf", m_viewTime);
    }

    /*
     *
     * @brief 自身を破壊する処理
     *
     * @param[in] なし
     *
     * @return なし
     *
     */
    void DestroyYourSelf()
    {
        //thisだとスクリプトだけが消えるので注意
        Destroy(gameObject);
    }
}
