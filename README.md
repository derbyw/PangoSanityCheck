# PangoSanityCheck

Application to test memory leaks in Eto.Forms running under GTK3 and "control" GTK2 version

I have created an simple project that demonstrates the slow leak I'm seeing in my Eto.Forms apps.

The solution builds both GTK2 and GTK3 versions of the application built against the 2.3 Eto packages from nuget

To test: hit the "Start/Stop" button which will start the display updates and watch the Process memory line.

As the updates run, the app will display both the GC and private (Process) memory of the app. The private memory 
includes non-managed memory allocations. The app shows the change since start as well as totals for each. The app 
*should* allocate memory when the updates first start and then stablize to a steady allocation and stay there. This 
is what occurs on the GTK2 app but the GTK3 version leaks and allocates more process memory every few seconds. 

I have tested this on Ubuntu 16.04 and Ubunto 17.10 as well as an embedded system with all the same results.

