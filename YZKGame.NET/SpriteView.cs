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
        private List<string> currentFrames = new List<string>();//the images of current animation
        private int currentFrameIndex = 0;
        private bool isRepeat;
        
        public string SpriteName { get; private set; }

        private string spritePath;
        private DispatcherTimer playTimer = new DispatcherTimer();

        public SpriteView(string spriteName)
        {
            spritePath = CommonHelper.MapPath("Sprites/" + spriteName);
            if (!Directory.Exists(spritePath))
            {
                throw new Exception("Sprite[" + spriteName + "] directory "
                    + spritePath + " not found.");
            }
            
            this.SpriteName = spriteName;
            this.playTimer.Interval = TimeSpan.FromMilliseconds(200);
            this.playTimer.Tick += playTimer_Tick;
        }

        /// <summary>
        /// Get the size of the largest image in the frame animation
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        static Size GetMaxPicSize(string[] files)
        {
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

            //Switch to the next image for the next Timer to play
            if (currentFrameIndex < currentFrames.Count - 1)
            {
                currentFrameIndex++;
            }
            else if (isRepeat)
            {
                currentFrameIndex = 0;//If the last one is played, the first one is replayed
            }
        }

        public void PlayAnimationAsync(string animationName, bool repeat)
        {
            this.isRepeat = repeat;

            playTimer.Stop();
            string animationPath = Path.Combine(spritePath, animationName);
            if (!Directory.Exists(animationPath))
            {
                throw new ArgumentException("Aniate[" + animationName + "] directory "
                    + animationPath + " not found");
            }
            currentFrames.Clear();
            string[] pngFiles = Directory.GetFiles(animationPath, "*.png");
            string[] jpgFiles = Directory.GetFiles(animationPath, "*.jpg");
            string[] files = pngFiles.Concat(jpgFiles).ToArray();
            Array.Sort(files);
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
