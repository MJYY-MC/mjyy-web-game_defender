using Godot;
using System;

public class Bullet : RigidBody2D
{
	public override void _Ready() {
		goneAnim = GetNode<Tween>("goneAnim");

		doGoneAnimTimer = goneAnim.GetNode<Timer>("doGoneAnimTimer");

		PlayShowAnim();
	}

#pragma warning disable IDE0051
	private void OnBulletBody_entered(object body) {
#pragma warning restore IDE0051
		if (body is Node nodeObj) {
			if (nodeObj.IsInGroup("bulletTarget")) {
				PlayGoneAnim();
			}
		}
	}

#pragma warning disable IDE0051
	private void OnGoneTimer_timeout() =>
#pragma warning restore IDE0051
		PlayGoneAnim();



	#region playGoneAnim
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


	Tween goneAnim;
	Timer doGoneAnimTimer;
	byte doGoneAnimTimer_code = 0;
#pragma warning disable IDE0051
	private void On_doGoneAnimTimer_timeout() {
#pragma warning restore IDE0051
		PlayGoneAnim(doGoneAnimTimer_code);
	}

	/// <summary>
	/// 是否正在执行消失
	/// </summary>
	private bool doingGone = false;
	public void PlayGoneAnim(byte code = 0) {
		switch (code) {
			case 0:
				if (!doingGone) {
					doingGone = true;
					doGoneAnimTimer_code = 1;
					doGoneAnimTimer.WaitTime = GoneAnimationDelay;
					doGoneAnimTimer.Start();
				}
				break;
			case 1:
				goneAnim.InterpolateProperty(
						this,
						"modulate:a",
						this.Modulate.a,
						0f,
						GoneAnimationDuration,
						Tween.TransitionType.Linear,
						Tween.EaseType.In
						);
				goneAnim_tac_code = 1;
				goneAnim.Start();
				break;
		}
	}
	byte goneAnim_tac_code = 0;
#pragma warning disable IDE0051
	private void On_goneAnim_tween_all_completed() {
#pragma warning restore IDE0051
		switch (goneAnim_tac_code) {
			case 1:
				QueueFree();
				break;
		}
	}

	/// <summary>
	/// 出现动画持续时间
	/// </summary>
	[Export] public float ShowAnimationDuration { get; set; } = 0.2f;

	public void PlayShowAnim() {
		goneAnim.InterpolateProperty(
			this,
			"scale",
			Vector2.Zero,
			this.Scale,
			ShowAnimationDuration,
			Tween.TransitionType.Linear,
			Tween.EaseType.In
			);
		goneAnim.Start();
	}
	#endregion
}
