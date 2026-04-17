using Godot;
using System;

public class ScoreShow : Label
{
    Label scoreValue;
    public override void _Ready(){
        scoreValue = GetNode<Label>("value");

        Score_Changed(DataCore.Instance.gameData.Score);
        DataCore.Instance.gameData.Score_OnChanged += Score_Changed;
    }
	void Score_Changed(int sv) => scoreValue.Text = sv.ToString();
}
