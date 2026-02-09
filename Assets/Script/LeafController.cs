using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;


/*
 * 
 * @brief 葉っぱタイルのコントローラー 
 * 
 * @autor 作成者　小塚太陽
 * 
 * @detail 葉っぱタイルを燃やして消える処理を担当するクラスです
 * 
 */


public class LeafController : MonoBehaviour
{
    [SerializeField] Tilemap m_tileMap; ///< 葉っぱのタイルマップ

    [SerializeField] private GameObject m_player; ///< プレイヤー

    [SerializeField] private GameObject m_effect; ///< 燃やした時のエフェクト

    [SerializeField] private float m_burnRange = 0.01f; ///< 燃える射程

    /*
     *
     * @brief 当たり判定に当たった時の処理
     * 
     * @param[in] collision 当たったもののコリジョン
     * 
     * @return なし
     * 
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //タイルマップがなければ何もせず終了
        if (!m_tileMap)
        {
            Debug.Log("タイルマップが存在しません");
            return;

        }

        //松明を持ってなかったら何もしない
        if (m_player.GetComponent<PlayerScript>().GetItem() != ItemKinds.TORCH)
        {
            Debug.Log("松明を持っていませんでした");
            return;
        }




        //プレイヤーを覆うセルを取得
        var playerPos = m_player.transform.position;
        var playerCollider = m_player.GetComponent<Collider2D>();

        Bounds playerBounds = playerCollider.bounds;

        //プレイヤーのいる位置の最小と最大を取る
        Vector3 min = playerBounds.min + new Vector3(-m_burnRange, -m_burnRange, 0);
        Vector3 max = playerBounds.max + new Vector3(m_burnRange, m_burnRange, 0);

        //セル座標に変換し、これで調べる
        Vector3Int playerMinCell = m_tileMap.WorldToCell(min);
        Vector3Int playerMaxCell = m_tileMap.WorldToCell(max);


        for (int x = playerMinCell.x; x <= playerMaxCell.x; x++)
        {
            for (int y = playerMinCell.y; y <= playerMaxCell.y; y++)
            {
                //調べるタイルの位置を取得
                Vector3Int checkPos = new Vector3Int(x, y, 0);

                //そこにタイルがあればそれを燃やす
                if (m_tileMap.HasTile(checkPos))
                {
                    //エフェクトがあれば生成
                    if (m_effect)
                    {                      
                        GameObject fireParticle = Instantiate(m_effect,m_tileMap.GetCellCenterWorld(checkPos),Quaternion.identity);
                    }

                    //燃やした場所にあったタイルは消滅させる
                    m_tileMap.SetTile(checkPos, null);
                }
            }

        }
    }
}
