using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class ItemGenerator : MonoBehaviour {
	// carPrefabを入れる
	public GameObject carPrefab;
	// coinPrefabを入れる
	public GameObject coinPrefab;
	// cornPrefabを入れる
	public GameObject conePrefab;
	// スタート地点
	private int startPos = -160;
	// ゴール地点
	private float goalPos = 120;
	// アイテムを出すx方向の範囲
	private float posRange = 3.4f;


	// Unityちゃんのオジュジェクト
	private GameObject unitychan;
	// Unityちゃんの位置
	private float positionZ;

	// Prefabs
	private List<GameObject> prefabList = new List<GameObject>();
	// 画面外の距離
	private float destroyDistance = 10;
	// アイテム範囲
	private int itemRange = 50;
	// アイテム設置終了位置
	private float endRangeLog;
	// アイテム設置終了位置から40m手前
	private float startLine;

	// Use this for initialization
	void Start () {

		// Unityちゃんのオブジェクトを取得
		this.unitychan = GameObject.Find ("unitychan");
		// Unityちゃんの位置を取得
		this.positionZ = unitychan.transform.position.z;

		// 初期障害物を設置
		GenerateItems (this.startPos, this.startPos + itemRange);

	}
	
	// Update is called once per frame
	void Update () {
		// Unityちゃんの位置を取得
		this.positionZ = this.unitychan.transform.position.z;

		// Unityちゃんがアイテムを設置してある40m手前のLineを超える
		if ((this.positionZ > this.startLine) && (this.endRangeLog < this.goalPos)) {
			// Unityちゃんの前方40-50mの範囲に障害物を設置
			GenerateItems (this.endRangeLog, this.endRangeLog + 10);
		}
			
		// 障害物を削除
		DestroyItems ();

		// Debug.Log
		//Debug.Log (this.positionZ + "," +this.startLine + "," + this.endRangeLog);
	}

	// unityちゃんが通り過ぎた障害物を削除
	void DestroyItems(){

		// 全ての障害物
		for (int i = 0; i < prefabList.Count; i++) {
			
			// prefabListの中身を代入
			GameObject prefabObject = prefabList [i];

			// Unityちゃんの座標より後ろになり、画面外いなると
			if (prefabObject.transform.position.z + destroyDistance < this.positionZ) {
				// prefabListから削除
				this.prefabList.Remove (prefabObject);
				// prefabObjectを削除
				Destroy (prefabObject);
			}
		}
	}

	// アイテム生成（アイテム設置開始、アイテム設置終了）
	void GenerateItems(float startRange, float endRange){

		// アイテムを設置した位置の更新
		this.endRangeLog = endRange;
		this.startLine = endRange - 40;

		// 一定の距離ごとにアイテムを生成
		for (float i = startRange; i < endRange; i += 15) {
			// どのアイテムを出すのかをランダムに設定
			int num = Random.Range (0, 10);
			if (num <= 1) {
				// コーンをx軸方向に一直線に生成
				for (float j = -1; j <= 1; j += 0.4f) {
					GameObject cone = Instantiate (conePrefab) as GameObject;
					cone.transform.position = new Vector3 (4 * j, cone.transform.position.y, i);

					// prefabListにオブジェクトを追加
					prefabList.Add (cone);
				}
			} else {
				// レーンごとにアイテムを生成
				for (int j = -1; j < 2; j++) {
					// アイテムの種類を決める
					int item = Random.Range (1, 11);
					// アイテムを置くZ座標のオフセットをランダムに設定
					int offsetZ = Random.Range (-5, 6);
					// 60%コイン配置：30%車配置：10%何もなし
					if (1 <= item && item <= 6) {
						// コインを生成
						GameObject coin = Instantiate (coinPrefab) as GameObject;
						coin.transform.position = new Vector3 (posRange * j, coin.transform.position.y, i + offsetZ);


					} else if (7 <= item && item <= 9) {
						// 車を生成
						GameObject car = Instantiate (carPrefab) as GameObject;
						car.transform.position = new Vector3 (posRange * j, car.transform.position.y, i + offsetZ);

						// prefabListにオブジェクトを追加
						prefabList.Add (car);
					}
				}
			}
		}
	}
}
