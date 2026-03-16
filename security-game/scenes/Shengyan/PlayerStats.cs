using Godot;
using System;

//Only handles energy for now

public partial class PlayerStats : Node
{
	[Signal]
	public delegate void EnergyChangedEventHandler(int energy);

	public const int MinEnergy = 0;
	public const int MaxEnergy = 100;

	[Export(PropertyHint.Range, "0,100,1")]
	private int _energy = MaxEnergy;

	public int Energy => _energy;

	public void SetEnergy(int energy)
	{
		GD.Print("Set Energy triggered.");
		int clampedEnergy = Mathf.Clamp(energy, MinEnergy, MaxEnergy);
		if (_energy == clampedEnergy)
		{
			return;
		}


		_energy = clampedEnergy;
		EmitSignal(SignalName.EnergyChanged, _energy);
	}

	public void ChangeEnergy(int amount)
	{

		SetEnergy(_energy + amount);
	}
}
