# TODO

## Don't forget mipmaps
Make / load mipmaps for things.

## Figure out why orientation has to be inverted from physics to rendering
This is all handled by the pbr renderer, but it is strange that rotations have to be inverted to render correctly.

## Figure out why camera position is backward
In order to move the camera through the world in the same coords as the rest the position must be inverted before setting the shader. This is
handled in pbr camera and lights.

## Refactor Sprites to have better vertex shader
Right now the sprite info is hacked into the bone matrix. Refactor the shader to have better input for sprites and take out what it doesnt need like the bones

## Fix physics timestep
use a fixed timestep with an accumulator, interpolate in between values
keep everything position seen on this frame, throw away results from last frame use this to interpolate unless its too old then just use the current value. assumption is nothing cared about its position until just now
otherwise can we make a copy of the whole simulation since it uses a buffer?

## Figure out if you can keep BodyReference or if BodyHandle should get a new one each time
You can use one BodyReference and its position will change, but some bepu demos show getting it each time. Figure this out.

## Get rid of custom file modes for virtual file system
Engine.Resources.FileMode

## Add wrapping support for material textures
Right now the material textures only work if the dest size is smaller than the source size
make it so the source can be wrapped to get pixels that would lay outside it, the groundwork is there

## GetDeviceCaps_GetNDCAttribs is pretty slow
This will do a copy on the native side into a pass struct then into a class on the managed side. Not so great. See if you can do other stuff.
Seems like you could pass a pass struct as ref then fill it out and have it "returned" that way. Not sure about putting that into a class. That still
seems smart to avoid passing around huge c# structs, even with some overhead. Depends on if its per frame or not (and you could pass the struct in as ref
if you had a big one per frame).

## Computing vertex position a 2nd time in RenderGLTF_PBR.vsh
To fix shadows the locPos had to be calculated again. This makes them work, but its a duplicate of what is already done in the vertex shader transform func, we just
can't get that value.

## Figure out SRGB
Figure out how to deal with srgb. The colors for the UI have been shifted with a ToSrgb function on Color. This can be found easily enough.
No changes to any shaders were made to deal with it. Need to figure out if we want to keep srgb or change to linear rgb. It looks like the gltf
shaders can do either mode with some defines.

## Make backspace better
Make backspace work better, it needs to work on key down not just repeat and key up

## Filter out characters
currently filtering out anything below 32 in the ascii table, could do this in the control, currently at the higher input level

## Tabs in fonts still need to be figrued out
Haven't tried tabs yet in the fonts. They are their own setting so need to mess with it.

## Physics character input seems backward
The input for the charcter seems to need a reversed x axis to work. Left / right are +1 and -1 vs -1 and + as would be expected.
Things collide correctly, so it must be right, but its strange.