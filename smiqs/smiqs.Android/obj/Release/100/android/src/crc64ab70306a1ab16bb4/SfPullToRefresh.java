package crc64ab70306a1ab16bb4;


public class SfPullToRefresh
	extends crc646f5ffa6b71be5cc7.SfPullToRefresh
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Sfpulltorefresh.SfPullToRefresh, Syncfusion.SfPullToRefresh.Android", SfPullToRefresh.class, __md_methods);
	}


	public SfPullToRefresh (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SfPullToRefresh.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.SfPullToRefresh, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public SfPullToRefresh (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SfPullToRefresh.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.SfPullToRefresh, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SfPullToRefresh (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SfPullToRefresh.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.SfPullToRefresh, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SfPullToRefresh (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == SfPullToRefresh.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Sfpulltorefresh.SfPullToRefresh, Syncfusion.SfPullToRefresh.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
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
