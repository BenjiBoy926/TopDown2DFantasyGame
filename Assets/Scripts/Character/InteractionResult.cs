public readonly struct InteractionResult
{
    public readonly CharacterInfo Interactor;
    public readonly CharacterInfo Target;

    public InteractionResult(CharacterInfo interactor, CharacterInfo target)
    {
        Interactor = interactor;
        Target = target;
    }
}