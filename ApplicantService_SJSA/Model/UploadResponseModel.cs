﻿namespace Model;

public class UploadResponseModel
{
    public string Id { get; set; } 
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? CloudeKey { get; set; }
}
