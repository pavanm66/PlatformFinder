using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    #region Public Variables
    public Pulpit pulpitPrefab;
    public Doofus doofus;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public AudioManager audioManager;
    #endregion

    #region private Variables


    bool isgameOver;
    Pulpit latestPulpit, oldPulpit;
    List<Pulpit> pulpitsList;
    [SerializeField] List<Vector3> usedPositionsList;
    int score;

    [SerializeField] private Image musicSprite;
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Image soundSprite;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    #endregion 

    #region Getter Setters

    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = "" + score;
        }
    }
    public bool IsGameOver
    {
        get => isgameOver;
        set
        {
            isgameOver = value;
            gameOverPanel.SetActive(isgameOver);
            Time.timeScale = isgameOver ? 0 : 1f;
        }
    }
    bool isMusicOn;
    public bool IsMusicOn
    {
        get => isMusicOn;
        set
        {
            isMusicOn = value;
            PlayerPrefs.SetInt("Music", isSoundOn ? 1 : 0);
            musicSprite.sprite = IsMusicOn ? musicOn : musicOff;
        }
    }
    bool isSoundOn;
    

    public bool IsSoundOn
    {
        get => isSoundOn;
        set
        {
            isSoundOn = value;
            PlayerPrefs.SetInt("Sound", isSoundOn ? 1 : 0);
            soundSprite.sprite = IsSoundOn ? soundOn : soundOff;
        }
    }

    #endregion

    #region UnityMethods
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        pulpitsList = new List<Pulpit>();
        for (int i = 0; i < 5; i++) //5 pool
        {
            Pulpit newPulpit = Instantiate<Pulpit>(pulpitPrefab, this.transform);
            newPulpit.gameObject.name = "" + i;
            newPulpit.gameObject.SetActive(false);
            pulpitsList.Add(newPulpit);
        }
        StartGame();
    }
    #endregion

    #region public Methods

    public void StartGame()
    {
        Score = 0;
        usedPositionsList = new List<Vector3>();
        SpawnPulpitRandom();
        doofus.transform.position = new Vector3(0, 1.5f, 0);
        doofus.transform.rotation = Quaternion.identity;
        doofus.myBody.WakeUp();
        IsGameOver = false;
        soundSprite.sprite = IsSoundOn ? soundOn : soundOff;
        musicSprite.sprite = IsMusicOn ? musicOn : musicOff;

        //SetMusicOn();
        //SetSoundOn();

    }
    public void GameOver()
    {
        audioManager.PlayFailure();
        IsGameOver = true;
        foreach (var item in pulpitsList)
        {
            StopCoroutine(item.ActivateTimer());
            item.gameObject.SetActive(false);
        }
        doofus.myBody.Sleep();
        oldPulpit = latestPulpit = null;

    }
    public void SpawnPulpitRandom()
    {
        if (usedPositionsList.Count >= 2)
            if (usedPositionsList.Contains(oldPulpit.transform.position))
            {
                usedPositionsList.Remove(oldPulpit.transform.position);
            }

        if (latestPulpit)
        {
            oldPulpit = latestPulpit;
            Score++;
        }
        latestPulpit = GetObjectFromPool();

        latestPulpit.transform.position = GetUnusedPosition();
        latestPulpit.gameObject.SetActive(true);
    }


    //for audio settings
    public void SetMusicOn()
    {
        print(PlayerPrefs.GetInt("Music", 1) + " in music on");
        IsMusicOn =!IsMusicOn;
        //musicSprite.sprite = IsMusicOn ? musicOn : musicOff;
        if (IsMusicOn)
        {
            audioManager.bgAudio.Play();
        }
        else
        {
            audioManager.bgAudio.Pause();
        }
    }
    public void SetSoundOn()
    {
        IsSoundOn = !IsSoundOn;
        //soundSprite.sprite = IsSoundOn ? soundOn : soundOff;
    }
    #endregion

    #region Private methods

    Vector3 GetUnusedPosition()
    {
        int i = 0;
        Vector3 newPos = Vector3.zero;
        i = Random.Range(0, 4);
        if (oldPulpit)
        {
            switch (i)
            {
                case 0: //forward
                    newPos = oldPulpit.transform.forward * 9;
                    break;
                case 1://right
                    newPos = oldPulpit.transform.right * 9;
                    break;
                case 2://back
                    newPos = -oldPulpit.transform.forward * 9;
                    break;
                case 3://left
                    newPos = -oldPulpit.transform.right * 9;
                    break;
                default:
                    break;
            }

            newPos += oldPulpit.transform.position;
        }
        if (!usedPositionsList.Contains(newPos))
        {
            usedPositionsList.Add(newPos);
            return newPos;
        }

        return GetUnusedPosition();
    }
    Pulpit GetObjectFromPool()
    {
        return pulpitsList.Find(x => !x.gameObject.activeInHierarchy);
    }

    #endregion

}//class
