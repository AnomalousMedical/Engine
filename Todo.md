# TODO

## Make Emissive colors work
The emissive lighting is not applied to the object that actually does the lighting. Sample the emissive shader and add it.

## Make a reflectiveness map
Make a reflectiveness map that can work like metalness (on or off), but change whether or not this part of the object casts reflection rays

## Add occlusion map
Add the occlusion maps to at least see what they look like.

## Fix Map Mesh Normals
The map mesh gets a bit triangly for some shadows. This is because the normals need to be averaged for each vertex. Once this is done the lighting should smooth out. As it is
now each triangle is similar to the face of a cube and lights in the same way. Thats why the triangles appear.

The meshes themselves seem to be converted between coords ok, this issue is related to the averaging.

## Diligent Engine BLAS String Fix
This is fixed in the anomalous diligent branch.

This will show up as sometimes blases not finding their geometry. There will be a log error that the hashes don't match and debug asserts. The objects will also not show in the scene. This is because strings are being deallocated coming across p/invoke. The fix requires a DiligentEngine modification.

Currently the diligent engine needs a small patch to work with c#. In Graphics/GraphicsEngine/src/BottomLevelASBase.cpp change line ~139 in `CopyBLASGeometryDesc` from
```
bool IsUniqueName = DstNameToIndex.emplace(SrcGeoName, BLASGeomIndex{i, ActualIndex}).second;
```
to
```
bool IsUniqueName = DstNameToIndex.emplace(pTriangles[i].GeometryName, BLASGeomIndex{i, ActualIndex}).second;
```

This also needs to be done to a similar line for the boxes on ~174
```
bool IsUniqueName = DstNameToIndex.emplace(pBoxes[i].GeometryName, BLASGeomIndex{i, ActualIndex}).second;
```

This is an issue where sometimes the blas GeometryNames from CreateBLAS are already garbage collected before we can call BuildBLAS. This is a problem anyway, since we don't want
to use strings that are gc'd on the native side. The fix here is that the description is creating copies of all the strings anyway. Instead of using the original string from c#
this mod changes it to use the copied string. That keeps the memory allocated. It also does not cause a leak since the "desc" lives with the object. This has the advantage of allocating
the string with diligent's allocater too.

The comment string below are some more notes about the problem, but this is the issue and fix.

//TODO: There is a potential problem here where the managed strings are deallocated, then diligent will error with "Cube" hashes the same as "" and then be unable to find the geometry. Need to figure out how to use strings with pass structs.
//There is a hint in it is the 'RHS.Str' that is messed up
//This is from m_NameToIndex, so it is the one being stored earlier when calling CreateBLAS, not the one from this line
//So it is string gc, but we need to make a copy for CreateBLAS so it can be used here later in BuildBLAS
//In BottomLevelASBase.cpp line 139 in CopyBLASGeometryDesc. You can see the string being added to the hash map is the
//one we are passing from c++. A Copy is made, so can we use that copy in the struct instead.

## Make border rooms transition
If a room is on a border it will get a barrier and prevent going to the next room. Either ensure there is always a 1x1 grid around the outside (best) or put the corridor id in place of a room tile

## Add north - south transitions
Make it so levels can go north and south too

## Add variable width and height to levels
Levels are hardcoded to 150, increase this size and allow it to change.

## Smooth out player transition
The player will stop a bit between levels. See if the velocity needs to be preserved or something.

## Level -65271249 puts start location in a bad spot
This level will make the connection in a corridor, since the easternmost room could have an
eastern corridor this will have to be delt with somehow. Same for other directions too.

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