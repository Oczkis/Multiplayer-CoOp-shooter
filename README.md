# Multiplayer-CoOp-shooter

### Installation  

## Using Unity
- Download the export package [download unitypackage](https://mega.nz/file/J9MF0CoY#XsWa1SjJhqFxcfMFu9wnVoYan5wdCpY2ZWjQ1V_fcuc).
- Create a new project, choose **3D URP** template.  
- Go to **Assets** > **Import Package** > **Custom Package** and choose the downloaded package.  
- Once everything has imported, simply click play.  
- See down below on how to fully test all implementations.  

## Try it out as a standalone exe
- Download the standalone [download standalone](https://mega.nz/file/owVyWChY#R56ltizx_BFBSDOX9R50FPlHtiM7jR58HRHhc-m0h-A). 
- Run the Zadanie Testowe Programista coop.exe.  
- Choose *Host Localy*

## Try it out using steam
- Download the standalone [download standalone](https://mega.nz/file/owVyWChY#R56ltizx_BFBSDOX9R50FPlHtiM7jR58HRHhc-m0h-A). 
- Open steam
- Go to **ADD A GAME** > **Add a Non-Steam Game**
- Choose the Zadanie Testowe Programista coop.exe.  
- Choose *Lobbies* then *Host Lobby*
- You can either add friend through steam to play *(Shift + tab)* or another person can find your lobby using filter *(Lobby name will be <Hostnamefromsteam>'Lobby)*.  
  
## Game Goal
- Players defend of enemy robots that spawn every fixed timer.
- Killing a robot rewards a player with 1 point.
- Whoever gets more points by the time ends or gets amount of points required to win the game wins.
  
## Controls
- Use WASD to move your character. Your character will move regarding which way he is facing. Character always faces your cursor position.
- Hold or click left mouse button to shoot.
- Press tab to check scoreboard.
  
## Implementations
- Owner can choose game settings after hosting lobby. **Game Duration** or **Score Goal** if it is being tested throught **Unity** these settings can be set from inspector
- Whole project uses global game timer, based on tick. Which means time between shots or AI calculations rely on global timer. For example AI makes calculation which target to choose every 12 ticks, if the global timer is set to 4 ticks per second AI will reconsider its actions every 3 seconds.
- After lobby starts players are asked to ready up, once players are ready the countdown will start which can be stopped at any time.
- Once the round is over, players can ready up and the game will restart.
