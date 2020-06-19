using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace PicView.OS_Integration
{
    class WindowTaskbar
    {
        #region Progress

        /// <summary>
        /// Show progress on taskbar
        /// </summary>
        /// <param name="i">index</param>
        /// <param name="ii">size</param>
        public static void Progress(int i, int ii)
        {
            TaskbarManager prog = TaskbarManager.Instance;
            prog.SetProgressState(TaskbarProgressBarState.Normal);
            prog.SetProgressValue(i, ii);
        }

        /// <summary>
        /// Stop showing taskbar progress, return to default
        /// </summary>
        public static void NoProgress()
        {
            TaskbarManager prog = TaskbarManager.Instance;
            prog.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        #endregion
    }
}
