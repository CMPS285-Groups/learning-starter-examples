using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningStarter.Entities;

public class PlayerStats
{
    public int Id { get; set; }
    public int Player { get; set; }
    public string Team { get; set; }
    public int StatType { get; set; }
    public decimal StatValue { get; set; }
    public DateTime GameDate { get; set; }
}

public class PlayerStatsGetDto
{
    public int Id { get; set; }
    public int Player { get; set; }
    public string Team { get; set; }
    public int StatType { get; set; }
    public decimal StatValue { get; set; }
    public DateTime GameDate { get; set; }
}

public class PlayerStatsCreateDto
{
    public int Player { get; set; }
    public string Team { get; set; }
    public int StatType { get; set; }
    public decimal StatValue { get; set; }
    public DateTime GameDate { get; set; }
}

public class PlayerStatsUpdateDto
{
    public int Player { get; set; }
    public string Team { get; set; }
    public int StatType { get; set; }
    public decimal StatValue { get; set; }
    public DateTime GameDate { get; set; }
}

public class PlayerStatsEntityTypeConfiguration : IEntityTypeConfiguration<PlayerStats>
{
    public void Configure(EntityTypeBuilder<PlayerStats> builder)
    {
        builder.ToTable("PlayerStats");
    }
}