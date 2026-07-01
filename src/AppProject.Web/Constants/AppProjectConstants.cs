using System;

namespace AppProject.Web.Constants;

public class AppProjectConstants
{
    public const string ProjectName = "Inventory";

    public const string LocalStorageKeyPrefix = $"{ProjectName}";

    public const string LanguageLocalStorageKey = $"{LocalStorageKeyPrefix}Language";

    public const string DefaultLanguage = "en-US";
}
