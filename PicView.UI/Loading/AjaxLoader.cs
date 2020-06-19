using System;
using static PicView.UC;

namespace PicView
{
    public static class AjaxLoader
    {
        //// AjaxLoading
        ///// <summary>
        ///// Loads AjaxLoading and adds it to the window
        ///// </summary>
        //public static void LoadAjaxLoading()
        //{
        //    ajaxLoading = new AjaxLoading
        //    {
        //        Focusable = false,
        //        Opacity = 0
        //    };

        //    mainWindow.bg.Children.Add(ajaxLoading);
        //}

        /// <summary>
        /// Start loading animation
        /// </summary>
        public static void AjaxLoadingStart()
        {
            if (ajaxLoading.Opacity != 1)
            {
                AnimationHelper.Fade(ajaxLoading, 1, TimeSpan.FromSeconds(.2));
            }
        }

        /// <summary>
        /// End loading animation
        /// </summary>
        public static void AjaxLoadingEnd()
        {
            AnimationHelper.Fade(ajaxLoading, 0, TimeSpan.FromSeconds(.2));
        }
    }
}
