﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AUT03_01_API_Discos.Models
{
    [Table("Album")]
    [Index("ArtistId", Name = "IFK_AlbumArtistId")]
    public partial class Album
    {
        public Album()
        {
            Tracks = new HashSet<Track>();
        }

        [Key]
        public int AlbumId { get; set; }
        [Required]
        [StringLength(160)]
        public string Title { get; set; }
        public int ArtistId { get; set; }

        [ForeignKey("ArtistId")]
        [InverseProperty("Albums")]
        public virtual Artist Artist { get; set; }
        [InverseProperty("Album")]
        public virtual ICollection<Track> Tracks { get; set; }
    }
}