using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 *
 * @brief ステージを徐々に崩していくクラス（複数のTilemapを一つに見立てて左から順に破壊）
 * 
 * @autor 制作者　小塚太陽（改良: GitHub Copilot 風）
 * 
 * @detail 複数のTilemapがサイズや左端位置がバラバラでも、
 *         全体を「ひとつの横方向の連続したグリッド」と見なして
 *         左端（最小X）から右方向に列単位で破壊していきます。
 * 
 */

public class StageDestroyer : MonoBehaviour
{
    [SerializeField] private Tilemap[] m_tileMaps; ///< 崩すタイルマップたち
    [SerializeField] float m_timer = 0f; ///< タイマー
    [SerializeField] float m_destroyInterval = 5f; ///< ブロックを破壊する間隔（秒単位）
    [SerializeField] int m_destroyCount = 1; ///< ステージを一気に破壊する列数

    [SerializeField] GameObject m_fallTileBlock; ///< 呼び出す落下オブジェクト

    int m_currentDestroyColumn = 0; ///< 既に破壊した列数（左からのオフセット）
    int m_globalMinX = int.MaxValue; ///< 崩すタイルマップの中で最小のX
    int m_globalMaxX = int.MinValue; ///< 崩すタイルマップの中で最大のX

    bool m_isSaccese = false; ///< 成功したかどうか

    void Start()
    {
        // 初期化
        m_timer = 0f;
        m_currentDestroyColumn = 0;

        //エラー回避処理(そもそもタイルマップが入ってない時など)
        if (m_tileMaps == null || m_tileMaps.Length == 0)
        {
            Debug.LogWarning("StageDestroyer: Tilemaps not assigned.");
            return;
        }

        foreach (var tileMap in m_tileMaps)
        {
            //そもそも無ければスキップして次へ
            if (tileMap == null) continue;

            //調べる前に不要な空のセルを圧縮する（boundsの弊害になるため）
            tileMap.CompressBounds();

            var bounds = tileMap.cellBounds;

            if (bounds.xMin < m_globalMinX) m_globalMinX = bounds.xMin;
            if (bounds.xMax > m_globalMaxX) m_globalMaxX = bounds.xMax;
        }

        //ここまで来たらエラーなしということで成功変数をtrueに
        m_isSaccese = true;
    }

    void Update()
    {
        //初期化成功してなければ終了する
        if(!m_isSaccese) return;

        //タイマー加算
        m_timer += Time.deltaTime;

        //タイマーが破壊間隔を超えたら破壊処理に
        if (m_timer > m_destroyInterval)
        {
            //破壊カウントの回数分破壊する
            for (int i = 0; i < m_destroyCount; i++)
            {
                int globalX = m_globalMinX + m_currentDestroyColumn;

                DestroyStage(globalX);
                m_currentDestroyColumn++;
            }

            m_timer = 0f;
        }
    }

    /*
     * @brief 指定されたX列を破壊する処理
     * @param[in] globalX グローバル座標系のX（ワールドではなくTilemapのセル座標）
     */
    void DestroyStage(int globalX)
    {

        foreach (var tileMap in m_tileMaps)
        {
            // 範囲内にいなければスキップして次へ
            if (globalX < tileMap.cellBounds.xMin) continue;

            // そのX列の上から下まで全て破壊する
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                //X,Yを位置に変換
                Vector3Int pos = new Vector3Int(globalX, y, 0);

                // タイルが存在する場合のみ破壊
                if (tileMap.HasTile(pos))
                {
                    //破壊されたタイルを落下するオブジェクトとして呼び出す
                    if(m_fallTileBlock != null)
                    {
                        //事前に取得しておいた落下ブロックのプレハブから落下ブロックのインスタンスを新たに作成
                        GameObject fallBlock = Instantiate(m_fallTileBlock,pos,Quaternion.identity);

                        //コンポーネント取得
                        var render    = fallBlock.GetComponent<SpriteRenderer>();
                        var rigidBody = fallBlock.GetComponent<Rigidbody2D>();

                        //スプライトを破壊するタイルのものに変更
                        render.sprite = tileMap.GetSprite(pos);
                        render.color = tileMap.color * tileMap.GetColor(pos);

                        //スプライトの落下速度などの物理設定
                        rigidBody.velocity = new Vector3(Random.Range(-0.1f, 0.1f),
                                                         Random.Range(-1.0f, 1.0f),
                                                         0);

                        rigidBody.angularVelocity = Random.Range(5.0f, -5.0f);
                    }


                    //元あったタイルはnullにする（そうすれば消える）
                    tileMap.SetTile(pos, null);
                }
            }
        }
    }
}
