using BlazorApp1.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorApp1.Components.Layout
{
	public partial class MainLayout : LayoutComponentBase
	{
		[Inject] private NavigationManager NavigationManager { get; set; }
		[Inject] private GlobalStateService GlobalState { get; set; }
		protected override void OnInitialized()
		{
			//var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
			//var segments = uri.Segments;

			//if (segments.Length > 1)
			//{
			//	GlobalState.GlobalId = Convert.ToInt32(segments[1].TrimEnd('/'));
			//}

			base.OnInitialized();
		}
	}
}
