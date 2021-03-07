# TODO

## Level hole
Random seen 40 or 19 has a hole in the geometry. The collision seems ok though.

## Don't forget mipmaps
Have mipmaps, but need to renormalize normals when creating normal mipmaps.

## Refactor Sprites to have better vertex shader
Right now the sprite info is hacked into the bone matrix. Refactor the shader to have better input for sprites and take out what it doesnt need like the bones

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

## Tabs in fonts still need to be figured out
Haven't tried tabs yet in the fonts. They are their own setting so need to mess with it.

## Use new in keyword
Switch ref to in where possible and pass as many structs as possible with in. See if things like Vector3.Forward can be made readonly then.

## Level Generation - boundaryCubeCenterPoints will add extra cubes
If you keep this method of boundary cubes know that L shaped corners will get an extra cube. Should try to prevent this for less per frame physics work.

----------------------------------------------------------------------------------------------------------------------------------------------------------------

# Low Priority
Stuff that is partially solved or maybe doesn't matter.

## Physics character input seems backward
The input for the charcter seems to need a reversed x axis to work. Left / right are +1 and -1 vs -1 and + as would be expected.
Things collide correctly, so it must be right, but its strange.

This is currently fixed in the CharacterMover, since this is part of the physics this is should be enough for now.

## Figure out why camera position is backward
In order to move the camera through the world in the same coords as the rest the position must be inverted before setting the shader. This is
handled in pbr camera and lights.

This is fixed in the pbr renderer when setting position via vector3, quaternion. Probably not a big deal for now. Everything else seems right.

## Figure out why orientation has to be inverted from physics to rendering
This is all handled by the pbr renderer, but it is strange that rotations have to be inverted to render correctly.

Seems to work, but not sure what the deal is