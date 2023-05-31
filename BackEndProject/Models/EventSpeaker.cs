﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models;

public class EventSpeaker
{
    public int Id { get; set; } 
    public int EventId { get; set; }
    public int SpeakerId { get; set; }
    public Event Event { get; set; }
    public Speaker Speaker { get; set; }
}
