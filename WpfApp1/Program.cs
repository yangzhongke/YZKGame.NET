using System;
using System.Windows.Input;
using System.Windows.Media;
using YZKGame.NET;

GameCore.Start(GameMain);
static void GameMain()
{
    /*
    GameCore.CreateSound(1,"a.mp3");
    GameCore.Pause(2000);
    GameCore.PauseSound(1);
    GameCore.Pause(2000);
    GameCore.PlaySound(1);
    GameCore.Pause(5000);
    GameCore.StopSound(1);
    GameCore.Pause(2000);
    GameCore.PlaySound(1);
    GameCore.Pause(5000);*/
    GameCore.CreateText(0, "杨中科");
    GameCore.SetTextColor(0, Colors.Red);
    /*
    while(true)
    {
        var p = GameCore.GetMousePosition();
        GameCore.SetText(0,p.ToString());
    }*/
    while (true)
    {
        if (GameCore.IsKeyDown(Key.Right) && GameCore.IsKeyDown(Key.Down))
        {
            GameCore.SetGameTitle("xxxx");
        }
        else
        {
            GameCore.SetGameTitle("yyyyy");
        }
    }
    GameCore.Alert(0);
    return;


    for (int i = 0; i < 500; i++)
    {
        //GameCore.SetGameSize(GameCore.GetGameWidth()+1,GameCore.GetGameHeight()+1);
        //GameCore.Pause(10);
    }

    GameCore.LoadBgView("field.png");

    GameCore.CreateImage(999);
    GameCore.SetImageSource(999, "14.png");
    for (int i = 100; i <= 200; i++)
    {
        GameCore.SetImagePosition(999, i, 200);
        //GameCore.Pause(10);
        //GameCore.SetText(333, "a");
    }
    GameCore.CreateSprite(1,"guizi");
    GameCore.PlaySpriteAnimation(1, "shenlanyao", true);

    GameCore.CreateSprite(2, "bird");
    GameCore.SetSpritePosition(2, 100, 100);
    GameCore.PlaySpriteAnimation(2, "fly", true);

    GameCore.Pause(1000);
    GameCore.SetSpriteFlipY(2, true);
    GameCore.Pause(1000);
    GameCore.SetSpriteFlipY(2, false);

    GameCore.CreateText(0);
    GameCore.SetTextPosition(0, 100, 100);
    GameCore.SetTextFontSize(0, 30);


    while (true)
    {
        Key key = GameCore.GetPressedKey();

        if (key == Key.Left)
        {
            int x = GameCore.GetSpritePositionX(1);
            int y = GameCore.GetSpritePositionY(1);
            GameCore.SetSpritePosition(1, x - 1, y);
        }


        if (key == Key.Right)
        {
            int x = GameCore.GetSpritePositionX(1);
            int y = GameCore.GetSpritePositionY(1);
            GameCore.SetSpritePosition(1, x + 1, y);
        }

        if (key == Key.Up)
        {
            int x = GameCore.GetSpritePositionX(1);
            int y = GameCore.GetSpritePositionY(1);
            GameCore.SetSpritePosition(1, x, y - 1);
        }
        if (key == Key.Down)
        {
            int x = GameCore.GetSpritePositionX(1);
            int y = GameCore.GetSpritePositionY(1);
            GameCore.SetSpritePosition(1, x, y + 1);
        }
        if (key == Key.A)
        {
            Console.WriteLine(DateTime.Now + "：A");
        }
        if (key == Key.Escape)
        {
            GameCore.Clear();
        }
        GameCore.Pause(10);
    }
}