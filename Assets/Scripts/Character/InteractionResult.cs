public readonly struct InteractionResult
{
    public readonly int InteractorHealth;
    public readonly int InteractorEnergy;
    public readonly int TargetHealth;
    public readonly int TargetEnergy;

    public InteractionResult(int interactorHealth, int interactorEnergy, int targetHealth, int targetEnergy)
    {
        InteractorHealth = interactorHealth;
        InteractorEnergy = interactorEnergy;
        TargetHealth = targetHealth;
        TargetEnergy = targetEnergy;
    }
}