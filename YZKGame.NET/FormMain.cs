using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace YZKGame.NET;
partial class FormMain : Window
{
    Thread gameThread;

    private Image bgImage;//background image(double buffer)
    private Canvas gridMain;
    private Key pressedKey = Key.None;

    public FormMain()
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        ResizeMode = ResizeMode.NoResize;
        this.Width = 600;
        this.Height = 600;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.Title = "YZKGame.NET";

        gridMain = new Canvas();
        Content = gridMain;         
  
        bgImage = new Image();
        gridMain.Children.Add(bgImage);

        this.Loaded += FormMain_Loaded;
        this.KeyDown += FormMain_KeyDown;
        this.KeyUp += FormMain_KeyUp;
    }

    public Key GetPressedKey()
    {
        return this.pressedKey;
    }

    private void FormMain_KeyUp(object sender, KeyEventArgs e)
    {
        pressedKey = Key.None;
    }

    private void FormMain_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        pressedKey = e.Key;
    }

    void FormMain_Loaded(object sender, RoutedEventArgs e)
    {
        gameThread.Start();
    }

    public void Exit()
    {
        this.Dispatcher.Invoke(Application.Current.Shutdown);
        //Environment.Exit(0);
    }

    public FormMain(Action gameMain):this()
    {
        gameThread = new Thread(() =>
        {
            gameMain();
            Exit();
        });
        gameThread.IsBackground = true;
    }

    public void Clear()
    {
        CommonHelper.Invoke(this, () => {
            gridMain.Children.Clear();
            bgImage = new Image();
            gridMain.Children.Add(bgImage);
        });
    }

    public void CreateText(int txtNum)
    {
        CommonHelper.Invoke(this,() =>
        {
            if (FindTextByNum(txtNum) != null)
            {
                throw new ArgumentException("Text with number=" + txtNum + " always exists.");
            }

            TextBlock text = new TextBlock();
            text.Tag = txtNum;
            gridMain.Children.Add(text);
        });
    }

    TextBlock FindTextByNum(int numToFind)
    {
        foreach (UIElement ctrl in gridMain.Children)
        {
            TextBlock? text = ctrl as TextBlock;
            if (text != null)
            {
                int num = (int)text.Tag;
                if (num == numToFind)
                {
                    return text;
                }
            }
        }
        return null;
    }

    public void SetTextColor(int txtNum, Brush color)
    {
        CommonHelper.Invoke(this,() =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return;
            }
            text.Foreground = color;
        });
    }

    public void SetTextFontSize(int txtNum, int size)
    {
        CommonHelper.Invoke(this,() =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return;
            }
            text.FontSize = size;
        });
    }

    public int GetTextFontSize(int txtNum)
    {
        return CommonHelper.Invoke(this, () =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {

                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return 0;
            }
            return (int)text.FontSize;
        });
    }

    public void SetTextPosition(int txtNum, int x, int y)
    {
        CommonHelper.Invoke(this,() =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return;
            }
            Canvas.SetLeft(text, x);
            Canvas.SetTop(text, y);
        });
    }

    public Point GetTextPosition(int txtNum)
    {
        return CommonHelper.Invoke(this, () =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return new Point(0,0);
            }
            double left = Canvas.GetLeft(text);
            double top = Canvas.GetTop(text);
            if(double.IsNaN(left)||double.IsNaN(top))
            {
                return new Point(0, 0);
            }
            Point point = new Point(left,top);
            return point;
        });
    }

    public Size GetTextSize(int txtNum)
    {
        return CommonHelper.Invoke(this, () =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return new Size(0, 0);
            }
            double width = text.Width;
            double height = text.Height;
            if (double.IsNaN(width) || double.IsNaN(height))
            {
                return new Size(0, 0);
            }
            Size size = new Size(width, height);
            return size;
        });
    }

    public void HideText(int txtNum)
    {
        CommonHelper.Invoke(this,() =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return;
            }
            text.Visibility = Visibility.Hidden;
        });
    }

    public void ShowText(int txtNum)
    {
        CommonHelper.Invoke(this,() =>
        {
            TextBlock text = this.FindTextByNum(txtNum);
            if (text == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return;
            }
            text.Visibility = Visibility.Visible;
        });
    }

    public void SetText(int txtNum,string text)
    {
        CommonHelper.Invoke(this,(Action)(() =>
        {
            TextBlock textBlock = this.FindTextByNum(txtNum);
            if (textBlock == null)
            {
                CommonHelper.LogError($"Cannot find text(number={txtNum})");
                return;
            }
            textBlock.Text = text;
        }));
    }

    public void CreateSprite(string spriteName,int num)
    {
        CommonHelper.Invoke(this,() =>
        {
            if (FindSpriteViewByNum(num) != null)
            {
                throw new ArgumentException($"Sprite with number={num} already exists.");
            }

            SpriteView view = new SpriteView(spriteName);
            gridMain.Children.Add(view);
            view.Tag = num;
        });
    }

    SpriteView? FindSpriteViewByNum(int numToFind)
    {
        foreach (UIElement ctrl in gridMain.Children)
        {
            SpriteView? spriteView = ctrl as SpriteView;
            if (spriteView != null)
            {
                int num = (int)spriteView.Tag;
                if (num == numToFind)
                {
                    return spriteView;
                }
            }
        }
        return null;
    }

    public void PlaySpriteAnimation(int spriteNum, string animationName,bool repeat)
    {
        CommonHelper.Invoke(this,() =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return;
            }
            spriteView.PlayAnimationAsync(animationName, repeat);
        });
    }

    public void SetSpritePosition(int spriteNum, int x, int y)
    {
        CommonHelper.Invoke(this,() =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return;
            }
            Canvas.SetLeft(spriteView, x);
            Canvas.SetTop(spriteView, y);
        });
    }

    public Point GetSpritePosition(int spriteNum)
    {
        return CommonHelper.Invoke(this,() =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return new Point(0, 0);
            }
            double x = Canvas.GetLeft(spriteView);
            double y = Canvas.GetTop(spriteView);
            if (double.IsNaN(x) || double.IsNaN(y))
            {
                return new Point(0, 0);
            }
            return new Point(x,y);
        });
    }

    public Size GetSpriteSize(int spriteNum)
    {
        return CommonHelper.Invoke(this, () =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return new Size(0, 0);
            }
            var width = spriteView.Width;
            var height = spriteView.Height;
            if (double.IsNaN(width) || double.IsNaN(height))
            {
                return new Size(0, 0);
            }
            return new Size(width,height);
        });
    }

    public void HideSprite(int spriteNum)
    {
        CommonHelper.Invoke(this,() =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return;
            }
            spriteView.Visibility = System.Windows.Visibility.Hidden;
        });
    }

    public void ShowSprite(int spriteNum)
    {
        CommonHelper.Invoke(this,() =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return;
            }
            spriteView.Visibility = System.Windows.Visibility.Hidden;
        });
    }
    public void SetSpriteFlipX(int spriteNum, bool flipX)
    {
        CommonHelper.Invoke(this, () =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return;
            }
            ScaleTransform scaleTransform = (ScaleTransform)spriteView.RenderTransform;
            scaleTransform.ScaleX = flipX ?-1:1;
            spriteView.RenderTransform = scaleTransform;
        });
    }

    public void SetSpriteFlipY(int spriteNum, bool flipY)
    {
        CommonHelper.Invoke(this, () =>
        {
            var spriteView = FindSpriteViewByNum(spriteNum);
            if (spriteView == null)
            {
                CommonHelper.LogError($"cannot find spriteview(number={spriteNum})");
                return;
            }
            ScaleTransform scaleTransform = (ScaleTransform)spriteView.RenderTransform;
            scaleTransform.ScaleY = flipY ? -1 : 1;
            spriteView.RenderTransform = scaleTransform;
        });
    }

    public void LoadBgView(string imgPath)
    {
        CommonHelper.Invoke(this,() =>
        {
            bgImage.Source = new BitmapImage(new Uri(imgPath, UriKind.Relative));
        });
    }

    public void Alert(string msg)
    {
        CommonHelper.Invoke(this, () =>
        {
            MessageBox.Show(msg);
            InvalidateVisual();
        });
    }

    public bool Confirm(string msg)
    {
        return CommonHelper.Invoke(this, () => {
            var result = MessageBox.Show(msg, " ", MessageBoxButton.YesNo)== MessageBoxResult.Yes;
            InvalidateVisual();
            return result;
        });
    }

    public bool Input(string msg, out string? value)
    {
        string? tempValue=null;
        var ret = CommonHelper.Invoke(this, () =>
        {
            var result = WindowInput.ShowInputBox(msg,out tempValue);
            InvalidateVisual();
            return result;
        });
        value = tempValue;
        return ret;
    }

    public void CreateImage(int imgNum)
    {
        CommonHelper.Invoke(this, () =>
        {
            if (FindImageByNum(imgNum) != null)
            {
                throw new ArgumentException($"Image(number={imgNum} already exists");
            }

            Image img = new Image();
            img.Tag = imgNum;
            gridMain.Children.Add(img);
        });
    }

    public void SetImageSource(int imgNum,string imgSrc)
    {
        CommonHelper.Invoke(this, () =>
        {
            var img = this.FindImageByNum(imgNum);
            if (img == null)
            {
                CommonHelper.LogError($"cannot find image(number={imgNum})");
                return;
            }
            img.Source = new BitmapImage(new Uri(imgSrc, UriKind.Absolute));
        });
    }

    Image? FindImageByNum(int numToFind)
    {
        foreach (UIElement ctrl in gridMain.Children)
        {
            Image? img = ctrl as Image;
            if (img != null&&!(img is SpriteView)&&img!=bgImage)//Spriteview is the subclass of image, so it should be excluded. 
            {
                int num = (int)img.Tag;
                if (num == numToFind)
                {
                    return img;
                }
            }
        }
        return null;
    }

    public void SetImagePosition(int imgNum, int x, int y)
    {
        CommonHelper.Invoke(this, () =>
        {
            var img = this.FindImageByNum(imgNum);
            if (img == null)
            {
                CommonHelper.LogError($"cannot find image(number={imgNum})");
            }
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
        });
    }

    public Point GetImagePosition(int imageNum)
    {
        return CommonHelper.Invoke(this, () =>
        {
            var image = this.FindImageByNum(imageNum);
            if (image == null)
            {
                CommonHelper.LogError($"cannot find image(number={imageNum})");
                return new Point(0, 0);
            }
                
            double x = Canvas.GetLeft(image);
            double y = Canvas.GetTop(image);
            if (double.IsNaN(x) || double.IsNaN(y))
            {
                return new Point(0, 0);
            }
            return new Point(x, y);
        });
    }

    public Size GetImageSize(int imageNum)
    {
        return CommonHelper.Invoke(this, () =>
        {
            var image = this.FindImageByNum(imageNum);
            if (image == null)
            {
                CommonHelper.LogError($"cannot find image(number={imageNum})");
                return new Size(0, 0);
            }
            var width = image.Width;
            var height = image.Height;
            if (double.IsNaN(width) || double.IsNaN(height))
            {
                return new Size(0, 0);
            }
            return new Size(width, height);
        });
    }

    public void HideImage(int num)
    {
        CommonHelper.Invoke(this, () =>
        {
            var img = this.FindImageByNum(num);
            if (img == null)
            {
                CommonHelper.LogError($"cannot find image(number={num})");
                return;
            }
            img.Visibility = System.Windows.Visibility.Hidden;
        });
    }

    public void ShowImage(int num)
    {
        CommonHelper.Invoke(this, () =>
        {
            var img = this.FindImageByNum(num);
            if (img == null)
            {
                CommonHelper.LogError($"cannot find image(number={num})");
                return;
            }
            img.Visibility = Visibility.Visible;
        });
    }
}