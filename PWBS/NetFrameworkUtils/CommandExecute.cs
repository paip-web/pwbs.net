using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace PWBS.NetFrameworkUtils;

/// <summary>
/// Command Execute Class
/// </summary>
public static class CommandExecute
{
    /// <summary>
    /// Execute PowerShell Command
    /// </summary>
    /// <param name="function">\
    /// (pwsh) => {
    ///     pwsh.AddCommand("Get-Command").AddParameter("Name", "bash");
    /// }
    /// </param>
    /// <returns>
    /// Powershell Result Object
    /// </returns>
    private static Collection<PSObject> ExecutePowerShell(
        Action<PowerShell> function
    )
    {
        var pwsh = PowerShell.Create();
        function(pwsh);
        return pwsh.Invoke();
    }
    
    /// <summary>
    /// Check if specified command is available
    /// </summary>
    /// <param name="command">Command to check</param>
    /// <returns>If command is available</returns>
    public static bool IsCommandAvailable(string command)
    {
        var rPowerShell = ExecutePowerShell(
            powerShell => powerShell.AddCommand("Get-Command").AddParameter("Name", command)
        );
        return rPowerShell.Count > 0;
    }
    
    /// <summary>
    /// Check if user is Administrator
    /// </summary>
    /// <param name="unsupportedException">
    /// If true, throw PlatformNotSupportedException if platform is not supported.
    /// If false, return false if platform is not supported.
    /// </param>
    /// <returns>If user is Administrator</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// Thrown if platform is not supported and unsupportedException is true
    /// </exception>
    public static bool IsAdmin(bool unsupportedException = false)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Get Current Identity
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            // Get Built In Admin Role
            var adminRole = System.Security.Principal.WindowsBuiltInRole.Administrator;
            // Get Current Principal from Identity
            var identityPrincipal = new System.Security.Principal.WindowsPrincipal(identity);
            // Check if Current Account is In Admin Role
            return identityPrincipal.IsInRole(adminRole);
        }

        if (!(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)))
        {
            // Unsupported Platform
            return unsupportedException
                ? throw new PlatformNotSupportedException("Can't check if user is admin on this platform")
                : false;
        }
        
        // Linux / OS X
        var uid = UnixUserTools.GetUserId();
        var gid = UnixUserTools.GetGroupId();
        
        if (uid == 0 || gid == 0)
        {
            return true;
        } 
        return false;
    }
    
    /// <summary>
    /// Execute Command
    /// </summary>
    /// <param name="shell">Shell Command</param>
    /// <param name="command">Command to execute</param>
    private static void ExecuteCommand(string shell, string command)
    {
        Process cmd = new Process();
        cmd.StartInfo.FileName = shell;
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.Start();
        cmd.StandardInput.WriteLine(command);
        cmd.StandardInput.Flush();
        cmd.StandardInput.Close();
        cmd.WaitForExit();
        Console.WriteLine(cmd.StandardOutput.ReadToEnd());
    }
    
    /// <summary>
    /// Execute Command as Admin
    /// </summary>
    /// <param name="command">Shell</param>
    /// <param name="arguments">Command to execute</param>
    /// <exception cref="PlatformNotSupportedException">
    /// Thrown if platform is not supported (it's not Windows and if sudo is not available)
    /// </exception>
    private static void ExecuteCommandAsAdmin(string command, string arguments)
    {
        if (IsAdmin())
        {
            ExecuteCommand(command, arguments);
            return;
        }
        
        Process cmd = new Process();
        cmd.StartInfo.FileName = command;
        cmd.StartInfo.Arguments = arguments;
        cmd.StartInfo.UseShellExecute = true;
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            cmd.StartInfo.Verb = "RunAs";
        }
        else
        {
            if (IsCommandAvailable("sudo"))
            {
                cmd.StartInfo.FileName = $"sudo {command}";
            }
            else
            {
                throw new PlatformNotSupportedException("Command 'sudo' not found. Please install it and try again.");
            }
        }

        cmd.Start();
        cmd.WaitForExit();
    }
    
    /// <summary>
    /// Cache for Basic Shell
    /// </summary>
    private static string _basicShell = "";
    
    /// <summary>
    /// Get Basic Shell based on operating system
    /// </summary>
    /// <returns>Shell Command</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// If platform is not supported
    /// </exception>
    private static string GetBasicShell()
    {
        if (!String.IsNullOrEmpty(_basicShell))
        {
            return _basicShell;
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows
            
            // Prefer PowerShell Core
            // Ignore because of Profile Timing
            // if (IsCommandAvailable("pwsh.exe"))
            // {
            //     _basicShell = "pwsh.exe";
            //     return _basicShell;
            // }
            
            // Fallback to CMD
            _basicShell = "cmd.exe";
            return _basicShell;
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Linux
            
            // Prefer PowerShell Core
            // Ignore because of Profile Timing
            // if (IsCommandAvailable("pwsh"))
            // {
            //     _basicShell = "pwsh";
            //     return _basicShell;
            // }
            
            // Then Prefer Bash
            if (IsCommandAvailable("bash"))
            {
                _basicShell = "bash";
                return _basicShell;
            }
            // Then Prefer Zsh
            if (IsCommandAvailable("zsh"))
            {
                _basicShell = "zsh";
                return _basicShell;
            }
            // Fallback to SH
            _basicShell = "sh";
            return _basicShell;
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            // Mac OS X
            
            // Prefer PowerShell Core
            // Ignore because of Profile Timing
            // if (IsCommandAvailable("pwsh"))
            // {
            //     _basicShell = "pwsh";
            //     return _basicShell;
            // }
            
            // Prefer Bash
            if (IsCommandAvailable("bash"))
            {
                _basicShell = "bash";
                return _basicShell;
            }
            
            // Fallback to ZSH or SH
            _basicShell = IsCommandAvailable("zsh") ? "zsh" : "sh";
            return _basicShell;
        }
        
        throw new PlatformNotSupportedException("Unsupported OS Platform");
    }
    
    /// <summary>
    /// Execute specified command
    /// </summary>
    /// <param name="command">command to execute</param>
    public static void Execute(string command)
    {
        ExecuteCommand(GetBasicShell(), command);
    }
    
    /// <summary>
    /// Execute specified command as administrator
    /// </summary>
    /// <param name="command">command to execute as administrator</param>
    public static void ExecuteAsAdmin(string command)
    {
        ExecuteCommandAsAdmin(GetBasicShell(), command);
    }
}