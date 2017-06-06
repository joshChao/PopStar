using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StarGameMainContext : BaseContext
{
    public int score
    {
        get;
        set;
    }

    public int level{
        get;
        set;
    }
    public StarGameMainContext()
        : base(UIType.StarGameMain)
    {

    }

}
public class StarGameMainView : AnimateView
{
    [SerializeField]
    private Transform changeStarRoot;
    [SerializeField]
    private Text txtLevel;
    [SerializeField]
    private RectTransform numberRoot;
     [SerializeField]
    private Text numberItem;
    [SerializeField]
    private Text txtTargert;
    [SerializeField]
    private Text txtCurScore;
    [SerializeField]
    private Transform starRoot;
    private static int width = 10;
    private static int hight = 10;
    private int blockWidth = 108;
    // Use this for initialization
    private GameObject starTemplate;
    private Star[,] starsArr = new Star[width, hight];
    private bool isCanClick = true;
    private bool isGameOver = false;
    [SerializeField]
    private Button _exitGame;
    StarGameMainContext m_context;
    private int curLevelFinishScore;
    private StarColorEnum m_changeColor;
    private List<GameObject> changeStarList=new List<GameObject>();
    public override void OnEnter(BaseContext context)
    {
        base.OnEnter(context);
        m_context = context as StarGameMainContext;
        InitGame();
    }

    public override void OnExit(BaseContext context)
    {
        base.OnExit(context);
        ExitGame();
    }

    public override void OnPause(BaseContext context)
    {
        _animator.SetTrigger("OnExit");
    }

    public override void OnResume(BaseContext context)
    {
        _animator.SetTrigger("OnEnter");
    }

    public void ExitGameCallBack()
    {
        Singleton<ContextManager>.Instance.Pop();
    }

    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        /* RectTransform rectTran= starRoot.GetComponent<RectTransform>();
         rectTran.anchoredPosition=new Vector2((Screen.width- GUIHelper.GetDeviceWidth(blockWidth)*width)/2,-GUIHelper.GetDeviceHeight(504));
          blockWidth=GUIHelper.GetDeviceWidth(blockWidth);*/

    }

    //初始化游戏
    private void InitGame()
    {
        isGameOver = false;
        isCanClick = true;
        starTemplate = Resources.Load(ResoucesPathEnum.prefabPath + "star", typeof(GameObject)) as GameObject;
        Debug.Log("template" + starTemplate);
       
        m_context.level=1;
        foreach(var go in changeStarList){
            Destroy(go);
        }
        changeStarList.Clear();
         changeStarRoot.gameObject.SetActive(true);
        for(int i=0;i<(int)StarColorEnum.MAX;i++){
            GameObject obj = GameObject.Instantiate(starTemplate);
                obj.SetActive(true);
                RectTransform tran = obj.GetComponent<RectTransform>();      
                tran.SetParent(changeStarRoot);
                tran.localScale = Vector3.one;
                tran.localRotation = Quaternion.identity;
                tran.localPosition = Vector3.zero;
                Star star = obj.AddComponent<Star>();
                star.GetButton().onClick.AddListener(delegate ()
                {
                    SeletColor(star);
                });
                star.SetColor((StarColorEnum)i);
                changeStarList.Add(obj);
              
        }
         changeStarRoot.gameObject.SetActive(false);
        ResetGame();
    }

private void SeletColor(Star star){
    Debug.Log(star.GetColor().ToString());
    m_changeColor=star.GetColor();
     changeStarRoot.gameObject.SetActive(false);
}
bool isChange=false;
public void ChangeColor(){
      isChange=!isChange;
      changeStarRoot.gameObject.SetActive(isChange);
}
    //重置游戏
    private void ResetGame()
    {
        tipList.Clear();
        thinkTime = 0;
        isGameOver = false;
        m_context.score=0;
        curLevelFinishScore=GameNumerical.GetFinishScore(m_context.level);
        txtTargert.text=curLevelFinishScore.ToString();
        txtCurScore.text= m_context.score.ToString();
        txtLevel.text=m_context.level.ToString();
        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if(starsArr[i,j]!=null)Destroy(starsArr[i,j].gameObject);
                GameObject obj = GameObject.Instantiate(starTemplate);
                obj.SetActive(true);
                RectTransform tran = obj.GetComponent<RectTransform>();
                obj.name = j + "_" + i;
                tran.SetParent(starRoot);
                tran.localScale = Vector3.one;
                tran.localRotation = Quaternion.identity;
                tran.localPosition = Vector3.zero;
                tran.anchoredPosition = new Vector3(j * blockWidth, -i * blockWidth);
                Star star = obj.AddComponent<Star>();
                star.SetColor((StarColorEnum)Random.Range(0, (int)StarColorEnum.MAX));
                star.SetIndex(j, i);
                star.GetButton().onClick.AddListener(delegate ()
                {
                    OnClickStar(star);
                });
                // star.ShowStarTip(true);
                starsArr[i, j] = star;

            }
        }
        emptyLine = 0;
    }

    //退出游戏
    private void ExitGame()
    {
       
    }

    private List<Star> checkList = new List<Star>();
    private List<Star> deleteList = new List<Star>();
    private HashSet<Star> haveCheckedList = new HashSet<Star>();
    private float thinkTime = 0;
    private void OnClickStar(Star star)
    {
        Debug.Log("x" + star.GetX() + "Y" + star.GetY());
        if (isCanClick)
        {
            if(isChange){
                isChange=false;
                tipList.Clear();
                star.SetColor(m_changeColor);
            }else{
            deleteList = GetDeleteStarList(star.GetX(), star.GetY());
            if (deleteList != null)
            {
                Debug.Log("delete Count" + deleteList.Count);
                if (deleteList.Count >= 2)
                {
                    int index = 0;
                    foreach (var s in deleteList)
                    {
                        starsArr[s.GetY(), s.GetX()] = null;
                        Debug.Log(s.gameObject.name);
                        float delayTime=0.15f;
                        if (index == 1)
                        {
                             delayTime=0;
                        }
                        StartCoroutine(DelayDestoryStar(delayTime,index,s));
                        foreach (var r in tipList)
                        {
                            if (r == s)
                            {
                                tipList.Clear();
                                break;
                            }
                        }
                        index++;
                    }
                    thinkTime = 0;
                    isCanClick = false;
                    StartCoroutine(Check());
                }
            }
            }
           
        }
    }
    private void PlayNumberEffect(int num,int x,int y){
                GameObject obj = GameObject.Instantiate(numberItem.gameObject);
                obj.SetActive(true);
                RectTransform tran = obj.GetComponent<RectTransform>();
                obj.name = num.ToString();
                tran.SetParent(starRoot);
                tran.localScale = Vector3.one;
                tran.localRotation = Quaternion.identity;
                tran.localPosition = Vector3.zero;
                tran.anchoredPosition = new Vector3(x * blockWidth, -y * blockWidth);
                Text txt=obj.GetComponent<Text>();
                txt.text=num.ToString();
                obj.transform.DOMove(txtCurScore.transform.position,1);
                StartCoroutine(DelayDestoryNum(1,num,obj));
               
    }
    IEnumerator DelayDestoryNum(float delayTime,int num,GameObject obj){
        yield return new WaitForSeconds(delayTime);
         m_context.score+=num;
         NumberUpdateEffect.ShowEffect(txtCurScore,m_context.score,0.8f);
        Destroy(obj);
    }
    IEnumerator DelayDestoryStar(float delayTime,int index,Star s)
    {
        yield return new WaitForSeconds(delayTime);
        GUIHelper.PlayEffect(starRoot, s.transform.position, "starEffect" + (int)s.GetColor(), s.GetColor(), new Vector3(blockWidth / 2, -blockWidth / 2, 0));
        GameObject.Destroy(s.gameObject);
        PlayNumberEffect(GameNumerical.GetStarScore(index+1),s.GetX(),s.GetY());
    }

    //获取与当前点相连接的同色块
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

    //加入可检测点
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
    //检查游戏各种状态
    IEnumerator Check()
    {
        yield return new WaitForSeconds(0.2f);
        UpAndDownCheck();
        LeftAndRightCheck();
        CheckGameOver();
        StartCoroutine(SetCanClickFlag());
    }

    //有块删除移动中不接受玩家点击
    IEnumerator SetCanClickFlag()
    {
        yield return new WaitForSeconds(0.2f);
        isCanClick = true;

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
                            HideTip(srcStar);
                            starsArr[j, i] = null;
                            break;

                        }
                    }
                }
            }
        }
    }

    //隐藏块放大缩小的提示
    private void HideTip(Star tstar)
    {
        foreach (var s in tipList)
        {
            if (s == tstar)
            {
                foreach (var v in tipList)
                {
                    v.ShowStarTip(false);
                }
                tipList.Clear();
                thinkTime = 0;
                break;
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
                            HideTip(tstar);
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
        isGameOver = true;
        float delayTime = 0;
        for (int k = 9; k >= 0; k--)
        {
            for (int v = 0; v < width; v++)
            {
                if (starsArr[k, v] != null)
                {
                    StartCoroutine(DelayDestoryStar(starsArr[k, v], delayTime));
                    delayTime += 0.1f;
                }
            }
        }
        if(m_context.score>=curLevelFinishScore){
            m_context.level+=1;          
        }else{
             m_context.level=1; 
            Debug.LogError("Game Over!!");
        }
     
        StartCoroutine(ShowGameOverTips(delayTime));
        return victory;
    }

    IEnumerator ShowGameOverTips(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ResetGame();
    }
    IEnumerator DelayDestoryStar(Star s, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject.Destroy(s.gameObject);
        GUIHelper.PlayEffect(starRoot, s.transform.position, "starEffect" + (int)s.GetColor(), s.GetColor(), new Vector3(blockWidth / 2, -blockWidth / 2, 0));
    }

    List<int> randomPool = new List<int>();
    List<Star> tipList = new List<Star>();
    //随机出提示
    private void CheckTipsRandom()
    {
        randomPool.Clear();
        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (starsArr[i, j] != null)
                    randomPool.Add(i * width + j);
            }
        }
        while (randomPool.Count > 0)
        {
            int index = Random.Range(0, randomPool.Count);
            int i = randomPool[index] / width;
            int j = randomPool[index] % width;
            tipList.Clear();
            tipList.AddRange(GetDeleteStarList(starsArr[i, j].GetX(), starsArr[i, j].GetY()));
            if (tipList != null && tipList.Count >= 2)
            {
                break;
            }
            else
            {
                randomPool.RemoveAt(index);
            }
        }
        if (tipList != null && tipList.Count >= 2)
        {
            foreach (var star in tipList)
            {
                star.ShowStarTip(true);
            }
        }
        else
        {
            tipList.Clear();
        }
    }

    //最多的
    private void CheckTipsMax()
    {
        tipList.Clear();
        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (starsArr[i, j] != null)
                {
                    starsArr[i, j].ShowStarTip(false);
                    List<Star> tlist = GetDeleteStarList(starsArr[i, j].GetX(), starsArr[i, j].GetY());
                    if (tlist != null && tlist.Count > tipList.Count)
                    {
                        tipList.Clear();
                        tipList.AddRange(tlist);
                    }
                }
            }
        }
        if (tipList != null && tipList.Count >= 2)
        {
            foreach (var star in tipList)
            {
                star.ShowStarTip(true);
            }
        }
        else
        {
            tipList.Clear();
        }
    }

//重置星星
List<Star> randomStarPool=new List<Star>();
public void ExchangeStar(){
    randomStarPool.Clear();
    for(int i=0;i<hight;i++){
        for(int j=0;j<width;j++)
        if(starsArr[i,j]!=null){
        starsArr[i, j].ShowStarTip(false);
        randomStarPool.Add(starsArr[i,j]);
        }
    }
  while(randomStarPool.Count>1){
        Star srcStar=randomStarPool[0];
        randomStarPool.Remove(srcStar);
        int index = Random.Range(0, randomStarPool.Count);
        Star targetStar=randomStarPool[index];
        srcStar.transform.DOMove(targetStar.transform.position,1);
        targetStar.transform.DOMove(srcStar.transform.position,1);
        randomStarPool.Remove(targetStar);
        starsArr[srcStar.GetY(),srcStar.GetX()]=targetStar;
        starsArr[targetStar.GetY(),targetStar.GetX()]=srcStar;
        int tempX=srcStar.GetX();
        int tempY=srcStar.GetY();
        srcStar.SetIndex(targetStar.GetX(),targetStar.GetY());
        targetStar.SetIndex(tempX,tempY);
     //   StarColorEnum tempColor=srcStar.GetColor();
     //   srcStar.SetColor(targetStar.GetColor());
     //   targetStar.SetColor(tempColor);
  }
   tipList.Clear();
}


    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;
        if (tipList.Count == 0)
            thinkTime += Time.deltaTime;
        if (tipList.Count == 0 && thinkTime > 2)
        {
            CheckTipsMax();
        }
    }
}
