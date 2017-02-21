using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StarManager : MonoBehaviour
{
    public Transform starRoot;
    private int width = 10;
    private int hight = 10;
    private int blockWidth = 107;
    // Use this for initialization
    private GameObject starTemplate;
    private Star[,] starsArr = null;
    private bool isCanClick = true;
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        /* RectTransform rectTran= starRoot.GetComponent<RectTransform>();
         rectTran.anchoredPosition=new Vector2((Screen.width- GUIHelper.GetDeviceWidth(blockWidth)*width)/2,-GUIHelper.GetDeviceHeight(504));
          blockWidth=GUIHelper.GetDeviceWidth(blockWidth);*/
        InitGame();
    }
    private void InitGame()
    {
        isCanClick = true;
        starTemplate = Resources.Load(ResoucesPathEnum.prrfabPath + "star", typeof(GameObject)) as GameObject;
        Debug.Log("template" + starTemplate);
        starsArr = new Star[width, hight];
        ResetGame();
    }
    private void ResetGame()
    {
        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject obj = GameObject.Instantiate(starTemplate);
                RectTransform tran = obj.GetComponent<RectTransform>();
                obj.name = j + "_" + i;
                tran.SetParent(starRoot);
                tran.localScale = Vector3.one;
                tran.localRotation = Quaternion.identity;
                tran.anchoredPosition = new Vector3(j * blockWidth, -i * blockWidth, 0);
                Star star = obj.AddComponent<Star>();
                star.SetColor((StarColorEnum)Random.Range(0, (int)StarColorEnum.MAX));
                star.SetIndex(j, i);
                star.GetButton().onClick.AddListener(delegate ()
                {
                    OnClickStar(star);
                });
                starsArr[i, j] = star;
            }
        }
        emptyLine = 0;
    }
    private List<Star> checkList = new List<Star>();
    private List<Star> deleteList = new List<Star>();
    private HashSet<Star> haveCheckedList = new HashSet<Star>();
    private void OnClickStar(Star star)
    {
        Debug.Log("x" + star.GetX() + "Y" + star.GetY());
        if (isCanClick)
        {
            deleteList = GetDeleteStarList(star.GetX(), star.GetY());
            if (deleteList != null)
            {
                Debug.Log("delete Count" + deleteList.Count);
                if (deleteList.Count >= 2)
                {
                    foreach (var s in deleteList)
                    {
                        starsArr[s.GetY(), s.GetX()] = null;
                        Debug.Log(s.gameObject.name);
                        GameObject.Destroy(s.gameObject);
                    }
                    isCanClick=false;
                    StartCoroutine(Check());
                }
            }
        }



    }

    private List<Star> GetDeleteStarList(int x, int y)
    {
        Star star = starsArr[y, x];
        if (star == null) return null;
        checkList.Clear();
        deleteList.Clear();
        haveCheckedList.Clear();
        deleteList.Add(star);
        haveCheckedList.Add(star);
        AddRoundCheckStar(star.GetX(), star.GetY());
        while (checkList.Count != 0)
        {
            Star tempStar = checkList[0];
            haveCheckedList.Add(tempStar);
            checkList.Remove(tempStar);
            if (tempStar.GetColor() == star.GetColor())
            {
                deleteList.Add(tempStar);
                AddRoundCheckStar(tempStar.GetX(), tempStar.GetY());
            }
        }
        return deleteList;
    }
    IEnumerator Check()
    {
        yield return new WaitForSeconds(0.2f);
        UpAndDownCheck();
        LeftAndRightCheck();
        CheckGameOver();
        StartCoroutine(SetCanClickFlag());
    }
    IEnumerator SetCanClickFlag()
    {
        yield return new WaitForSeconds(0.2f);
        isCanClick=true;

    }
    private void AddRoundCheckStar(int x, int y)
    {

        if (x - 1 >= 0 && starsArr[y, x - 1] != null && !haveCheckedList.Contains(starsArr[y, x - 1]) && !checkList.Contains(starsArr[y, x - 1]))
            checkList.Add(starsArr[y, x - 1]);
        if (y - 1 >= 0 && starsArr[y - 1, x] != null && !haveCheckedList.Contains(starsArr[y - 1, x]) && !checkList.Contains(starsArr[y - 1, x]))
            checkList.Add(starsArr[y - 1, x]);
        if (x + 1 < width && starsArr[y, x + 1] != null && !haveCheckedList.Contains(starsArr[y, x + 1]) && !checkList.Contains(starsArr[y, x + 1]))
            checkList.Add(starsArr[y, x + 1]);
        if (y + 1 < hight && starsArr[y + 1, x] != null && !haveCheckedList.Contains(starsArr[y + 1, x]) && !checkList.Contains(starsArr[y + 1, x]))
            checkList.Add(starsArr[y + 1, x]);
    }
    //上下检测
    private void UpAndDownCheck()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = hight - 1; j >= 0; j--)
            {
                if (starsArr[j, i] != null)
                {
                    for (int k = j + 1; k < hight; k++)
                    {
                        if (starsArr[k, i] != null && k == j + 1)
                        {
                            break;
                        }
                        if ((starsArr[k, i] != null && k > j + 1) || (starsArr[k, i] == null && k == hight - 1))
                        {
                            Star srcStar = starsArr[j, i];
                            if (srcStar == null)
                                Debug.LogError(i + "_" + j);
                            if (starsArr[k, i] == null && k == hight - 1)
                            {
                                srcStar.SetIndex(i, k);
                                Tweener tweener = srcStar.GetRectTransform().DOAnchorPos3D(new Vector3(i * blockWidth, -k * blockWidth, 0), 0.1f);
                                //设置这个Tween不受Time.scale影响
                                tweener.SetUpdate(true);
                                //设置移动类型
                                tweener.SetEase(Ease.Linear);
                                /* tweener.OnComplete = delegate ()
                                 {
                                     Debug.Log("移动完毕事件");
                                 };*/
                                //  srcStar.GetRectTransform().anchoredPosition = new Vector3(i * blockWidth, -k * blockWidth, 0);
                                starsArr[k, i] = srcStar;
                            }
                            else
                            {
                                srcStar.SetIndex(i, k - 1);
                                Tweener tweener = srcStar.GetRectTransform().DOAnchorPos3D(new Vector3(i * blockWidth, -(k - 1) * blockWidth, 0), 0.1f);
                                tweener.SetUpdate(true);
                                //设置移动类型
                                tweener.SetEase(Ease.Linear);
                                // srcStar.GetRectTransform().anchoredPosition = new Vector3(i * blockWidth, -(k - 1) * blockWidth, 0);
                                starsArr[k - 1, i] = srcStar;
                            }

                            starsArr[j, i] = null;
                            break;

                        }
                    }
                }
            }
        }
    }
    private int emptyLine = 0;
    private void LeftAndRightCheck()
    {
        for (int i = 0; i < width - emptyLine; i++)
        {
            bool isEmpty = true;
            for (int j = 0; j < hight; j++)
            {
                if (starsArr[j, i] != null)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty)
            {

                for (int k = i + 1; k < width; k++)
                {
                    for (int t = 0; t < hight; t++)
                    {
                        Star tstar = starsArr[t, k];
                        if (tstar != null)
                        {
                            tstar.SetIndex(k - 1, t);
                            Tweener tweener = tstar.GetRectTransform().DOAnchorPos3D(new Vector3((k - 1) * blockWidth, -t * blockWidth, 0), 0.1f);
                            tweener.SetUpdate(true);
                            //设置移动类型
                            tweener.SetEase(Ease.Linear);
                            // tstar.GetRectTransform().anchoredPosition = new Vector3((k - 1) * blockWidth, -t * blockWidth, 0);
                        }
                        starsArr[t, k - 1] = tstar;
                        starsArr[t, k] = null;
                    }
                }
                emptyLine++;
                i--;
            }

        }
    }

    private bool CheckGameOver()
    {
        bool victory = true;
        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (starsArr[i, j] != null)
                {
                    List<Star> tdeleteList = GetDeleteStarList(starsArr[i, j].GetX(), starsArr[i, j].GetY());
                    if (tdeleteList != null && tdeleteList.Count >= 2)
                    {
                        victory = false;
                        return victory;
                    }
                }

            }
        }
        Debug.LogError("Game Over!!");
        return victory;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
