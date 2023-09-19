﻿using System.ComponentModel.DataAnnotations;

namespace DomainEntities;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Department { get; set; }

    public DateTime DateOfBirth { get; set; }
}