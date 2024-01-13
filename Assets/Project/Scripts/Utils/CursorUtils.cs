using UnityEngine;

public class CursorUtils
{
    #region Cursor mode
    public static void SetCursorNone() => ChangeCursor(CursorLockMode.None);
    public static void SetCursorLocked() => ChangeCursor(CursorLockMode.Locked);
    public static void SetCursorConfined() => ChangeCursor(CursorLockMode.Confined);
    private static void ChangeCursor(CursorLockMode _cursorLockMode) => Cursor.lockState = _cursorLockMode;

    public static bool IsCursorNone() => IsCursorMode(CursorLockMode.None);
    public static bool IsCursorLocked() => IsCursorMode(CursorLockMode.Locked);
    public static bool IsCursorConfined() => IsCursorMode(CursorLockMode.Confined);
    private static bool IsCursorMode(CursorLockMode _cursorLockMode) => Cursor.lockState == _cursorLockMode;
    #endregion
}
