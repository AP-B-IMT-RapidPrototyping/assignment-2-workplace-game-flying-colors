using Godot;
using System;

public partial class ELabel : Control
{
    public override void _Ready()
    {
        this.Visible = false;
    }

	public void ChangeState(Node3D interactible, bool lookingAt)
	{
		if (lookingAt)
		{
			this.Visible = true;
		} else
		{
			this.Visible = false;
		}
	}
}
