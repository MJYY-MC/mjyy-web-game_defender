using Godot;
using System;

public class ScoreChangeShow : RigidBody2D
{
	/// <summary>
	/// 消失动画持续时间
	/// </summary>
	[Export]
	public float GoneAnimationDuration { get; set; } = 0.5f;
	
	Label text;
    CollisionShape2D coll;
	Tween goneAnim;
    Timer doGoneTimer;
	public override void _Ready()
    {
        text=GetNode<Label>("text");
        coll=GetNode<CollisionShape2D>("coll");

		goneAnim=GetNode<Tween>("goneAnim");
		doGoneTimer=GetNode<Timer>("doGoneTimer");
	}

	
	public void Show(string inputText) {
        text.Text = inputText;
        {
            Vector2 getMinRectSize = text.GetCombinedMinimumSize();//获取其最小尺寸，避免立即更改文本后尺寸没有立即更新
            text.RectPosition = new Vector2(getMinRectSize.x / -2, getMinRectSize.y / -2); //new Vector2(text.RectSize.x / -2, text.RectSize.y / -2);
        }
		if (coll.Shape is RectangleShape2D rs) {
            rs.Extents = text.RectSize;
        }
        this.Visible = true;

        doGoneTimer.Start();
	}

#pragma warning disable IDE0051
	private void On_doGoneTimer_timeout() {
#pragma warning restore IDE0051
		goneAnim.InterpolateProperty(
						this,
						"modulate:a",
						this.Modulate.a,
						0f,
						GoneAnimationDuration,
						Tween.TransitionType.Linear,
						Tween.EaseType.In
						);
		goneAnim.Start();
	}
#pragma warning disable IDE0051
	private void On_goneAnim_tween_all_completed() {
#pragma warning restore IDE0051
		QueueFree();
	}
}
