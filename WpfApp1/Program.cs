﻿using System;
using System.Windows.Input;
using System.Windows.Media;
using YZKGame.NET;

GameCore.Start(GameMain);
static void GameMain()
{
    GameCore.SetGameSize(1000, 800);
    GameCore.PlaySound("ab.mp3", true);
    GameCore.SetGameTitle("Game Demo");

    for (int i = 0; i < 500; i++)
    {
        //GameCore.SetGameSize(GameCore.GetGameWidth()+1,GameCore.GetGameHeight()+1);
        //GameCore.Pause(10);
    }

    GameCore.LoadBgView("大片草地.png");

    GameCore.CreateImage(999);
    GameCore.SetImageSource(999, "14.png");
    for (int i = 100; i <= 200; i++)
    {
        GameCore.SetImagePosition(999, i, 200);
        //GameCore.Pause(10);
        //GameCore.SetText(333, "a");
    }
    GameCore.CreateSprite("guizi", 1);
    GameCore.PlaySpriteAnimate(1, "shenlanyao", true);

    GameCore.CreateSprite("bird", 2);
    GameCore.SetSpritePosition(2, 100, 100);
    GameCore.PlaySpriteAnimate(2, "fly", true);

    GameCore.Pause(1000);
    GameCore.SetSpriteFlipY(2, true);
    GameCore.Pause(1000);
    GameCore.SetSpriteFlipY(2, false);

    GameCore.CreateText(0);
    GameCore.SetTextPosition(0, 100, 100);
    GameCore.SetTextFontSize(0, 30);
    GameCore.SetTextColor(0, Brushes.Red);


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
            GameCore.PlaySound("打拳.mp3", false);
            Console.WriteLine(DateTime.Now + "：A");
        }
        if (key == Key.Escape)
        {
            GameCore.Clear();
        }
        GameCore.Pause(10);
    }
}