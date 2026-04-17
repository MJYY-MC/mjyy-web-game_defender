using Godot;
using System;

public class Menu : Control {
    Label about;
	private VBoxContainer mainBox;
	private Label overScore;
	public override void _Ready() {
        about = GetNode<Label>("about");
        LoadAbout();
		mainBox = GetNode<VBoxContainer>("mainBox");
		overScore = mainBox.GetNode<Label>("overScore");

		if (DataCore.Instance.gameData.GameState == GameData.GameStateEnum.over) {
			overScore.Text = overScore.Text
				.Replace("{value}", DataCore.Instance.gameData.Score.ToString());
            overScore.Visible = true;
		}
	}

    void LoadAbout() {
        about.Text =
            "版本：" + About.version + "\n" + About.copyright;
    }

#pragma warning disable IDE0051
    private void On_startGameBtn_pressed() {
#pragma warning restore IDE0051
        if (
            DataCore.Instance.gameData.GameState != GameData.GameStateEnum.loading
			&& DataCore.Instance.gameData.GameState != GameData.GameStateEnum.running
           ) {
            if (DataCore.Instance.gameData.GameState == GameData.GameStateEnum.over)
                DataCore.Instance.InitData();
			DataCore.Instance.gameData.GameState = GameData.GameStateEnum.loading;

			DataCore.Instance.gameData.Setting.touchButton = mainBox.GetNode<CheckButton>("touchButtonCheck").Pressed;

            GetTree().ChangeScene("res://scenes/main/main.tscn");
        }
    }

    public override void _Input(InputEvent ie) {
        if (ie.IsActionPressed("key_enter") || ie.IsActionPressed("key_space")) {
            On_startGameBtn_pressed();
		}
    }
}
