using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    int m_score = 0;
    static int m_hiscore = 0;
    int m_ammo = 30;
    Player m_player;
    Text txt_ammo;
    Text txt_hiscore;
    Text txt_life;
    Text txt_score;

    void Start()
    {
        Instance = this;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foreach(Transform t in this.transform.GetComponentsInChildren<Transform>())
        {
            if(t.name.CompareTo("txt_ammo")==0)
            {
                txt_ammo = t.GetComponent<Text>();
            }
            else if(t.name.CompareTo("txt_hiscore")==0)
            {
                txt_hiscore = t.GetComponent<Text>();
                txt_hiscore.text = "High Score" + m_hiscore;
            }
            else if(t.name.CompareTo("txt_life")==0)
            {
                txt_life = t.GetComponent<Text>();
            }
            else if(t.name.CompareTo("txt_score")==0)
            {
                txt_score = t.GetComponent<Text>();
            }
        }
    }
    public void SetScore(int score)
    {
        m_score += score;
        if (m_score > m_hiscore)
            m_hiscore = m_score;
        txt_score.text = "Score<color=yellow>" + m_score + "</color>";
        txt_hiscore.text = "HighScore" + m_hiscore;
    }
    public void SetAmmo(int ammo)
    {
        m_ammo -= ammo;
        if (m_ammo <= 0)
            m_ammo = 30 - m_ammo;
        txt_ammo.text = m_ammo.ToString() + "/30";
    }
    public void SetLife(int life)
    {
        txt_life.text = life.ToString();
    }
    void OnGUI()
    {
        if(m_player.m_life<=0)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Game Over");
            GUI.skin.label.fontSize = 30;
            if(GUI.Button(new Rect(Screen.width*0.5f-150,Screen.height*0.75f,300,40),
                "Try again"))
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
        using (StreamWriter sw = new StreamWriter(@"H:\Score.txt", true))
        {
            DateTime dt = DateTime.Now;
            dt.ToLongTimeString().ToString();
            sw.Write(dt);
            sw.WriteLine("分数：" + m_hiscore);
        }
    }
  /*  public void Save(string Path, string inform)
    {

    }*/



}
