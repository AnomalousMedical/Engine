/*
 * This source file is part of libRocket, the HTML/CSS Interface Middleware
 *
 * For the latest information, see http://www.librocket.com
 *
 * Copyright (c) 2008-2010 CodePoint Ltd, Shift Technology Ltd
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 */
#include "StdAfx.h"
#include "RenderInterfaceOgre3D.h"
#include <Ogre.h>
#include "CommonResources.h"

 //These are defined in the RocketInterface class
#define MAIN_RESOURCE_GROUP "Rocket.Common"
#define SHARED_RESOURCE_GROUP "Rocket.Shared"

//16 bit index buffer functions
Ogre::HardwareIndexBufferSharedPtr create16BitIndexBuffer(int num_indices);
void fill16BitIndexBuffer(Ogre::HardwareIndexBufferSharedPtr index_buffer, int* indices, int num_indices);

//32 bit index buffer functions
Ogre::HardwareIndexBufferSharedPtr create32BitIndexBuffer(int num_indices);
void fill32BitIndexBuffer(Ogre::HardwareIndexBufferSharedPtr index_buffer, int* indices, int num_indices);

struct RocketOgre3DVertex
{
	float x, y, z;
	Ogre::uint32 diffuse;
	float u, v;
};

// The structure created for each texture loaded by Rocket for Ogre.
struct RocketOgre3DTexture
{
	RocketOgre3DTexture(Ogre::TexturePtr texture) : texture(texture), destroyed(false)
	{
	}

	Ogre::TexturePtr texture;
	bool destroyed;
};

// The structure created for each set of geometry that Rocket compiles. It stores the vertex and index buffers and the
// texture associated with the geometry, if one was specified.
struct RocketOgre3DCompiledGeometry
{
	Ogre::RenderOperation render_operation;
	RocketOgre3DTexture* texture;
};

RenderInterfaceOgre3D::RenderInterfaceOgre3D(unsigned int window_width, unsigned int window_height, QueueBackgroundImageLoad queueBackgroundImageLoad HANDLE_ARG)
	:pixelsPerInch(100),
	pixelScale(1.0f),
	queueBackgroundImageLoad(queueBackgroundImageLoad)
	ASSIGN_HANDLE_INITIALIZER
{
	render_system = Ogre::Root::getSingleton().getRenderSystem();

	// Configure the colour blending mode.
	colour_blend_mode.blendType = Ogre::LBT_COLOUR;
	colour_blend_mode.source1 = Ogre::LBS_DIFFUSE;
	colour_blend_mode.source2 = Ogre::LBS_TEXTURE;
	colour_blend_mode.operation = Ogre::LBX_MODULATE;

	// Configure the alpha blending mode.
	alpha_blend_mode.blendType = Ogre::LBT_ALPHA;
	alpha_blend_mode.source1 = Ogre::LBS_DIFFUSE;
	alpha_blend_mode.source2 = Ogre::LBS_TEXTURE;
	alpha_blend_mode.operation = Ogre::LBX_MODULATE;

	scissor_enable = false;

	scissor_left = 0;
	scissor_top = 0;
	scissor_right = (int)window_width;
	scissor_bottom = (int)window_height;

	textureVertProg = Ogre::HighLevelGpuProgramManager::getSingletonPtr()->getByName("libRocketTextureVS");
	textureVertProg->load();
	textureFragProg = Ogre::HighLevelGpuProgramManager::getSingletonPtr()->getByName("libRocketTexturePS");
	textureFragProg->load();

	noTextureVertProg = Ogre::HighLevelGpuProgramManager::getSingletonPtr()->getByName("libRocketNoTextureVS");
	noTextureVertProg->load();
	noTextureFragProg = Ogre::HighLevelGpuProgramManager::getSingletonPtr()->getByName("libRocketNoTexturePS");
	noTextureFragProg->load();

	//Determine which index buffer functions to use
	if (Ogre::Root::getSingleton().getRenderSystem()->getCapabilities()->hasCapability(Ogre::RSC_32BIT_INDEX))
	{
		createIndexBuffer = &create32BitIndexBuffer;
		fillIndexBuffer = &fill32BitIndexBuffer;
	}
	else
	{
		createIndexBuffer = &create16BitIndexBuffer;
		fillIndexBuffer = &fill16BitIndexBuffer;
	}
}

RenderInterfaceOgre3D::~RenderInterfaceOgre3D()
{
}

// Called by Rocket when it wants to render geometry that it does not wish to optimise.
void RenderInterfaceOgre3D::RenderGeometry(Rocket::Core::Vertex* ROCKET_UNUSED_PARAMETER(vertices), int ROCKET_UNUSED_PARAMETER(num_vertices), int* ROCKET_UNUSED_PARAMETER(indices), int ROCKET_UNUSED_PARAMETER(num_indices), Rocket::Core::TextureHandle ROCKET_UNUSED_PARAMETER(texture), const Rocket::Core::Vector2f& ROCKET_UNUSED_PARAMETER(translation))
{
	ROCKET_UNUSED(vertices);
	ROCKET_UNUSED(num_vertices);
	ROCKET_UNUSED(indices);
	ROCKET_UNUSED(num_indices);
	ROCKET_UNUSED(texture);
	ROCKET_UNUSED(translation);

	// We've chosen to not support non-compiled geometry in the Ogre3D renderer.
}

// Called by Rocket when it wants to compile geometry it believes will be static for the forseeable future.
Rocket::Core::CompiledGeometryHandle RenderInterfaceOgre3D::CompileGeometry(Rocket::Core::Vertex* vertices, int num_vertices, int* indices, int num_indices, Rocket::Core::TextureHandle texture)
{
	RocketOgre3DCompiledGeometry* geometry = new RocketOgre3DCompiledGeometry();
	geometry->texture = texture == NULL ? NULL : (RocketOgre3DTexture*)texture;

	geometry->render_operation.vertexData = new Ogre::VertexData();
	geometry->render_operation.vertexData->vertexStart = 0;
	geometry->render_operation.vertexData->vertexCount = num_vertices;

	geometry->render_operation.indexData = new Ogre::IndexData();
	geometry->render_operation.indexData->indexStart = 0;
	geometry->render_operation.indexData->indexCount = num_indices;

	geometry->render_operation.operationType = Ogre::RenderOperation::OT_TRIANGLE_LIST;


	// Set up the vertex declaration.
	Ogre::VertexDeclaration* vertex_declaration = geometry->render_operation.vertexData->vertexDeclaration;
	size_t element_offset = 0;
	vertex_declaration->addElement(0, element_offset, Ogre::VET_FLOAT3, Ogre::VES_POSITION);
	element_offset += Ogre::VertexElement::getTypeSize(Ogre::VET_FLOAT3);
	vertex_declaration->addElement(0, element_offset, Ogre::VET_COLOUR, Ogre::VES_DIFFUSE);
	element_offset += Ogre::VertexElement::getTypeSize(Ogre::VET_COLOUR);
	vertex_declaration->addElement(0, element_offset, Ogre::VET_FLOAT2, Ogre::VES_TEXTURE_COORDINATES);


	// Create the vertex buffer.
	Ogre::HardwareVertexBufferSharedPtr vertex_buffer = Ogre::HardwareBufferManager::getSingleton().createVertexBuffer(vertex_declaration->getVertexSize(0), num_vertices, Ogre::HardwareBuffer::HBU_STATIC_WRITE_ONLY);
	geometry->render_operation.vertexData->vertexBufferBinding->setBinding(0, vertex_buffer);

	// Fill the vertex buffer.
	RocketOgre3DVertex* ogre_vertices = (RocketOgre3DVertex*)vertex_buffer->lock(0, vertex_buffer->getSizeInBytes(), Ogre::HardwareBuffer::HBL_NORMAL);
	for (int i = 0; i < num_vertices; ++i)
	{
		ogre_vertices[i].x = vertices[i].position.x;
		ogre_vertices[i].y = vertices[i].position.y;
		ogre_vertices[i].z = 0;

		Ogre::ColourValue diffuse(vertices[i].colour.red / 255.0f, vertices[i].colour.green / 255.0f, vertices[i].colour.blue / 255.0f, vertices[i].colour.alpha / 255.0f);
		render_system->convertColourValue(diffuse, &ogre_vertices[i].diffuse);

		ogre_vertices[i].u = vertices[i].tex_coord[0];
		ogre_vertices[i].v = vertices[i].tex_coord[1];
	}
	vertex_buffer->unlock();


	// Create the index buffer.
	Ogre::HardwareIndexBufferSharedPtr index_buffer = createIndexBuffer(num_indices);
	geometry->render_operation.indexData->indexBuffer = index_buffer;
	geometry->render_operation.useIndexes = true;

	// Fill the index buffer.
	fillIndexBuffer(index_buffer, indices, num_indices);
	index_buffer->unlock();


	return reinterpret_cast<Rocket::Core::CompiledGeometryHandle>(geometry);
}

Ogre::HardwareIndexBufferSharedPtr create16BitIndexBuffer(int num_indices)
{
	return Ogre::HardwareBufferManager::getSingleton().createIndexBuffer(Ogre::HardwareIndexBuffer::IT_16BIT, num_indices, Ogre::HardwareBuffer::HBU_STATIC_WRITE_ONLY);
}

void fill16BitIndexBuffer(Ogre::HardwareIndexBufferSharedPtr index_buffer, int* indices, int num_indices)
{
	short* ogre_indices = (short*)index_buffer->lock(0, index_buffer->getSizeInBytes(), Ogre::HardwareBuffer::HBL_NORMAL);
	for (int i = 0; i < num_indices; ++i)
	{
		ogre_indices[i] = indices[i];
	}
}

Ogre::HardwareIndexBufferSharedPtr create32BitIndexBuffer(int num_indices)
{
	return Ogre::HardwareBufferManager::getSingleton().createIndexBuffer(Ogre::HardwareIndexBuffer::IT_32BIT, num_indices, Ogre::HardwareBuffer::HBU_STATIC_WRITE_ONLY);
}

void fill32BitIndexBuffer(Ogre::HardwareIndexBufferSharedPtr index_buffer, int* indices, int num_indices)
{
	void* ogre_indices = index_buffer->lock(0, index_buffer->getSizeInBytes(), Ogre::HardwareBuffer::HBL_NORMAL);
	memcpy(ogre_indices, indices, sizeof(unsigned int) * num_indices);
}

// Called by Rocket when it wants to render application-compiled geometry.
void RenderInterfaceOgre3D::RenderCompiledGeometry(Rocket::Core::CompiledGeometryHandle geometry, const Rocket::Core::Vector2f& translation)
{
	Ogre::Matrix4 transform;
	transform.makeTrans(translation.x, translation.y, 0);
	render_system->_setWorldMatrix(transform);

	render_system = Ogre::Root::getSingleton().getRenderSystem();
	RocketOgre3DCompiledGeometry* ogre3d_geometry = (RocketOgre3DCompiledGeometry*)geometry;

	Ogre::HighLevelGpuProgramPtr vertProg;
	Ogre::HighLevelGpuProgramPtr fragProg;

	if (ogre3d_geometry->texture != NULL && !ogre3d_geometry->texture->texture.isNull())
	{
		render_system->_setTexture(0, true, ogre3d_geometry->texture->texture);

		// Ogre can change the blending modes when textures are disabled - so in case the last render had no texture,
		// we need to re-specify them.
		render_system->_setTextureBlendMode(0, colour_blend_mode);
		render_system->_setTextureBlendMode(0, alpha_blend_mode);

		vertProg = textureVertProg;
		fragProg = textureFragProg;
	}
	else
	{
		render_system->_disableTextureUnit(0);

		vertProg = noTextureVertProg;
		fragProg = noTextureFragProg;
	}

	render_system->bindGpuProgram(vertProg->_getBindingDelegate());
	render_system->bindGpuProgram(fragProg->_getBindingDelegate());

	Ogre::GpuProgramParametersSharedPtr params = vertProg->createParameters();
	params->setNamedConstant("elementWorldViewProj", lastProjectionMatrix * transform);
	render_system->bindGpuProgramParameters(Ogre::GPT_VERTEX_PROGRAM, params, Ogre::GPV_ALL);

	render_system->_render(ogre3d_geometry->render_operation);
}

// Called by Rocket when it wants to release application-compiled geometry.
void RenderInterfaceOgre3D::ReleaseCompiledGeometry(Rocket::Core::CompiledGeometryHandle geometry)
{
	RocketOgre3DCompiledGeometry* ogre3d_geometry = reinterpret_cast<RocketOgre3DCompiledGeometry*>(geometry);
	delete ogre3d_geometry->render_operation.vertexData;
	delete ogre3d_geometry->render_operation.indexData;
	delete ogre3d_geometry;
}

// Called by Rocket when it wants to enable or disable scissoring to clip content.
void RenderInterfaceOgre3D::EnableScissorRegion(bool enable)
{
	scissor_enable = enable;

	if (!scissor_enable)
		render_system->setScissorTest(false);
	else
		render_system->setScissorTest(true, scissor_left, scissor_top, scissor_right, scissor_bottom);
}

// Called by Rocket when it wants to change the scissor region.
void RenderInterfaceOgre3D::SetScissorRegion(int x, int y, int width, int height)
{
	scissor_left = x;
	scissor_top = y;
	scissor_right = x + width;
	scissor_bottom = y + height;

	if (scissor_enable)
		render_system->setScissorTest(true, scissor_left, scissor_top, scissor_right, scissor_bottom);
}

// Called by Rocket when a texture is required by the library.
bool RenderInterfaceOgre3D::LoadTexture(Rocket::Core::TextureHandle& texture_handle, Rocket::Core::Vector2i& texture_dimensions, const Rocket::Core::String& source)
{
	try
	{
		Ogre::String ogreSource = source.CString();
		if (source.Empty())
		{
			return false;
		}

		Ogre::TextureManager* texture_manager = Ogre::TextureManager::getSingletonPtr();
		Ogre::TexturePtr ogre_texture = texture_manager->getByName(ogreSource);
		if (ogre_texture.isNull())
		{
			try
			{
				if (queueBackgroundImageLoad != NULL)
				{
					RocketOgre3DTexture* tex = new RocketOgre3DTexture(Ogre::TexturePtr());
					Vector2i size;
					if (queueBackgroundImageLoad(source.CString(), tex, size))
					{
						texture_dimensions = size.toVector2i();
						texture_handle = reinterpret_cast<Rocket::Core::TextureHandle>(tex);

						return true;
					}
					else
					{
						texture_manager->remove(ogreSource); //Remove the texture from ogre, or it will crash trying to find nonexistant textures the second time the same missing texture is accessed
						ogre_texture = texture_manager->load(IMAGE_NOT_FOUND, SHARED_RESOURCE_GROUP, Ogre::TEX_TYPE_2D, 0);

						texture_dimensions.x = ogre_texture->getWidth();
						texture_dimensions.y = ogre_texture->getHeight();

						texture_handle = reinterpret_cast<Rocket::Core::TextureHandle>(new RocketOgre3DTexture(ogre_texture));
					}
				}
				else
				{
					ogre_texture = texture_manager->load(ogreSource, MAIN_RESOURCE_GROUP, Ogre::TEX_TYPE_2D, 0);

					texture_dimensions.x = ogre_texture->getWidth();
					texture_dimensions.y = ogre_texture->getHeight();

					texture_handle = reinterpret_cast<Rocket::Core::TextureHandle>(new RocketOgre3DTexture(ogre_texture));

					return true;
				}
			}
			catch (Ogre::Exception& ex)
			{
				texture_manager->remove(ogreSource); //Remove the texture from ogre, or it will crash trying to find nonexistant textures the second time the same missing texture is accessed
				ogre_texture = texture_manager->load(IMAGE_NOT_FOUND, SHARED_RESOURCE_GROUP, Ogre::TEX_TYPE_2D, 0);

				texture_dimensions.x = ogre_texture->getWidth();
				texture_dimensions.y = ogre_texture->getHeight();

				texture_handle = reinterpret_cast<Rocket::Core::TextureHandle>(new RocketOgre3DTexture(ogre_texture));

				return true;
			}
		}
	}
	catch (Ogre::Exception& ex)
	{

	}

	return false;
}

// Called by Rocket when a texture is required to be built from an internally-generated sequence of pixels.
bool RenderInterfaceOgre3D::GenerateTexture(Rocket::Core::TextureHandle& texture_handle, const Rocket::Core::byte* source, const Rocket::Core::Vector2i& source_dimensions)
{
	static int texture_id = 1;

	Ogre::DataStreamPtr data_stream(new Ogre::MemoryDataStream((void*)source, source_dimensions.x * source_dimensions.y * sizeof(unsigned int)));

	Ogre::TexturePtr ogre_texture = Ogre::TextureManager::getSingleton().loadRawData(Rocket::Core::String(16, "%d", texture_id++).CString(),
		MAIN_RESOURCE_GROUP,
		data_stream,
		source_dimensions.x,
		source_dimensions.y,
		Ogre::PF_A8B8G8R8,
		Ogre::TEX_TYPE_2D,
		0);

	if (ogre_texture.isNull())
		return false;

	texture_handle = reinterpret_cast<Rocket::Core::TextureHandle>(new RocketOgre3DTexture(ogre_texture));
	return true;
}

// Called by Rocket when a loaded texture is no longer required.
void RenderInterfaceOgre3D::ReleaseTexture(Rocket::Core::TextureHandle texture)
{
	RocketOgre3DTexture* ogreTex = (RocketOgre3DTexture*)texture;
	if (ogreTex->texture.isNull())
	{
		//Texture null, don't delete, but tell the RocketOgreTexture it is destroyed
		ogreTex->destroyed = true;
	}
	else
	{
		//Texture not null, normal delete
		Ogre::TextureManager::getSingleton().remove(ogreTex->texture->getName());
		delete ((RocketOgre3DTexture*)texture);
	}
}

// Returns the native horizontal texel offset for the renderer.
float RenderInterfaceOgre3D::GetHorizontalTexelOffset()
{
	return -render_system->getHorizontalTexelOffset();
}

// Returns the native vertical texel offset for the renderer.
float RenderInterfaceOgre3D::GetVerticalTexelOffset()
{
	return -render_system->getVerticalTexelOffset();
}

// Configures Ogre's rendering system for rendering Rocket.
void RenderInterfaceOgre3D::ConfigureRenderSystem(const int &renderWidth, const int &renderHeight, const bool &requiresTextureFlipping)
{
	Ogre::RenderSystem* render_system = Ogre::Root::getSingleton().getRenderSystem();

	// Set up the projection and view matrices.
	BuildProjectionMatrix(lastProjectionMatrix, renderWidth, renderHeight, requiresTextureFlipping);
	render_system->_setProjectionMatrix(lastProjectionMatrix);
	render_system->_setViewMatrix(Ogre::Matrix4::IDENTITY);

	// Disable lighting, as all of Rocket's geometry is unlit.
	render_system->setLightingEnabled(false);
	// Disable depth-buffering; all of the geometry is already depth-sorted.
	render_system->_setDepthBufferParams(false, false);
	// Rocket generates anti-clockwise geometry, so enable clockwise-culling.
	render_system->_setCullingMode(Ogre::CULL_CLOCKWISE);
	// Disable fogging.
	render_system->_setFog(Ogre::FOG_NONE);
	// Enable writing to all four channels.
	render_system->_setColourBufferWriteEnabled(true, true, true, true);
	// Unbind any vertex or fragment programs bound previously by the application.
	//render_system->unbindGpuProgram(Ogre::GPT_FRAGMENT_PROGRAM);
	//render_system->unbindGpuProgram(Ogre::GPT_VERTEX_PROGRAM);

	// Set texture settings to clamp along both axes.
	Ogre::TextureUnitState::UVWAddressingMode addressing_mode;
	addressing_mode.u = Ogre::TextureUnitState::TAM_CLAMP;
	addressing_mode.v = Ogre::TextureUnitState::TAM_CLAMP;
	addressing_mode.w = Ogre::TextureUnitState::TAM_CLAMP;
	render_system->_setTextureAddressingMode(0, addressing_mode);

	// Set the texture coordinates for unit 0 to be read from unit 0.
	render_system->_setTextureCoordSet(0, 0);
	// Disable texture coordinate calculation.
	render_system->_setTextureCoordCalculation(0, Ogre::TEXCALC_NONE);
	// Enable linear filtering; images should be rendering 1 texel == 1 pixel, so point filtering could be used
	// except in the case of scaling tiled decorators.
	render_system->_setTextureUnitFiltering(0, Ogre::FO_LINEAR, Ogre::FO_LINEAR, Ogre::FO_POINT);
	// Disable texture coordinate transforms.
	render_system->_setTextureMatrix(0, Ogre::Matrix4::IDENTITY);
	// Reject pixels with an alpha of 0.
	render_system->_setAlphaRejectSettings(Ogre::CMPF_GREATER, 0, false);
	// Disable all texture units but the first.
	render_system->_disableTextureUnitsFrom(1);

	// Enable simple alpha blending.
	render_system->_setSceneBlending(Ogre::SBF_SOURCE_ALPHA, Ogre::SBF_ONE_MINUS_SOURCE_ALPHA);

	// Disable depth bias.
	render_system->_setDepthBias(0, 0);
}

// Builds an OpenGL-style orthographic projection matrix.
void RenderInterfaceOgre3D::BuildProjectionMatrix(Ogre::Matrix4& projection_matrix, const int &renderWidth, const int &renderHeight, const bool &requiresTextureFlipping)
{
	float z_near = -1;
	float z_far = 1;

	projection_matrix = Ogre::Matrix4::ZERO;

	// Set up matrices.
	projection_matrix[0][0] = 2.0f / renderWidth;// (the window width)
	projection_matrix[0][3] = -1.0000000f;
	projection_matrix[1][1] = -2.0f / renderHeight;//(the window height)
	projection_matrix[1][3] = 1.0000000f;
	projection_matrix[2][2] = -2.0f / (z_far - z_near);
	projection_matrix[3][3] = 1.0000000f;

	if (requiresTextureFlipping)
	{
		projection_matrix[1][1] = -projection_matrix[1][1];
		projection_matrix[1][3] = -projection_matrix[1][3];
	}
}

/// Returns the number of pixels per inch.
/// @returns The number of pixels per inch. The default implementation returns 100.
float RenderInterfaceOgre3D::GetPixelsPerInch()
{
	return pixelsPerInch;
}

/// Sets the number of pixels per inch.
void RenderInterfaceOgre3D::SetPixelsPerInch(float ppi)
{
	pixelsPerInch = ppi;
}

/// Returns the amount to scale spx by.
/// @returns The amount to scale spx by. The default implementation returns 1.0.
float RenderInterfaceOgre3D::GetPixelScale()
{
	return pixelScale;
}

/// Set the pixel scale
void RenderInterfaceOgre3D::SetPixelScale(float scale)
{
	pixelScale = scale;
}

extern "C" _AnomalousExport RenderInterfaceOgre3D* RenderInterfaceOgre3D_Create(int width, int height, QueueBackgroundImageLoad queueBackgroundImageLoad HANDLE_ARG)
{
	return new RenderInterfaceOgre3D(width, height, queueBackgroundImageLoad PASS_HANDLE_ARG);
}

extern "C" _AnomalousExport void RenderInterfaceOgre3D_Delete(RenderInterfaceOgre3D* renderInterface)
{
	delete renderInterface;
}

extern "C" _AnomalousExport void RenderInterfaceOgre3D_ConfigureRenderSystem(RenderInterfaceOgre3D* renderInterface, int &renderWidth, int &renderHeight, bool &requiresTextureFlipping)
{
	renderInterface->ConfigureRenderSystem(renderWidth, renderHeight, requiresTextureFlipping);
}

extern "C" _AnomalousExport float RenderInterfaceOgre3D_GetPixelsPerInch(RenderInterfaceOgre3D* renderInterface)
{
	return renderInterface->GetPixelsPerInch();
}

/// Sets the number of pixels per inch.
extern "C" _AnomalousExport void RenderInterfaceOgre3D_SetPixelsPerInch(RenderInterfaceOgre3D* renderInterface, float ppi)
{
	renderInterface->SetPixelsPerInch(ppi);
}

extern "C" _AnomalousExport float RenderInterfaceOgre3D_GetPixelScale(RenderInterfaceOgre3D* renderInterface)
{
	return renderInterface->GetPixelScale();
}

/// Sets the pixel scale.
extern "C" _AnomalousExport void RenderInterfaceOgre3D_SetPixelScale(RenderInterfaceOgre3D* renderInterface, float scale)
{
	renderInterface->SetPixelScale(scale);
}

extern "C" _AnomalousExport void RenderInterfaceOgre3D_finishTextureLoad(RocketOgre3DTexture* rocketTexture, Ogre::TexturePtr* texturePtr)
{
	if (rocketTexture->destroyed)
	{
		delete rocketTexture;
	}
	else if (texturePtr != nullptr)
	{
		rocketTexture->texture = *texturePtr;
	}
}