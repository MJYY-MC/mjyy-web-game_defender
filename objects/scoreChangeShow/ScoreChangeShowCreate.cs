using Godot;
using Godot.Collections;

public class ScoreChangeShowCreate : Position2D
{
	[Export]
	public PackedScene ScoreChangeShowObject { get; set; }


	private Dictionary<ushort, string> allText = new Dictionary<ushort, string>() {
		{0,"这是一个测试文本" },
		{1,"发射蓄力弹" },
		{2,"发射霰弹" },
		{3,"命中" },
	};
	public void Create(ushort textKey,int score,ushort num) {
		ScoreChangeShow scsObj = (ScoreChangeShow)ScoreChangeShowObject.Instance();
		GetTree().CurrentScene.AddChild(scsObj);

		scsObj.Show(
			allText[textKey]+" "+
			(
				(score>=0)
					?"+"
					:""
			)+
			score.ToString()+
			(
				(num>1)
					?"x"+num.ToString()
					:""
			)
			);
		DataCore.Instance.gameData.Score+= score * num;

		scsObj.GlobalPosition = this.GlobalPosition;
	}
}
