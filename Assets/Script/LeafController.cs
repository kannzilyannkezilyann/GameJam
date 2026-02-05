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

    [SerializeField] private GameObject m_particle; ///< パーティクル

    [SerializeField] private float m_burnRange = 0.01f; ///< 燃える射程

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //タイルマップがなければ何もせず終了
        if (!m_tileMap)
        {
            Debug.Log("何もない");
            return;

        }

        //松明を持ってなかったら何もしない
        if (m_player.GetComponent<PlayerScript>().GetItem() != ItemKinds.TORCH)
        {
            Debug.Log("松明じゃない");
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
        Vector3Int minCell = m_tileMap.WorldToCell(min);
        Vector3Int maxCell = m_tileMap.WorldToCell(max);

        //それを元にプレイヤーのいるセルを取得
        //Vector3Int playerCell = m_tileMap.WorldToCell(playerPos);


        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                //調べるタイルの位置を取得
                Vector3Int checkPos = new Vector3Int(x, y, 0);

                //そこにタイルがあればそれを燃やす
                if (m_tileMap.HasTile(checkPos))
                {
                    //パーティクルがあれば出す
                    if (m_particle)
                    {
                        //まずはエフェクト生成
                        GameObject fireParticle = Instantiate(m_particle,m_tileMap.GetCellCenterWorld(checkPos),Quaternion.identity);

                        Debug.Log($"燃やしたタイル: x={x}, y={y}");
                    }

                    //あったタイルは消滅させる
                    m_tileMap.SetTile(checkPos, null);
                }
            }

        }
    }
}
