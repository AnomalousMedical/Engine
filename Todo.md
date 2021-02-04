# TODO

## Get SURFACE_TRANSFORM from SwapChain to use in calls to GetAdjustedProjectionMatrix
This works with optimal for now, but need to implement this.

## Pool             var pRTV = swapChain.GetCurrentBackBufferRTV(); and var pDSV = swapChain.GetDepthBufferDSV();
 * These make a new value every frame. Pool them, seems to be 1 per buffer, so need more than one. It switches back and forth
 * Find other things that can be adjusted this way