using System.Runtime.InteropServices;
using EndlessOnlinePatcher.Models;

namespace EndlessOnlinePatcher.Interop;

public static class Windows
{
    public static async Task StartEO()
    {
        await Task.Run(() =>
        {
            RunAsDesktopUser(EndlessOnlineDirectory.GetExe());
        });
    }

    // Launches a process as the desktop (non-elevated) user even when the caller is elevated.
    // Source: https://stackoverflow.com/questions/11169431/how-to-start-a-new-process-without-administrator-privileges-from-a-process-with
    private static void RunAsDesktopUser(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileName));

        var hProcessToken = nint.Zero;
        try
        {
            var process = GetCurrentProcess();
            if (!OpenProcessToken(process, 0x0020, ref hProcessToken))
                return;

            var tkp = new TOKEN_PRIVILEGES
            {
                PrivilegeCount = 1,
                Privileges = new LUID_AND_ATTRIBUTES[1]
            };

            if (!LookupPrivilegeValue("", "SeIncreaseQuotaPrivilege", ref tkp.Privileges[0].Luid))
                return;

            tkp.Privileges[0].Attributes = 0x00000002;

            if (!AdjustTokenPrivileges(hProcessToken, false, ref tkp, 0, nint.Zero, nint.Zero))
                return;
        }
        finally
        {
            CloseHandle(hProcessToken);
        }

        var hwnd = GetShellWindow();
        if (hwnd == nint.Zero)
            return;

        var hShellProcess = nint.Zero;
        var hShellProcessToken = nint.Zero;
        var hPrimaryToken = nint.Zero;
        try
        {
            uint dwPID;
            if (GetWindowThreadProcessId(hwnd, out dwPID) == 0)
                return;

            hShellProcess = OpenProcess(ProcessAccessFlags.QueryInformation, false, dwPID);
            if (hShellProcess == nint.Zero)
                return;

            if (!OpenProcessToken(hShellProcess, 0x0002, ref hShellProcessToken))
                return;

            var dwTokenRights = 395U;

            if (!DuplicateTokenEx(hShellProcessToken, dwTokenRights, nint.Zero, SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, TOKEN_TYPE.TokenPrimary, out hPrimaryToken))
                return;

            var si = new STARTUPINFO();
            var pi = new PROCESS_INFORMATION();
            if (!CreateProcessWithTokenW(hPrimaryToken, 0, fileName, "", 0, nint.Zero, Path.GetDirectoryName(fileName)!, ref si, out pi))
                return;
        }
        finally
        {
            CloseHandle(hShellProcessToken);
            CloseHandle(hPrimaryToken);
            CloseHandle(hShellProcess);
        }
    }

    #region Interop

    private struct TOKEN_PRIVILEGES
    {
        public uint PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES[] Privileges;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    private struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct LUID
    {
        public uint LowPart;
        public int HighPart;
    }

    [Flags]
    private enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VirtualMemoryOperation = 0x00000008,
        VirtualMemoryRead = 0x00000010,
        VirtualMemoryWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }

    private enum SECURITY_IMPERSONATION_LEVEL
    {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation
    }

    private enum TOKEN_TYPE
    {
        TokenPrimary = 1,
        TokenImpersonation
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct PROCESS_INFORMATION
    {
        public nint hProcess;
        public nint hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct STARTUPINFO
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public nint lpReserved2;
        public nint hStdInput;
        public nint hStdOutput;
        public nint hStdError;
    }

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern nint GetCurrentProcess();

    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    private static extern bool OpenProcessToken(nint h, int acc, ref nint phtok);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool LookupPrivilegeValue(string host, string name, ref LUID pluid);

    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    private static extern bool AdjustTokenPrivileges(nint htok, bool disall, ref TOKEN_PRIVILEGES newst, int len, nint prev, nint relen);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(nint hObject);

    [DllImport("user32.dll")]
    private static extern nint GetShellWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, uint processId);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool DuplicateTokenEx(nint hExistingToken, uint dwDesiredAccess, nint lpTokenAttributes, SECURITY_IMPERSONATION_LEVEL impersonationLevel, TOKEN_TYPE tokenType, out nint phNewToken);

    [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool CreateProcessWithTokenW(nint hToken, int dwLogonFlags, string lpApplicationName, string lpCommandLine, int dwCreationFlags, nint lpEnvironment, string lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

    #endregion
}
