# Visual Studio Build & Notify

Tired of long build process that you can do nothing about? With this Visual Studio plugin you can slack off a little and get notification when the build is done!

![Compiling!](https://imgs.xkcd.com/comics/compiling.png)

Project came to live to check how to develop plugins for Visual Studio. Now I know that i don't like it, but this thing is nice and handy sometimes.

## How to use it?

Start the solution build via **Tools -> Build Solution and Notify** (or just hit CTRL + F6). The notification with the build status will be send after build completion. (Builds started in a different way do not trigger notifications by design.)

Notification channel configuration can be done via **Tools -> Options -> Build Notify**.

## Requirements
Developed and tested in **Visual Studio 2017**.

## Supported notification channels
* MessageBox (mainly for testing purposes),
* Pushbullet pushes

(And I don't know what else could be useful. Mail? SMS? Azure Notification Hub pushes?)

*"[Compiling](https://xkcd.com/303/)" comic comes from [XKCD](https://xkcd.com/) series by Randall Munroe.*