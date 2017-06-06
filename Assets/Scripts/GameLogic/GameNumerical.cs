using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNumerical  {

//计算当前关卡需要完成的分数 ax幂次方+bx+c x为关卡编号
//系数取5
private   const int coeA=5;
private   const int coeB=5;
private   const int coeC=1000;
public static int GetFinishScore(int level){
	return (int)Mathf.Pow(coeA,level)+coeB*level+coeC;
}

public static int GetStarScore(int index){
	return index*index*5;
}
}
