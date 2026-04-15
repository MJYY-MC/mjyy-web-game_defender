using Godot;
using System;

public class Bullet : RigidBody2D
{
	private Bullet_GoneAnim goneAnim;
	public override void _Ready() {
		goneAnim = GetNode<Bullet_GoneAnim>("goneAnim");

		goneAnim.PlayShowAnim(this);
	}

	/// <summary>
	/// 是否正在执行消失
	/// </summary>
	private bool doingGone = false;
#pragma warning disable IDE0051
	private void OnBulletBody_entered(object body) {
#pragma warning restore IDE0051
		if (body is Node nodeObj) {

			if (nodeObj.Name=="border" && !doingGone) {
				doingGone = true;
				goneAnim.PlayGoneAnim(this);
			}
		}
	}
}
