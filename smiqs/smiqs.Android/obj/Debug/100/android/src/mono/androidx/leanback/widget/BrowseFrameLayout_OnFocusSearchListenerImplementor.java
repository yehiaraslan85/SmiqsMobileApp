package mono.androidx.leanback.widget;


public class BrowseFrameLayout_OnFocusSearchListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		androidx.leanback.widget.BrowseFrameLayout.OnFocusSearchListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFocusSearch:(Landroid/view/View;I)Landroid/view/View;:GetOnFocusSearch_Landroid_view_View_IHandler:AndroidX.Leanback.Widget.BrowseFrameLayout/IOnFocusSearchListenerInvoker, Xamarin.AndroidX.Leanback\n" +
			"";
		mono.android.Runtime.register ("AndroidX.Leanback.Widget.BrowseFrameLayout+IOnFocusSearchListenerImplementor, Xamarin.AndroidX.Leanback", BrowseFrameLayout_OnFocusSearchListenerImplementor.class, __md_methods);
	}


	public BrowseFrameLayout_OnFocusSearchListenerImplementor ()
	{
		super ();
		if (getClass () == BrowseFrameLayout_OnFocusSearchListenerImplementor.class)
			mono.android.TypeManager.Activate ("AndroidX.Leanback.Widget.BrowseFrameLayout+IOnFocusSearchListenerImplementor, Xamarin.AndroidX.Leanback", "", this, new java.lang.Object[] {  });
	}


	public android.view.View onFocusSearch (android.view.View p0, int p1)
	{
		return n_onFocusSearch (p0, p1);
	}

	private native android.view.View n_onFocusSearch (android.view.View p0, int p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
