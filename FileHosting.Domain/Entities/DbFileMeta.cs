﻿using FileHosting.Domain.Attributes;

namespace FileHosting.Domain.Entities;

public class DbFileMeta
{
    public Guid Guid { get; set; }
    public long Size { get; set; }
    public string Name { get; set; } = string.Empty;
}