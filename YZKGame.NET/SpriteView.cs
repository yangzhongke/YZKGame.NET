using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace YZKGame.NET
{
    class SpriteView : Image
    {
        private List<string> currentFrames = new List<string>();//当前精灵、当前动作的帧图片路径
        private int currentFrameIndex = 0;//当前帧的序号
        private bool isRepeat;//是否重复播放
        
        public string SpriteName { get; private set; }//精灵的名字

        private string spritePath;//精灵文件的目录
        private DispatcherTimer playTimer = new DispatcherTimer();//播放动画用的定时器

        public SpriteView(string spriteName)
        {
            spritePath = CommonHelper.MapPath("Sprites/" + spriteName);
            if (!Directory.Exists(spritePath))
            {
                throw new Exception("Sprite[" + spriteName + "] directory "
                    + spritePath + " not found。找不到名字为" + spriteName + "的精灵");
            }
            
            this.SpriteName = spriteName;
            this.playTimer.Interval = TimeSpan.FromMilliseconds(200);//0.2秒切换一次
            this.playTimer.Tick += playTimer_Tick;
        }

        /// <summary>
        /// 获得帧动画中尺寸最大的图片的尺寸
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        static Size GetMaxPicSize(string[] files)
        {
            //以宽高中的最大值为准
            double maxWidth = 0, maxHeight = 0;
            foreach (string file in files)
            {
                BitmapImage img = new BitmapImage(new Uri(file, UriKind.Absolute));
                if (img.Width > maxWidth)
                {
                    maxWidth = img.Width;
                }
                if (img.Height > maxHeight)
                {
                    maxHeight = img.Height;
                }
            }
            return new Size(maxWidth, maxHeight);
        }

        void playTimer_Tick(object sender, EventArgs e)
        {
            if (currentFrameIndex <= currentFrames.Count - 1)
            {
                this.Source = new BitmapImage(new Uri(currentFrames[currentFrameIndex]));
            }

            //切换为下一张图片，供下次Timer播放
            if (currentFrameIndex < currentFrames.Count - 1)
            {
                currentFrameIndex++;
            }
            else if (isRepeat)
            {
                currentFrameIndex = 0;//如果播放到最后一张，则重新播放第一张
            }
        }

        public void PlayAnimationAsync(string animationName, bool repeat)
        {
            this.isRepeat = repeat;

            playTimer.Stop();
            string animationPath = Path.Combine(spritePath, animationName);
            if (!Directory.Exists(animationPath))
            {
                throw new Exception("Aniate[" + animationName + "] directory "
                    + animationPath + " not found" + "。找不到名字为" + animationName + "的动画");
            }
            currentFrames.Clear();
            string[] pngFiles = Directory.GetFiles(animationPath, "*.png");
            string[] jpgFiles = Directory.GetFiles(animationPath, "*.jpg");
            string[] files = pngFiles.Concat(jpgFiles).ToArray();
            Array.Sort<string>(files);//根据文件名排序
            currentFrames.AddRange(files);

            var maxPicSize = GetMaxPicSize(files);
            this.Width = maxPicSize.Width;
            this.Height = maxPicSize.Height;
            this.Stretch = System.Windows.Media.Stretch.Uniform;

            ScaleTransform scaleTransform = new ScaleTransform();            
            scaleTransform.CenterX = this.Width / 2;
            scaleTransform.CenterY = this.Height / 2;
            this.RenderTransform = scaleTransform;
            playTimer.Start();
        }
    }
}
