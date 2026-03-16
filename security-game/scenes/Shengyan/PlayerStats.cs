using Godot;
using System;

//Only handles energy for now

public partial class PlayerStats : Node
{
	[Signal]
	public delegate void EnergyChangedEventHandler(bool hasEnergy);

	[Export] private bool _hasEnergy = true;

	public bool HasEnergy => _hasEnergy;

	public void SetEnergy(bool hasEnergy)
	{
		if (_hasEnergy == hasEnergy)
		{
			return;
		}

		_hasEnergy = hasEnergy;
		EmitSignal(SignalName.EnergyChanged, _hasEnergy);
	}

	public void ToggleEnergy()
	{
		SetEnergy(!_hasEnergy);
	}
}
