using Godot;
using System;

public class Bullet_GoneAnim : Tween
{
	/// <summary>
	/// 消失动画持续时间
	/// </summary>
	[Export] 
	public float GoneAnimationDuration { get; set; } = 0.5f;
	/// <summary>
	/// 执行消失动画前的等待时间
	/// </summary>
	[Export] 
	public float GoneAnimationDelay { get; set; } = 3.0f;

	public async void PlayGoneAnim(Node2D targetNode) {
		await ToSignal(GetTree().CreateTimer(GoneAnimationDelay), "timeout");

		this.InterpolateProperty(
			targetNode,
			"modulate:a", 
			targetNode.Modulate.a, 
			0f, 
			GoneAnimationDuration, 
			Tween.TransitionType.Linear, 
			Tween.EaseType.In
			);
		this.Start();

		await ToSignal(this, "tween_all_completed");

		targetNode.QueueFree();
	}

	/// <summary>
	/// 出现动画持续时间
	/// </summary>
	[Export] public float ShowAnimationDuration { get; set; } = 0.2f;

	public void PlayShowAnim(Node2D targetNode) {
		this.InterpolateProperty(
			targetNode,
			"scale",
			Vector2.Zero,
			targetNode.Scale,
			ShowAnimationDuration,
			Tween.TransitionType.Linear,
			Tween.EaseType.In
			);
		this.Start();
	}
}
