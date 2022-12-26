using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace YZKGame.NET;

public static  class GameCore
{
    private static bool isStarted = false;
    private static FormMain formMain;

    private static void CheckStarted()
    {
        if (!isStarted)
        {
            string msg = "Start() was not invoked. Do not call members of GameCore directly from Main.";
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw new Exception(msg);
        }
    }

    public static void Start(Action gameMain)
    {
        if (isStarted)
        {
            return;
        }
        isStarted = true;
        //WPF application should run in STA mode.
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
        formMain = new FormMain(gameMain);
        Application app = new Application();
        app.Run(formMain);
    }

    public static Key GetPressedKey()
    {
        CheckStarted();
        return formMain.GetPressedKey();
    }

    public static void Clear()
    {
        CheckStarted();
        formMain.Clear();
    }

    public static void Exit()
    {
        CheckStarted();
        formMain.Exit();
    }

    
    public static void Pause(int millSeconds)
    {
        CheckStarted();
        Thread.Sleep(millSeconds);
    }

    public static void SetGameTitle(string title)
    {
        CheckStarted();
        CommonHelper.Invoke(formMain, () =>
        {
            formMain.Title = title;
        });            
    }

    public static void SetGameSize(int width,int height)
    {
        CheckStarted();
        CommonHelper.Invoke(formMain, () =>
        {
            formMain.Width = width;
            formMain.Height = height;
        });
    }

    public static int GetGameWidth()
    {
        CheckStarted();
        return CommonHelper.Invoke(formMain, () =>
        {
            FrameworkElement content = (FrameworkElement)formMain.Content;
            return (int)content.ActualWidth;
        });
    }

    public static int GetGameHeight()
    {
        CheckStarted();
        return CommonHelper.Invoke(formMain, () =>
        {
            FrameworkElement content = (FrameworkElement)formMain.Content;
            return (int)content.ActualHeight;
        });
    }

    public static void CreateSprite(int num,string spriteName)
    {
        CheckStarted();
        formMain.CreateSprite(num, spriteName);
    }

    public static void PlaySpriteAnimation(int spriteNum, string animationName)
    {
        CheckStarted();
        PlaySpriteAnimation(spriteNum, animationName, false);
    }

    public static void PlaySpriteAnimation(int spriteNum, string animationName, bool repeat)
    {
        CheckStarted();
        formMain.PlaySpriteAnimation(spriteNum, animationName, repeat);
    }

    public static void SetSpritePosition(int spriteNum, int x, int y)
    {
        CheckStarted();
        formMain.SetSpritePosition(spriteNum, x, y);
    }

    public static int GetSpritePositionX(int spriteNum)
    {
        CheckStarted();
        var pos = formMain.GetSpritePosition(spriteNum);
        return (int)pos.X;
    }

    public static int GetSpritePositionY(int spriteNum)
    {
        CheckStarted();
        var pos = formMain.GetSpritePosition(spriteNum);
        return (int)pos.Y;
    }

    public static int GetSpriteWidth(int spriteNum)
    {
        CheckStarted();
        var size = formMain.GetSpriteSize(spriteNum);
        return (int)size.Width;
    }

    public static int GetSpriteHeight(int spriteNum)
    {
        CheckStarted();
        var size = formMain.GetSpriteSize(spriteNum);
        return (int)size.Height;
    }

    public static void HideSprite(int spriteNum)
    {
        CheckStarted();
        formMain.HideSprite(spriteNum);
    }

    public static void ShowSprite(int spriteNum)
    {
        CheckStarted();
        formMain.ShowSprite(spriteNum);
    }

   

    public static void SetSpriteFlipX(int spriteNum, bool flipX)
    {
        CheckStarted();
        formMain.SetSpriteFlipX(spriteNum, flipX);
    }

    public static void SetSpriteFlipY(int spriteNum, bool flipY)
    {
        CheckStarted();
        formMain.SetSpriteFlipY(spriteNum, flipY);
    }

    public static void PlaySound(string soundName, bool repeat=false)
    {
        CheckStarted();
        string musicPath = CommonHelper.MapPath("Sounds/" + soundName);
        CommonHelper.PlaySound(musicPath, repeat);
    }

    public static void CloseSound(string soundName)
    {
        CheckStarted();
        string musicPath = CommonHelper.MapPath("Sounds/" + soundName);
        CommonHelper.CloseSound(musicPath);
    }

    public static void CreateImage(int imgNum)
    {
        CheckStarted();
        formMain.CreateImage(imgNum);
    }

    public static void CreateImage(int imgNum,string imgSrc)
    {
        CheckStarted();
        formMain.CreateImage(imgNum);
        string imgPath = CommonHelper.MapPath("Images/" + imgSrc);
        formMain.SetImageSource(imgNum, imgPath);
    }

    public static void SetImageSource(int imgNum, string imgName)
    {
        CheckStarted();
        string imgPath = CommonHelper.MapPath("Images/" + imgName);
        formMain.SetImageSource(imgNum,imgPath);
    }

    public static void SetImagePosition(int imgNum, int x, int y)
    {
        CheckStarted();
        formMain.SetImagePosition(imgNum, x, y);
    }

    public static int GetImagePositionX(int imgNum)
    {
        CheckStarted();
        var pos = formMain.GetImagePosition(imgNum);
        return (int)pos.X;
    }

    public static int GetImagePositionY(int imgNum)
    {
        CheckStarted();
        var pos = formMain.GetImagePosition(imgNum);
        return (int)pos.Y;
    }

    public static int GetImageWidth(int imgNum)
    {
        CheckStarted();
        var size = formMain.GetImageSize(imgNum);
        return (int)size.Width;
    }

    public static int GetImageHeight(int imgNum)
    {
        CheckStarted();
        var size = formMain.GetImageSize(imgNum);
        return (int)size.Height;
    }


    public static void HideImage(int imgNum)
    {
        CheckStarted();
        formMain.HideImage(imgNum);
    }

    public static void ShowImage(int imgNum)
    {
        CheckStarted();
        formMain.ShowImage(imgNum);
    }

    /// <summary>
    /// Load background image
    /// </summary>
    /// <param name="imgName"></param>
    public static void LoadBgView(string imgName)
    {
        CheckStarted();
        string musicPath = CommonHelper.MapPath("Images/" + imgName);
        formMain.LoadBgView(musicPath);
    }

    public static void CreateText(int txtNum)
    {
        CheckStarted();
        formMain.CreateText(txtNum);
    }

    public static void SetTextPosition(int txtNum, int x, int y)
    {
        CheckStarted();
        formMain.SetTextPosition(txtNum, x, y);
    }

    public static void SetText(int txtNum, string text)
    {
        CheckStarted();
        formMain.SetText(txtNum, text);
    }

    public static void SetTextColor(int txtNum, Color color)
    {
        CheckStarted();
        SolidColorBrush brush = new SolidColorBrush(color);
        SetTextColor(txtNum, brush);
    }

    public static void SetTextColor(int txtNum, Brush color)
    {
        CheckStarted();
        formMain.SetTextColor(txtNum, color);
    }

    public static void SetTextFontSize(int txtNum, int size)
    {
        CheckStarted();
        formMain.SetTextFontSize(txtNum, size);
    }

    public static void HideText(int txtNum)
    {
        CheckStarted();
        formMain.HideText(txtNum);
    }

    public static void ShowText(int txtNum)
    {
        CheckStarted();
        formMain.ShowText(txtNum);
    }

    public static int GetTextPositionX(int txtNum)
    {
        CheckStarted();
        var pos = formMain.GetTextPosition(txtNum);
        return (int)pos.X;
    }

    public static int GetTextPositionY(int txtNum)
    {
        CheckStarted();
        var pos = formMain.GetTextPosition(txtNum);
        return (int)pos.Y;
    }

    public static int GetTextWidth(int txtNum)
    {
        CheckStarted();
        var size = formMain.GetTextSize(txtNum);
        return (int)size.Width;
    }

    public static int GetTextHeight(int txtNum)
    {
        CheckStarted();
        var size = formMain.GetTextSize(txtNum);
        return (int)size.Height;
    }

    public static void ShowTypeName(Object obj)
    {
        if(obj==null)
        {
            Alert("null");
            return;
        }
        Type type = obj.GetType();
        Dictionary<Type, string> dict = new Dictionary<Type, string>();
        dict[typeof(Single)] = "float";
        dict[typeof(Double)] = "double";
        dict[typeof(Int16)] = "short";
        dict[typeof(Int32)] = "int";
        dict[typeof(Int64)] = "long";
        dict[typeof(Boolean)] = "bool";
        if (dict.ContainsKey(type))
        {
            Alert(dict[type]);
            return;
        }
        Alert(obj.GetType().Name);
    }
    public static void Alert(object? msg)
    {
        Alert(Convert.ToString(msg));
    }

    public static void Alert(string msg)
    {
        CheckStarted();
        formMain.Alert(msg);
    }

    public static bool Confirm(string msg)
    {
        CheckStarted();
        return formMain.Confirm(msg);
    }

    public static bool Input(string msg,out string? inputValue)
    {
        CheckStarted();
        return formMain.Input(msg,out inputValue);
    }

    public static string? Input(string msg)
    {
        string? inputValue;
        if(Input(msg,out inputValue))
        {
            return inputValue;
        }
        else
        {
            return null;
        }
    }
}
