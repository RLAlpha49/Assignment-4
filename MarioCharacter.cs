class MarioCharacter
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Species { get; set; }
    public string? FirstAppearance { get; set; }
    public int? YearCreated { get; set; }

    public MarioCharacter(ulong id, string name, string? description, string? species, string? firstAppearance, int? yearCreated)
    {
        Id = id;
        Name = name;
        Description = description;
        Species = species;
        FirstAppearance = firstAppearance;
        YearCreated = yearCreated;
    }
} 