# TODO

## Get SURFACE_TRANSFORM from SwapChain to use in calls to GetAdjustedProjectionMatrix
This works with optimal for now, but need to implement this.

## Pool             var pRTV = swapChain.GetCurrentBackBufferRTV(); and var pDSV = swapChain.GetDepthBufferDSV();
 * These make a new value every frame. Pool them, seems to be 1 per buffer, so need more than one. It switches back and forth
 * Find other things that can be adjusted this way

 ## Remove ktx or write c# loader
 Remove ktx or write a c# loader. You won't need any linked libraries when doing this with the engine.

 ## Remove bullet libraris from diligen
 Don't need bullet libraries in diligent wrapper.

 ## Change passing lists to pointers to arrays in diligent wrapper.
  * This will be faster and match other code, can use stackalloc or fixed to make it work
  * This would include getting rid of the take size from stuff.

## Figure out how to create environment maps, load these manually

## Get rid of ktx loader

## Move hardcoded values out of Render in GLTF_PBR_Renderer
There are too many hardcoded values here, move them further out.

##Normals still don't seem 100% 
Chainmail004 shows this pretty well. if you watch an edge pass over the center it looks like the light meets at the edge instead of going along it.
It might be ok too, just need to look at it later.
These might be ok too, with the env map the lighting is more complicated.

## Embed shaders in pbr binary