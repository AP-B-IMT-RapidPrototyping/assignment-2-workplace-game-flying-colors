using Godot;
using System;

public partial class checkpoints : Node
{
	private Godot.Collections.Array<Godot.Node> _checkpoints;
	private Godot.Collections.Array<Godot.Node> _items;

	public override void _Ready()
	{
		_checkpoints = GetTree().GetNodesInGroup("checkpoints");
	}

	
	public override void _Process(double delta)
	{
	}
}
