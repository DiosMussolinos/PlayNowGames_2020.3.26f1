using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailHolder : MonoBehaviour
{

    ///////// PUBLIC /////////
    //Scriptable Object
    public Email_Scriptable holder;

    //UI
    public Image logo;
    public Text sender;
    public Text tittle;
    public Text content;
    public Text timeHour;
    public Text timeMin;

    ///////// PUBLIC /////////

    ///////// PRIVATE /////////
    private Main mainScript;
    ///////// PRIVATE /////////


    private void Start()
    {

        #region Email Related
        logo = holder.logo;
        sender.text = holder.sender;
        StartTittle();
        StartContent();
        timeHour.text = holder.timeHour;
        timeMin.text = holder.timeMin;
        #endregion

        #region Player Related
        
        GameObject player = GameObject.Find("====Character/Camera====");
        mainScript = player.GetComponent<Main>();
        #endregion
    }

    private void StartTittle()
    {
        #region Simulation of OutLook Behavior

        if (holder.tittle.Length > 24)
        {
            //New Tittle Empty
            string NewTittle = "";

            for (int i = 0; i < holder.tittle.Length; i++)
            {

                if (i < 21)
                {
                    NewTittle += holder.tittle[i];
                }
                else
                {
                    NewTittle += "...";
                    tittle.text = NewTittle;
                    break;
                }
            }
        }
        else
        {
            tittle.text = holder.tittle;
        }
        
        #endregion
    }

    private void StartContent()
    {
        #region Simulation of OutLook Behavior

        if (holder.content.Length > 32)
        {
            //New Content Empty
            string NewContent = "";

            for (int i = 0; i < holder.content.Length; i++)
            {

                if (i < 21)
                {
                    NewContent += holder.content[i];
                }
                else
                {
                    NewContent += "...";
                    content.text = NewContent;
                    break;
                }
            }
        }
        else
        {
            content.text = holder.content;
        }

        #endregion
    }

    public void ClickEmail()
    {
        //Change the position
        //if active and deactivate, take 2 clicks to update the message at the first moment
        Vector3 newPos = new Vector3(mainScript.selected.transform.position.x, mainScript.selected.transform.position.y, 0);
        mainScript.selected.transform.position = newPos;
        mainScript.selectedEmail = holder;

        ClickChangeInfo();
    }

    public void ClickChangeInfo()
    {
        #region Get All Info And Change
        GameObject BodyLogo = GameObject.Find("=Body-Image/Logo=");
        if (BodyLogo)
        {
            Image Logo = BodyLogo.GetComponent<Image>();
            if (Logo)
            {
                Logo = holder.logo;
            }
        }


        GameObject BodyTittle = GameObject.Find("=Body-Tittle=");
        if (BodyTittle)
        {
            Text Tittle = BodyTittle.GetComponent<Text>();
            if(Tittle)
            {
                Tittle.text = holder.tittle;

            }
        }       


        GameObject BodyGreetins = GameObject.Find("=Body-Greetins=");
        if (BodyGreetins)
        {
            Text Greetings = BodyGreetins.GetComponent<Text>();
            if(Greetings)
            {
                Greetings.text = holder.greetings;
            }
        }

        GameObject BodyContent = GameObject.Find("=Body-Content=");
        if (BodyContent)
        {
            Text Content = BodyContent.GetComponent<Text>();
            if(Content)
            {
                Content.text = holder.content;
            }
        }

        GameObject BodyBye = GameObject.Find("=Body-Bye=");
        if (BodyBye)
        {
            Text Bye = BodyBye.GetComponent<Text>();
            if(Bye)
            {
                Bye.text = holder.bye;
            }
        }

        GameObject BodyHour = GameObject.Find("=Body-TimeHour=");
        if (BodyHour)
        {
            Text Hour = BodyHour.GetComponent<Text>();
            if(Hour)
            {
                Hour.text = holder.timeHour;
            }
        }

        GameObject BodyMin = GameObject.Find("=Body-TimeMin=");
        if (BodyMin)
        {
            Text Min = BodyMin.GetComponent<Text>();
            if(Min)
            {
                Min.text = holder.timeMin;
            }
        }

        #endregion
    }

}