package crc64ab70306a1ab16bb4;


public class HeaderView
	extends android.widget.LinearLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Sfpulltorefresh.HeaderView, Syncfusion.SfPullToRefresh.Android", HeaderView.class, __md_methods);
	}


	public HeaderView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == HeaderView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.HeaderView, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public HeaderView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == HeaderView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.HeaderView, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public HeaderView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == HeaderView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.HeaderView, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public HeaderView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == HeaderView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.HeaderView, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}

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
