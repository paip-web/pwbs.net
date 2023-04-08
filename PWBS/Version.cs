namespace PWBS;

/// <summary>
/// Version Object
/// </summary>
public class Version: IComparable<Version>
{
    #region Variables
    /// <summary>
    /// Version String
    /// </summary>
    private string _version;
    /// <summary>
    /// Version Array
    /// </summary>
    private int[] VersionArray => _version.Split('.').Select(int.Parse).ToArray();
    #endregion
    
    #region Constructor
    /// <summary>
    /// Version Constructor
    /// </summary>
    /// <param name="version">Version String</param>
    /// <exception cref="ArgumentException">If version string is invalid</exception>
    public Version(string version)
    {
        _version = ValidateVersion(version);
    }
    #endregion
    
    #region Version String
    /// <summary>
    /// Validate Version String
    /// </summary>
    /// <param name="version">Version String</param>
    /// <returns>If version string is valid</returns>
    /// <exception cref="ArgumentException">If version string is invalid</exception>
    private static string ValidateVersion(string version)
    {
        var versionArray = version.Split('.').Select(int.Parse).ToArray();
        return versionArray.Length switch
        {
            0 => throw new ArgumentException("Version string is empty"),
            > 4 => throw new ArgumentException("Version string is too long"),
            _ => version
        };
    }
    
    /// <summary>
    /// Set new Version String
    /// </summary>
    /// <param name="version">New Version</param>
    public void SetVersion(string version)
    {
        _version = ValidateVersion(version);
    }
    
    /// <summary>
    /// Get Version String
    /// </summary>
    /// <returns>Version String</returns>
    public string GetVersion()
    {
        return _version;
    }

    /// <summary>
    /// Convert to String
    /// </summary>
    /// <returns>Version String</returns>
    public override string ToString()
    {
        return GetVersion();
    }  
    #endregion
    
    #region Compare
    /// <summary>
    /// Compare two Version objects
    /// </summary>
    /// <param name="other">Other Version Object</param>
    /// <returns>
    /// 1 if this instance is greater than other.
    /// 0 if this instance is equal to other.
    /// -1 if this instance is less than other.
    /// </returns>
    public int CompareTo(Version? other)
    {
        // If other is not a valid object reference, this instance is greater.
        if (other == null) return 1;

        for (var i = 0; i < VersionArray.Length; i++)
        {
            if (VersionArray[i] > other.VersionArray[i]) return 1;
            if (VersionArray[i] < other.VersionArray[i]) return -1;
        }

        return 0;
    }

    public static bool operator > (Version operand1, Version operand2)
    {
        return operand1.CompareTo(operand2) > 0;
    }

    public static bool operator < (Version operand1, Version operand2)
    {
        return operand1.CompareTo(operand2) < 0;
    }

    public static bool operator >= (Version operand1, Version operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    public static bool operator <= (Version operand1, Version operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }
    #endregion
}