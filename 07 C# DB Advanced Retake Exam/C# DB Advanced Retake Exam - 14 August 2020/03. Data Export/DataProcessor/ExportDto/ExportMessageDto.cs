﻿using System.Security.Cryptography;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class ExportMessageDto
    {
        //<Message>
        //<Description>!?sdnasuoht evif-ytnewt rof deksa uoy ro orez artxe na ereht sI</Description>
        [XmlElement("Description")]
        public string Description { get; set; } = null!;



    }
}