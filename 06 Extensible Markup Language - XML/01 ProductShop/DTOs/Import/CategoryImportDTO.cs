﻿using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("Category")]
    public class CategoryImportDTO
    {
        [XmlElement("name")]
        public string? Name { get; set; } 
    }
}
