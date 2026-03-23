namespace CBus;

public static class CBusDefaults
{
    public const int DefaultPort = 3323;
    public const string DefaultPipeAddress = "cbus.pipe";
    public const string DefaultRegistrySubKey = @"Software\CBus";
    public const string HttpPortValueName = "HttpPort";
    public const string PipeAddressFileName = "cbus.pipe";

    public static string DefaultPublishDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CBus");
}
