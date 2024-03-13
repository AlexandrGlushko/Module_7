using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    [SerializeField] ImageTimer _timerHarvest;
    [SerializeField] ImageTimer _timerFeedWarrior;
    [SerializeField] ImageTimer _timerToRaid;
    [SerializeField] Image _timerSignedFarmer;
    [SerializeField] Image _timerSignedWarrior;
    [SerializeField] TMP_Text _infoWheatText;
    [SerializeField] TMP_Text _infoFarmerText;
    [SerializeField] TMP_Text _infoWarriorText;
    [SerializeField] Button _signedFarmerButton;
    [SerializeField] Button _signedWarriorButton;
    [SerializeField] GameObject _pauseWindow;
    [SerializeField] TMP_Text _pauseWindowText;
    [SerializeField] GameObject _infoPlaneWindows;
    [SerializeField] TMP_Text _infoSplashWindowText;
    [SerializeField] Image _backgroundSplashWindow;
    [SerializeField] TMP_Text _timeScaleButtonText;
    [SerializeField] TMP_Text _warriorOnRaidText;
    [SerializeField] TMP_Text _raidLeftText;
    [SerializeField] GameObject _infoMassegePlane;
    [SerializeField] GameObject _infoMessage;
    [SerializeField] TMP_Text _infoMessageText;
    [SerializeField] int warriorCount;
    [SerializeField] int farmerCount;
    [SerializeField] int farmerMaxCount;
    [SerializeField] int wheatCout;
    [SerializeField] int wheatFarmerTake;
    [SerializeField] int wheatEatFarmer;
    [SerializeField] int wheatEatWarrior;
    [SerializeField] int notAttackRaid;
    [SerializeField] int warriorOnRaid;
    [SerializeField] int raidCount;
    [SerializeField] int raidMaxCount;
    [SerializeField] int amountPlayCount;
    [SerializeField] int timeCreateFarmer;
    [SerializeField] int timeCreateWarrior;
    [SerializeField] int valTimeHarvest;
    [SerializeField] int valTimeFeedWarrior;
    [SerializeField] int valTimeToRaid;
    [SerializeField] float maxScaleTimeVal;
    [SerializeField] float scaleValueClick;
    private float timerFarmer = -2;
    private float timerWarrior = -2;
    private int clikOnScaleSpeedCount;
    private int warriorIsDead;
    private bool isWarriorCreate;
    private bool isFarmerCreate;
    private bool isPauseGame;
    private int incrementWarrior = 1;
    private int incrementEnemy = 1;
  
    private void Start()
    {
        StartCoroutine(CallInfoMassege($"На нас нападут через {notAttackRaid} неделю, необходимо обучить больше войнов и крестьян!!!", 4));
        AudioManager.Instance.PlayMusic("Background");
        _timerHarvest.TimerSetStart(valTimeHarvest, true);
        _timerToRaid.TimerSetStart(valTimeToRaid, true);
        InfoTextUpdate();
    }
    void Update()
    {
        CheckWheatCount();
        RaidToVillage();
        CreateFarmerTimer();
        CreateWarriorTimer();
        TakeWheatFromField();
        FeedWarrior();
        InfoTextUpdate();
    }

    private void MainGameLogic()
    {
        if(warriorCount < 0)
        {
            CallSplashWindow(false);
        }
        if(farmerCount >= farmerMaxCount)
        {
            CallSplashWindow(true);
        }
        if(raidCount >= raidMaxCount)
        {
            CallSplashWindow(true);
        }
        if(farmerCount == 20 || farmerCount == 40 || farmerCount == 60)
        {
            timeCreateWarrior -= 1;
            StartCoroutine(CallInfoMassege($"Работают {farmerCount} крестьян, теперь войны обучаются быстрее", 3));
        }
        if (farmerCount == 80)
        {
            incrementWarrior++;
            timeCreateWarrior -= 1;
            StartCoroutine(CallInfoMassege($"Работают {farmerCount} крестьян, теперь {incrementWarrior} войнов быстрее обучается за один курс", 3));
        }
        if ((raidCount == 6 && !isFarmerCreate) || (raidCount == 9 && !isFarmerCreate))
        {
            incrementEnemy += 2;
            warriorOnRaid += 1;
            StartCoroutine(CallInfoMassege($"После {raidCount} набега, на нас нападут {warriorOnRaid} войнов", 3));
        }
        if(warriorOnRaid > (warriorCount + incrementEnemy + incrementWarrior))
        {
            StartCoroutine(CallInfoMassege($"У нас всего {warriorCount} войнов, обучите больше войнов", 3));
        }
    }

    IEnumerator CallInfoMassege(string str, float second)
    {
        _infoMessage.SetActive(true);
        _infoMessageText.text = str;
        yield return new WaitForSeconds(second);
        _infoMessage.SetActive(false);
    }

    private void CallSplashWindow(bool isWin)
    {
        AudioManager.Instance.StopMusic("Background");
        string strTextInfo = "";
        Time.timeScale = 0;
        _infoPlaneWindows.SetActive(true);
        _infoMassegePlane.SetActive(false);
        if (isWin)
        {
            strTextInfo = "Вы выиграли\n\n";
            _backgroundSplashWindow.color = new Color(0.33f, 0.9f, 0.33f, 1);
            AudioManager.Instance.PlayMusic("WinGame");
        }
        else
        {
            strTextInfo = "Вы проиграли\n\n";
            _backgroundSplashWindow.color = new Color(0.57f, 0.06f, 0.12f, 1);
            AudioManager.Instance.PlayMusic("LoseGame");
        }
        _infoSplashWindowText.text = strTextInfo +
                                     $"Недель прошло: {amountPlayCount}\n" +
                                     $"Нападений отбито: {raidCount}\n" +
                                     $"Зерна собрано: {wheatCout}\n" +
                                     $"Крестьян: {farmerCount}\n" +
                                     $"Войнов погибло: {warriorIsDead}";
    }

    private void RaidToVillage()
    {
        if (_timerToRaid.timeTick)
        {  
            if (amountPlayCount >= notAttackRaid)
            {
                warriorCount -= warriorOnRaid;
                AudioManager.Instance.PlaySfx("DeadWarrior");
                if (warriorCount < 0) warriorIsDead += warriorCount;
                warriorIsDead += warriorOnRaid;
                MainGameLogic();
                StartCoroutine(CallInfoMassege($"Пало {warriorIsDead} война, мы будет их помнить", 3));
                warriorOnRaid += incrementEnemy;
                raidCount++;
            }
            amountPlayCount++;
        }
    }

    private bool CheckWheatCount()
    {
        if(wheatCout > ((warriorCount * wheatEatWarrior) + wheatEatWarrior)) 
        {
            if (isFarmerCreate) _signedFarmerButton.interactable = false;
            else _signedFarmerButton.interactable = true;
            if (isWarriorCreate) _signedWarriorButton.interactable = false;
            else _signedWarriorButton.interactable = true;
            return true;
        }
        _signedFarmerButton.interactable = false;
        _signedWarriorButton.interactable = false;
        return false;
    }

    public void ClickSondOffOn()
    {
        AudioManager.Instance.MuteSound();
    }

    public void ClickRestartButton()
    {
        AudioManager.Instance.PlaySfx("Click");
        AudioManager.Instance.StopMusic("WinGame");
        AudioManager.Instance.StopMusic("LoseGame");
        AudioManager.Instance.PlayMusic("Background");
        Time.timeScale = 1;
        _timeScaleButtonText.text = Time.timeScale.ToString();
        _infoPlaneWindows.SetActive(false);
        _infoMassegePlane.SetActive(true);
        _timerFeedWarrior.TimerSetStart(valTimeFeedWarrior, false);
        _timerHarvest.TimerSetStart(valTimeHarvest, false);
        _timerToRaid.TimerSetStart(valTimeToRaid, false);
        _timerHarvest.TimerSetStart(valTimeHarvest, true);
        _timerToRaid.TimerSetStart(valTimeToRaid, true);
        if(isWarriorCreate) warriorCount = -1;
        warriorCount = 0;
        if(isFarmerCreate) farmerCount = 2;
        farmerCount = 2;
        farmerMaxCount = 100;
        wheatCout = 10;
        notAttackRaid = 0;
        warriorOnRaid = 3;
        raidCount = 0;
        raidMaxCount = 10;
        amountPlayCount = 0;
        warriorIsDead = 0;
        incrementWarrior = 1;
        incrementEnemy = 1;
    }

    public void ClickTimeSpeedUpButton()
    {
        AudioManager.Instance.PlaySfx("Click");
        clikOnScaleSpeedCount++;
        if (Time.timeScale >= maxScaleTimeVal)
        {
            Time.timeScale = 1;
            clikOnScaleSpeedCount = 0;
        }
        Time.timeScale += scaleValueClick * clikOnScaleSpeedCount;
        _timeScaleButtonText.text = Time.timeScale.ToString();
    }

    public void ClickPauseGameButton()
    {
        AudioManager.Instance.PlaySfx("Click");
        if (!isPauseGame)
        {
            Time.timeScale = 0.0f;
            isPauseGame = true;
            _pauseWindow.SetActive(true);
            _pauseWindowText.text = $"ПАУЗА\n\n" +
                                    $"Недель прошло: {amountPlayCount}\n" +
                                    $"Нападений отбито: {raidCount}\n" +
                                    $"Войнов нападет: {warriorOnRaid}\n" +
                                    $"Зерна собрано: {wheatCout}\n" +
                                    $"Крестьян: {farmerCount}\n" +
                                    $"Войнов погибло: {warriorIsDead}";
        }
    }

    public void ClickPlayGameButton()
    {
        isPauseGame = false;
        Time.timeScale = 1.0f;
        _pauseWindow.SetActive(false);
    }

    public void ClickCreateFarmerButton()
    {
        AudioManager.Instance.PlaySfx("Click");
        if (CheckWheatCount())
        {
            isFarmerCreate = true;
            wheatCout -= wheatEatFarmer;
            timerFarmer = timeCreateFarmer;
            _signedFarmerButton.interactable = false;
            MainGameLogic();
        }
    }

    public void ClickCreateWarriorButton()
    {
        AudioManager.Instance.PlaySfx("Click");
        if (CheckWheatCount())
        {
            isWarriorCreate = true;
            wheatCout -= wheatEatWarrior;
            timerWarrior = timeCreateWarrior;
            _signedWarriorButton.interactable = false;
            _timerFeedWarrior.TimerSetStart(valTimeFeedWarrior, true);
        }
    }

    private void CreateFarmerTimer()
    {
        if(timerFarmer > 0)
        {
            timerFarmer -= Time.deltaTime;
            _timerSignedFarmer.fillAmount = timerFarmer / timeCreateFarmer;
        }
        else if(timerFarmer > -1)
        {
            _timerSignedFarmer.fillAmount = 1;
            timerFarmer = -2;
            farmerCount += 1;
            _signedFarmerButton.interactable = true;
            isFarmerCreate = false;
            AudioManager.Instance.PlaySfx("CreateFarmer");
        }
    }

    public void CreateWarriorTimer()
    {
        if (timerWarrior > 0)
        {
            timerWarrior -= Time.deltaTime;
            _timerSignedWarrior.fillAmount = timerWarrior / timeCreateWarrior;
        }
        else if (timerWarrior > -1)
        {
            _timerSignedWarrior.fillAmount = 1;
            timerWarrior = -2;
            warriorCount += incrementWarrior;
            _signedWarriorButton.interactable = true;
            isWarriorCreate = false;
            AudioManager.Instance.PlaySfx("CreateWarrior");
        }
    }

    private void TakeWheatFromField()
    {
        if (_timerHarvest.timeTick)
        {
            wheatCout += farmerCount * wheatFarmerTake;
            AudioManager.Instance.PlaySfx("WheatTake");
        }
    }

    private void FeedWarrior()
    {
        if(_timerFeedWarrior.timeTick && wheatCout > 0)
        {
            wheatCout -= warriorCount * wheatEatWarrior;
            AudioManager.Instance.PlaySfx("FeedWarrior");
        }
    }

    private void InfoTextUpdate()
    {
        _infoWheatText.text = wheatCout.ToString();
        _infoFarmerText.text = farmerCount.ToString();
        _infoWarriorText.text = warriorCount.ToString();
        _warriorOnRaidText.text = warriorOnRaid.ToString();
        _raidLeftText.text = (raidMaxCount - raidCount).ToString();
    }
}
