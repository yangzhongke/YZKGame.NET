using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YZKGame.NET
{
    partial class FormMain : Window
    {
        Thread gameThread;

        private Image bgImage;//背景图片（内存缓冲）
        private Canvas gridMain;
        private Key pressedKey = Key.None;

        public FormMain()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            ResizeMode = System.Windows.ResizeMode.NoResize;
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

        public FormMain(Action gameMain):this()
        {
            gameThread = new Thread(() =>
            {
                gameMain();
                Environment.Exit(0);//gameMain结束程序就退出
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
                if (FindLabelByNum(txtNum) != null)
                {
                    throw new ArgumentException("编号为" + txtNum + "的文本已经存在，不能重复创建");
                }

                TextBlock label = new TextBlock();
                label.Tag = txtNum;
                gridMain.Children.Add(label);
            });
        }

        TextBlock FindLabelByNum(int numToFind)
        {
            foreach (UIElement ctrl in gridMain.Children)
            {
                TextBlock label = ctrl as TextBlock;
                if (label != null)
                {
                    int num = (int)label.Tag;
                    if (num == numToFind)
                    {
                        return label;
                    }
                }
            }
            return null;
        }

        public void SetTextColor(int labelNum, Brush color)
        {
            CommonHelper.Invoke(this,() =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    CommonHelper.LogError("找不到编号为" + labelNum + "的label");
                    return;
                }
                label.Foreground = color;
            });
        }

        public void SetTextFontSize(int labelNum, int size)
        {
            CommonHelper.Invoke(this,() =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    //throw new ArgumentException("找不到编号为" + labelNum + "的label");
                    CommonHelper.LogError("找不到编号为" + labelNum + "的label");
                    return;
                }
                label.FontSize = size;
            });
        }

        public int GetTextFontSize(int labelNum)
        {
            int? value= (int?)CommonHelper.Invoke(this, () =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    //throw new ArgumentException("找不到编号为" + labelNum + "的label");
                    CommonHelper.LogError("找不到编号为" + labelNum + "的label");
                    return 0;
                }
                return label.FontSize;
            });
            return value ?? 0;
        }

        public void SetTextPosition(int labelNum, int x, int y)
        {
            CommonHelper.Invoke(this,() =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    //throw new ArgumentException("找不到编号为" + labelNum + "的label");
                    CommonHelper.LogError("找不到编号为" + labelNum + "的label");
                    return;
                }
                Canvas.SetLeft(label, x);
                Canvas.SetTop(label, y);
            });
        }

        public Point GetTextPosition(int labelNum)
        {
            Point ? value= (Point?)CommonHelper.Invoke(this, () =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    //throw new ArgumentException("找不到编号为" + labelNum + "的label");
                    CommonHelper.LogError("找不到编号为" + labelNum + "的label");
                    return new Point(0,0);
                }
                double left = Canvas.GetLeft(label);
                double top = Canvas.GetTop(label);
                if(double.IsNaN(left)||double.IsNaN(top))
                {
                    return new Point(0, 0);
                }
                Point point = new Point(left,top);
                return point;
            });
            return value ?? new Point(0,0);
        }

        public Size GetTextSize(int labelNum)
        {
            Size? value= (Size?)CommonHelper.Invoke(this, () =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    //throw new ArgumentException("找不到编号为" + labelNum + "的label");
                    CommonHelper.LogError("找不到编号为" + labelNum + "的label");
                    return new Size(0, 0);
                }
                double width = label.Width;
                double height = label.Height;
                if (double.IsNaN(width) || double.IsNaN(height))
                {
                    return new Size(0, 0);
                }
                Size size = new Size(width, height);
                return size;
            });
            return value ?? new Size(0, 0);
        }

        public void HideText(int labelNum)
        {
            CommonHelper.Invoke(this,() =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    CommonHelper.LogError("找不到编号为" + labelNum + "的Label");
                    return;
                    // throw new ArgumentException("找不到编号为" + labelNum + "的Label");
                }
                label.Visibility = System.Windows.Visibility.Hidden;
            });
        }

        public void ShowText(int labelNum)
        {
            CommonHelper.Invoke(this,() =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    CommonHelper.LogError("找不到编号为" + labelNum + "的Label");
                    return;
                    //throw new ArgumentException("找不到编号为" + labelNum + "的Label");
                }
                label.Visibility = System.Windows.Visibility.Visible;
            });
        }

        public void SetText(int labelNum,string text)
        {
            CommonHelper.Invoke(this,() =>
            {
                TextBlock label = this.FindLabelByNum(labelNum);
                if (label == null)
                {
                    CommonHelper.LogError("找不到编号为" + labelNum + "的Label");
                    return;
                    //throw new ArgumentException("找不到编号为" + labelNum + "的Label");
                }
                label.Text = text;
            });
        }

        /// <summary>
        /// 创建精灵
        /// </summary>
        /// <param name="spriteName">精灵名字</param>
        /// <param name="num">使用者给精灵定的编号</param>
        public void CreateSprite(string spriteName,int num)
        {
            CommonHelper.Invoke(this,() =>
            {
                if (FindSpriteViewByNum(num) != null)
                {
                    throw new ArgumentException("编号为" + num + "的Sprite已经存在，不能重复创建");
                }

                SpriteView view = new SpriteView(spriteName);
                gridMain.Children.Add(view);
                view.Tag = num;
            });
        }

        /// <summary>
        /// 查找编号为numToFind的精灵
        /// </summary>
        /// <param name="numToFind"></param>
        /// <returns></returns>
        SpriteView FindSpriteViewByNum(int numToFind)
        {
            foreach (UIElement ctrl in gridMain.Children)
            {
                SpriteView spriteView = ctrl as SpriteView;
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

        public void PlaySpriteAnimate(int spriteNum, string animateName,bool repeat)
        {
            CommonHelper.Invoke(this,() =>
            {
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return;
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
                }
                spriteView.PlayAnimateAsync(animateName, repeat);
            });
        }

        public void SetSpritePosition(int spriteNum, int x, int y)
        {
            CommonHelper.Invoke(this,() =>
            {
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return;
                    //throw new ArgumentException("找不到编号为"+spriteView+"的精灵");
                }
                Canvas.SetLeft(spriteView, x);
                Canvas.SetTop(spriteView, y);
            });
        }

        public Point GetSpritePosition(int spriteNum)
        {
            var pos = (Point ?)CommonHelper.Invoke(this,() =>
            {
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return new Point(0, 0);
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
                }
                double x = Canvas.GetLeft(spriteView);
                double y = Canvas.GetTop(spriteView);
                if (double.IsNaN(x) || double.IsNaN(y))
                {
                    return new Point(0, 0);
                }
                return new Point(x,y);
            });
            return pos ?? new Point(0, 0);
        }

        public Size GetSpriteSize(int spriteNum)
        {
            var size = (Size ?)CommonHelper.Invoke(this, () =>
            {
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return new Point(0, 0);
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
                }
                var width = spriteView.Width;
                var height = spriteView.Height;
                if (double.IsNaN(width) || double.IsNaN(height))
                {
                    return new Size(0, 0);
                }
                return new Size(width,height);
            });
            return size ?? new Size(0, 0);
        }

        public void HideSprite(int spriteNum)
        {
            CommonHelper.Invoke(this,() =>
            {
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return;
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
                }
                spriteView.Visibility = System.Windows.Visibility.Hidden;
            });
        }

        public void ShowSprite(int spriteNum)
        {
            CommonHelper.Invoke(this,() =>
            {
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return;
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
                }
                spriteView.Visibility = System.Windows.Visibility.Hidden;
            });
        }
        public void SetSpriteFlipX(int spriteNum, bool flipX)
        {
            CommonHelper.Invoke(this, () =>
            {
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return;
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
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
                SpriteView spriteView = FindSpriteViewByNum(spriteNum);
                if (spriteView == null)
                {
                    CommonHelper.LogError("找不到编号为" + spriteView + "的精灵");
                    return;
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
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
            bool? ret = (bool?)CommonHelper.Invoke(this, () => {
                var result = MessageBox.Show(msg, " ", MessageBoxButton.YesNo)== MessageBoxResult.Yes;
                InvalidateVisual();
                return result;
            });            
            return  ret??false;
        }

        public bool Input(string msg, out string value)
        {
            String tempValue=null;
            bool? ret = (bool?)CommonHelper.Invoke(this, () =>
            {
                var result = WindowInput.ShowInputBox(msg,out tempValue);
                InvalidateVisual();
                return result;
            });
            value = tempValue;
            return ret??false;
        }

        public void CreateImage(int imgNum)
        {
            CommonHelper.Invoke(this, () =>
            {
                if (FindImageByNum(imgNum) != null)
                {
                    throw new ArgumentException("编号为" + imgNum + "的图片已经存在，不能重复创建");
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
                Image img = this.FindImageByNum(imgNum);
                if (img == null)
                {
                    CommonHelper.LogError("找不到编号为" + imgNum + "的图片");
                    return;
                    //throw new ArgumentException("找不到编号为" + imgNum + "的图片");
                }
                img.Source = new BitmapImage(new Uri(imgSrc, UriKind.Absolute));
            });
        }

        Image FindImageByNum(int numToFind)
        {
            foreach (UIElement ctrl in gridMain.Children)
            {
                Image img = ctrl as Image;
                if (img != null&&!(img is SpriteView)&&img!=bgImage)//Spriteview是从Image继承的，要排除掉
                    //排除掉背景图
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
                Image img = this.FindImageByNum(imgNum);
                if (img == null)
                {
                    CommonHelper.LogError("找不到编号为" + imgNum + "的图片");
                    return;
                    //throw new ArgumentException("找不到编号为" + imgNum + "的图片");
                }
                Canvas.SetLeft(img, x);
                Canvas.SetTop(img, y);
            });
        }

        public Point GetImagePosition(int imageNum)
        {
            var pos = (Point?)CommonHelper.Invoke(this, () =>
            {
                Image image = this.FindImageByNum(imageNum);
                if (image == null)
                {
                    CommonHelper.LogError("找不到编号为" + imageNum + "的图片");
                    return new Point(0, 0);
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
                }
                
                double x = Canvas.GetLeft(image);
                double y = Canvas.GetTop(image);
                if (double.IsNaN(x) || double.IsNaN(y))
                {
                    return new Point(0, 0);
                }
                return new Point(x, y);
            });
            return pos ?? new Point(0, 0);
        }

        public Size GetImageSize(int imageNum)
        {
            var size = (Size?)CommonHelper.Invoke(this, () =>
            {
                Image image = this.FindImageByNum(imageNum);
                if (image == null)
                {
                    CommonHelper.LogError("找不到编号为" + imageNum + "的图片");
                    return new Size(0, 0);
                    //throw new ArgumentException("找不到编号为" + spriteView + "的精灵");
                }
                var width = image.Width;
                var height = image.Height;
                if (double.IsNaN(width) || double.IsNaN(height))
                {
                    return new Size(0, 0);
                }
                return new Size(width, height);
            });
            return size??new Size(0,0);
        }

        public void HideImage(int num)
        {
            CommonHelper.Invoke(this, () =>
            {
                Image img = this.FindImageByNum(num);
                if (img == null)
                {
                    CommonHelper.LogError("找不到编号为" + num + "的图片");
                    return;
                    //throw new ArgumentException("找不到编号为" + num + "的图片");
                }
                img.Visibility = System.Windows.Visibility.Hidden;
            });
        }

        public void ShowImage(int num)
        {
            CommonHelper.Invoke(this, () =>
            {
                Image img = this.FindImageByNum(num);
                if (img == null)
                {
                    CommonHelper.LogError("找不到编号为" + num + "的图片");
                    return;
                    //throw new ArgumentException("找不到编号为" + num + "的图片");
                }
                img.Visibility = System.Windows.Visibility.Visible;
            });
        }
    }
}
