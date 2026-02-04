using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

/*
 *
 * @brief ステージを徐々に崩していくクラス
 * 
 * @autor 制作者　小塚太陽
 * 
 * @detail ステージがどんどん崩れていく部分のクラスです。
 * 
 * 
 */





public class StageDestroyer : MonoBehaviour
{

    [SerializeField] private Tilemap m_tileMap; ///< 崩すタイルマップ

    [SerializeField] float m_timer = 0; ///< タイマー

    [SerializeField] int m_destroyInterval = 5; ///< ブロックを破壊する間隔（秒単位）

    [SerializeField] int m_destroyCount = 1; ///< ステージを一気に破壊する列数

    int m_currentDestroyRaw; ///<　今壊しているRaw;

    int m_tileMapWidth; ///< タイルマップの横幅

    int m_tileMapHeight; ///< タイルマップの縦幅


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
        //タイマーリセット
        m_timer = 0;

        //タイルの幅から
        m_tileMapWidth = m_tileMap.GetComponent<Tilemap>().cellBounds.x;
        m_tileMapHeight = m_tileMap.GetComponent<Tilemap>().cellBounds.y;
    }

  
    /*
     * 
     * @brief 更新処理
     * 
     * @param[in] なし
     * 
     * @return なし
     * 
     */
    void Update()
    {
        //時間加算
        m_timer += Time.deltaTime;

        
        //間隔が来たら破壊させる
        if(m_timer > m_destroyInterval)
        {
            //設定した破壊数分破壊する
            for (int i = 0; i < m_destroyCount; i++)
            {
                DestroyStage(i + m_currentDestroyRaw);
            }

            //破壊した行を加算（次に破壊する場所の指標にする）
            m_currentDestroyRaw += m_destroyCount;

            //タイマーリセット
            m_timer = 0;
        }

    }

    /*
     * 
     * @brief ステージの破壊処理
     * 
     * @param[in] x 破壊する行（X）
     * 
     * @return なし
     * 
     */
    void DestroyStage(int x)
    {
        for(int y = 0;y < m_tileMapHeight;y++)
        {
            //x,yから位置を作る
            Vector3Int pos = new(x, y,0);

            //その位置のタイルをnullに置き換える
            m_tileMap.SetTile(pos,null);
        }
    }
}
