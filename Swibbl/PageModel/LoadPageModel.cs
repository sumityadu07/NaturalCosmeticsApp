using Swibbl.Services;
using System;

namespace Swibbl.Pages
{
    public class LoadPageModel : FreshMvvm.FreshBasePageModel
    {
        public LoadPageModel()
        {

        }
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            CoreMethods.SwitchOutRootNavigation(NavigationStack.MainAppStack);
        }
    }
}
