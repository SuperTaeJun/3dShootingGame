using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Ui_InputFields
{
    public TextMeshProUGUI ResultText;
    public TMP_InputField IdInputField;
    public TMP_InputField PassWordInputField;
    public TMP_InputField PassWordConfirmInputField;

    public Button ConfirmButton;
}

public class Ui_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject SingupPanel;

    [Header("Login")]
    public Ui_InputFields LoginInputField;

    [Header("Singup")]
    public Ui_InputFields SingupInputField;

    private const string PREFIX = "ID+";
    private const string SALT = "990222";
    private void Start()
    {
        LoginPanel.SetActive(true);
        SingupPanel.SetActive(false);

        LoginCheck();
    }


    public void OnClickGoToSingupPanel()
    {
        LoginPanel.SetActive(false);
        SingupPanel.SetActive(true);
    }
    public void OnClickGoToLoginPanel()
    {
        LoginPanel.SetActive(true);
        SingupPanel.SetActive(false);
    }

    public void SingUp()
    {
        string id = SingupInputField.IdInputField.text;

        if (string.IsNullOrEmpty(id))
        {
            SingupInputField.ResultText.text = "아이디 다시 확인하시오";
            SingupInputField.ResultText.gameObject.SetActive(true);
            return;
        }

        string password = SingupInputField.PassWordInputField.text;

        string passWordConfirm = SingupInputField.PassWordConfirmInputField.text;

        if (password != passWordConfirm && string.IsNullOrEmpty(password))
        {
            SingupInputField.ResultText.text = "비번 다시 확인하시오";
            SingupInputField.ResultText.gameObject.SetActive(true);
            return;
        }

        PlayerPrefs.SetString(PREFIX + id, Encryption(password + SALT));

        SingupInputField.ResultText.gameObject.SetActive(false);
        LoginInputField.IdInputField.text = id;

        OnClickGoToLoginPanel();
    }
    public void Login()
    {
        string id = LoginInputField.IdInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            LoginInputField.ResultText.text = "아이디 다시 확인하시오";
            LoginInputField.ResultText.gameObject.SetActive(true);
            return;
        }
        string password = LoginInputField.PassWordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            LoginInputField.ResultText.text = "비번 다시 확인하시오";
            LoginInputField.ResultText.gameObject.SetActive(true);
            return;
        }

        if (!PlayerPrefs.HasKey(PREFIX + id))
        {
            LoginInputField.ResultText.text = "아이디나 비번 다시 확인하시오";
            LoginInputField.ResultText.gameObject.SetActive(true);
            return;
        }
        string hashedPassward = PlayerPrefs.GetString(PREFIX + id);
        if(hashedPassward != Encryption(password+SALT))
        {
            LoginInputField.ResultText.text = "아이디나 비번 다시 확인하시오";
            LoginInputField.ResultText.gameObject.SetActive(true);
            return;
        }


        Debug.Log("로그인 성공함");
        SceneManager.LoadScene(1);
    }

    public void LoginCheck()
    {
        string id = LoginInputField.IdInputField.text;
        string password = LoginInputField.PassWordInputField.text;

        LoginInputField.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);

    }

    public string Encryption(string text)
    {
        SHA256 sha256 = SHA256.Create();

        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string result = string.Empty;
        foreach (var b in hash)
        {
            result += b.ToString("X2");
        }

        return result;
    }





}
