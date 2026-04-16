using Godot;
using System;

public class Menu : Control
{
    Label about;
    public override void _Ready()
    {
        about = GetNode<Label>("about");

        LoadAbout();
	}

    void LoadAbout() {
        about.Text =
            "版本：" + About.version + "\n" + About.copyright;
	}

#pragma warning disable IDE0051
	private void On_startGameBtn_pressed() {
#pragma warning restore IDE0051
        DataCore.Instance.gameData.GameState = GameData.GameStateEnum.loading;

		GetTree().ChangeScene("res://scenes/main/main.tscn");
	}
}
