namespace Pentagon.Extensions.Localization.Json {
    using System.ComponentModel;

    public enum JsonFormat
    {
        Unspecified,

        [Description("keyValue")]
        KeyValue,

        [Description("tree")]
        Tree
    }
}