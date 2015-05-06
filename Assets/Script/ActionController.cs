﻿using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {

	public float xOff = 0f;//x轴偏移量
	public float yOff = 0f;//y轴偏移量

	private bool isReadyToExchange =true;
	private ArrayList exchangeList;
	private Candy exchangeItem;

	void Start () {
		exchangeItem = null;
		exchangeList = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void reset_pos(){
		if (exchangeList.Count == 2) {
			Candy item0 = exchangeList [0] as Candy;
			Candy item1 = exchangeList[1] as Candy;
			//
			exchange_pos(item0,item1);
			//
			SendMessage("apply_adjust_postion_0",exchangeList);//请求更新在数组中的位置
			//
			exchangeItem = null;
			//
			this.isReadyToExchange = true;
			//
			item0.setChosen(false);
			item1.setChosen(false);
		}
	}

	void notice_isReadyToExchange(bool param){
		isReadyToExchange = param;
	}

	void apply_exchange_pos(Candy item){
		//交换位置判断
		if (null != exchangeItem ){
			if(item != exchangeItem && item.mIndex!=exchangeItem.mIndex && isReadyToExchange){
				//列判断是否可以交换位置
				if (Mathf.Approximately (item.mPos.y, exchangeItem.mPos.y)) {
					if (Mathf.Approximately (item.mPos.x + xOff, exchangeItem.mPos.x) || Mathf.Approximately (item.mPos.x - xOff, exchangeItem.mPos.x)) {
						exchangeList.Clear();//清空交换列表
						//
						exchange_pos(exchangeItem,item);//交换位置
						//
						exchangeList.Add (item);
						exchangeList.Add (exchangeItem);
						//
						isReadyToExchange = false;

						if(item.isSpecial && exchangeItem.isSpecial){//两个特殊糖果交换

						}else if(item.isSpecial || exchangeItem.isSpecial){//其中一个是特殊糖果
							//任意一个是彩色糖果则触发彩色糖果特效
							if(item.mType == GameController._TYPE.COLORFUL){
								SendMessage("apply_trigger_special_candy",exchangeList);
							}else if(exchangeItem.mType == GameController._TYPE.COLORFUL){
								SendMessage("apply_trigger_special_corlorful_candy",exchangeList);
							}
						}else{//普通糖果则请求更新在数组中的位置
							SendMessage("apply_adjust_postion",exchangeList);//请求更新在数组中的位置
						}
						//
						exchangeItem = null;
					}
					
				} else if (Mathf.Approximately (item.mPos.x, exchangeItem.mPos.x)) {//行判断是否可以交换位置
					if (Mathf.Approximately (item.mPos.y + yOff, exchangeItem.mPos.y) || Mathf.Approximately (item.mPos.y - yOff, exchangeItem.mPos.y)) {
						exchangeList.Clear();//清空交换列表
						//
						exchange_pos(exchangeItem,item);//交换位置
						//
						exchangeList.Add (item);
						exchangeList.Add (exchangeItem);
						//
						isReadyToExchange = false;
						//
						//如果是特殊糖果则请求触发特殊糖果的效果
						if(item.isSpecial || exchangeItem.isSpecial){
							SendMessage("apply_trigger_special_candy",exchangeList);
						}else{//普通糖果则请求更新在数组中的位置
							SendMessage("apply_adjust_postion",exchangeList);//请求更新在数组中的位置
						}
						//
						exchangeItem = null;
					}
				}
			}
			else{
				exchangeItem = null;
			}
		} else {
			item.setChosen (true);
			exchangeItem = item;
		}
	}
	//交换位置及索引
	void exchange_pos(Candy item0,Candy item1){

		Vector3 temp_pos = item0.mPos;
		item0.mPos = item1.mPos;
		item1.mPos = temp_pos;
		//
		int temp_row = item0.mRow;
		int temp_col = item0.mCol;
		//
		item0.mCol = item1.mCol;
		item0.mRow = item1.mRow;
		//
		item1.mRow = temp_row;
		item1.mCol = temp_col;
	}
}
