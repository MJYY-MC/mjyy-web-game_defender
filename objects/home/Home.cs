using Godot;

public class Home : StaticBody2D
{
	Sprite healthShow;
	public override void _Ready()
    {
        DataCore.Instance.gameData.HomeHealth_OnChanged += HomeHealth_Changed;

		healthShow = GetNode<Sprite>("health");
	}

	private void HomeHealth_Changed(float h) {
		healthShow.Modulate = new Color(
			healthShow.Modulate.r, healthShow.Modulate.g, healthShow.Modulate.b, 
			(1 - DataCore.Instance.gameData.GetHomeHealthPercent())
			);
	}

#pragma warning disable IDE0051
	private void OnHomeBody_entered(object body) {
#pragma warning restore IDE0051
		if(body is Node nodeObj) {
			if (
				   nodeObj.IsInGroup("bullet") 
				&& body is RigidBody2D rigBody
				) {
				float speed = rigBody.LinearVelocity.Length();
				DataCore.Instance.gameData.HomeHealth -= speed * 0.01f;


			}
		}
	}
}
