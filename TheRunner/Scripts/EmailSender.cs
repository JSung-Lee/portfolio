using UnityEngine;

public class EmailSender : MonoBehaviour
{
    public UserManager user;
    public void OnClickEvent()
    {
        string mailto = "imallgame@naver.com";
        string subject = EscapeURL("아이몰 게임 The Runner 버그 리포트 및 문의사항");
        string body = EscapeURL
            (
             "이 곳에 내용을 작성해주세요.\n\n\n\n" +
             "________\n" +
             "Device Model : " + SystemInfo.deviceModel + "\n" +
             "Device OS : " + SystemInfo.operatingSystem + "\n" +
             "GameVersion : " + user.version.ToString()  + "\n" +
             "닉네임 : " + user.nickname + "\n" +
             "iMallID : " + user.imallid + "\n" +
             "Phone : " + user.phoneNumber + "\n" +
             "________"
            );

        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }

    private string EscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

}