using System.Drawing;
using System.Windows.Forms;

namespace CommandsEditor
{
    internal static class SharedFormIcon
    {
        private static Icon _icon;

        internal static Icon Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = SystemIcons.Application;
                    try
                    {
                        var executableIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                        if (executableIcon != null)
                        {
                            _icon = executableIcon;
                        }
                    }
                    catch
                    {
                        // Fallback stays as SystemIcons.Application.
                    }
                }

                return _icon;
            }
        }
    }
}
