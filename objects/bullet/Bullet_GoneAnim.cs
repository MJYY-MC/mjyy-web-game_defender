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
		/*
		 * 批注：该函数存在await，在游戏结束对象都被销毁时await结束后仍然会执行，会导致其报错：
		 * E 0:00:05.905   _signal_callback: Resumed after await, but class instance is gone.
			<C++ 错误>      Condition "conn_target_id && !ObjectDB::get_instance(conn_target_id)" is true. Returned: Variant()
			<C++ 源文件>     modules/mono/signal_awaiter_utils.cpp:69 @ _signal_callback()
		 * 后续可选的解决方案：使用Timer节点来实现方法
		 */
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
