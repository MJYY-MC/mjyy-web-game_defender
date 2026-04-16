using Godot;
using System;

public class GameOver : Control
{
    public override void _Ready()
    {
        
    }

#pragma warning disable IDE0051
	private void On_restartGameBtn_pressed() {
#pragma warning restore IDE0051
		DataCore.Instance.InitData();

		DataCore.Instance.gameData.GameState = GameData.GameStateEnum.loading;

		GetTree().ChangeScene("res://scenes/main/main.tscn");
	}
}
